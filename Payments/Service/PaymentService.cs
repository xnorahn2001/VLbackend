using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IPayment
{
    Task<PaymentDto> CreatePaymentAsyncService(CreatePaymentDto newPayment);
    Task<List<PaymentDto>> GetPaymentsAsyncService(int pageNumber, int pageSize, string searchQuery);
    Task<PaymentDto?> GetPaymentByIdAsyncService(Guid paymentId);
    Task<bool> DeletePaymentByIdAsyncService(Guid paymentId);
    Task<PaymentDto?> UpdatePaymentByIdAsyncService(Guid paymentId, UpdatePaymentDto updatePayment);
}

public class PaymentService : IPayment
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
            var customer = await _appDbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == newPayment.CustomerId);
            if (customer == null)
            {
                throw new Exception("this customer does not exisit");
            }
            payment.Customer = customer;
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
            var payments = await _appDbContext.Payments.Include(p => p.Customer).ToListAsync();
            // using query to search for all the customers whos matching the name otherwise return null
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
            payment.TotalPrice = updatePayment.TotalPrice ?? payment.TotalPrice;

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