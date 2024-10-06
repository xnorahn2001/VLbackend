using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IPaymentService
{
    Task<PaymentDto> CreatePaymentAsyncService(CreatePaymentDto newPayment);
    Task<List<PaymentDto>> GetPaymentsAsyncService(int pageNumber, int pageSize, string searchQuery);
    Task<PaymentDto?> GetPaymentByIdAsyncService(Guid paymentId);
    Task<bool> DeletePaymentByIdAsyncService(Guid paymentId);
    Task<PaymentDto?> UpdatePaymentByIdAsyncService(Guid paymentId, UpdatePaymentDto updatePayment);
}

public class PaymentService : IPaymentService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public PaymentService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<PaymentDto> CreatePaymentAsyncService(CreatePaymentDto newPayment)
    {
        try
        {
            var payment = _mapper.Map<Payment>(newPayment);

            var paymentUser = payment.User;
            paymentUser = await _appDbContext.Users.FirstOrDefaultAsync(c => c.UserId == newPayment.UserId);
            if (paymentUser == null)
            {
                throw new Exception($"There is no user with this Id {newPayment.UserId}");
            }

            var paymentOrder = payment.Order;
            paymentOrder = await _appDbContext.Orders.FirstOrDefaultAsync(c => c.OrderId == newPayment.OrderId);
            if (paymentUser == null)
            {
                throw new Exception($"There is no order with this Id {newPayment.OrderId}");
            }

            payment.TotalPrice = payment.Order.TotalPrice;
            var paymentAdded = await _appDbContext.Payments.AddAsync(payment);
            await _appDbContext.Payments.AddAsync(payment);
            await _appDbContext.SaveChangesAsync();
            var paymentData = _mapper.Map<PaymentDto>(payment);
            return paymentData;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<List<PaymentDto>> GetPaymentsAsyncService(int pageNumber, int pageSize, string searchQuery)
    {
        try
        {
            var payments = await _appDbContext.Payments.Include(p => p.User).Include(p => p.Order).ToListAsync();
            // using query to search for all the users whos matching the name otherwise return null
            var filterPayments = payments.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterPayments = filterPayments.Where(a => a.CardNumber.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                if (filterPayments.Count() == 0)
                {
                    var exisitingPayment = _mapper.Map<List<PaymentDto>>(filterPayments);
                    return exisitingPayment;
                }
            }
            // return the pagination result
            var paginationResult = filterPayments.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var paymentData = _mapper.Map<List<PaymentDto>>(paginationResult);
            return paymentData;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }

    }
    public async Task<PaymentDto?> GetPaymentByIdAsyncService(Guid paymentId)
    {
        try
        {
            var payment = await _appDbContext.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                return null;
            }
            var paymentData = _mapper.Map<PaymentDto>(payment);
            return paymentData;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<bool> DeletePaymentByIdAsyncService(Guid paymentId)
    {
        try
        {
            var payment = await _appDbContext.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                return false;
            }
            _appDbContext.Payments.Remove(payment);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<PaymentDto?> UpdatePaymentByIdAsyncService(Guid paymentId, UpdatePaymentDto updatePayment)
    {
        try
        {
            var payment = await _appDbContext.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                return null;
            }

            payment.PaymentMethod = updatePayment.PaymentMethod ?? payment.PaymentMethod;
            payment.CardNumber = updatePayment.CardNumber ?? payment.CardNumber;

            _appDbContext.Payments.Update(payment);
            await _appDbContext.SaveChangesAsync();

            var paymentData = _mapper.Map<PaymentDto>(payment);
            return paymentData;
        }
        catch (DbUpdateException dbEx)
        {
            // Handle database update exceptions
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }
}