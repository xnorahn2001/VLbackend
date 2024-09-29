using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class CustomerService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public CustomerService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<Customer> CreateCustomerAsyncService(CreateCustomerDto newCustomer)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newCustomer.Password);
            newCustomer.Password = hashedPassword;
            var customer = _mapper.Map<Customer>(newCustomer);
            await _appDbContext.Customers.AddAsync(customer);
            await _appDbContext.SaveChangesAsync();
            return customer;
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

    public async Task<List<CustomerDto>> GetCustomersAsyncService()
    {
        try
        {
            var customers = await _appDbContext.Customers.ToListAsync();
            var customerData = _mapper.Map<List<CustomerDto>>(customers);
            return customerData;
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
    public async Task<CustomerDto?> GetCustomerByAsyncService(Guid customerId)
    {
        try
        {
            var customer = await _appDbContext.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return null;
            }
            var customerData = _mapper.Map<CustomerDto>(customer);
            return customerData;
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

    public async Task<bool> DeleteCustomerByIdAsyncService(Guid customerId)
    {
        try
        {
            var customer = await _appDbContext.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return false;
            }
            _appDbContext.Customers.Remove(customer);
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

    public async Task<CustomerDto?> UpdateCustomerByIdAsyncService(Guid customerId, Customer updateCustomer)
    {
        try
        {
            var customer = await _appDbContext.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return null;
            }

            customer.FirstName = updateCustomer.FirstName ?? customer.FirstName;
            customer.LastName = updateCustomer.LastName ?? customer.LastName;
            customer.Email = updateCustomer.Email ?? customer.Email;
            if (updateCustomer.Password != null)
            {
                customer.Password = BCrypt.Net.BCrypt.HashPassword(updateCustomer.Password) ?? customer.Password;
            }
            customer.Phone = updateCustomer.Phone ?? customer.Phone;

            _appDbContext.Update(customer);
            await _appDbContext.SaveChangesAsync();

            var customerData = _mapper.Map<CustomerDto>(customer);
            return customerData;
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