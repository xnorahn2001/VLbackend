using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface ICustomer
{
    Task<CustomerDto> CreateCustomerAsyncService(CreateCustomerDto newCustomer);
    Task<List<CustomerDto>> GetCustomersAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder);
    Task<CustomerDto?> GetCustomerByIdAsyncService(Guid customerId);
    Task<bool> DeleteCustomerByIdAsyncService(Guid customerId);
    Task<CustomerDto?> UpdateCustomerByIdAsyncService(Guid customerId, UpdateCustomerDto updateCustomer);
}
public class CustomerService : ICustomer
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public CustomerService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<CustomerDto> CreateCustomerAsyncService(CreateCustomerDto newCustomer)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newCustomer.Password);
            newCustomer.Password = hashedPassword;
            var customer = _mapper.Map<Customer>(newCustomer);
            await _appDbContext.Customers.AddAsync(customer);
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

    public async Task<List<CustomerDto>> GetCustomersAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder)
    {
        try
        {
            var customers = await _appDbContext.Customers.ToListAsync();

            // using query to search for all the customers whos matching the name otherwise return null
            var filterCustomers = customers.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterCustomers = filterCustomers.Where(c => c.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                if (filterCustomers.Count() == 0)
                {
                    var exisitingCustomer = _mapper.Map<List<CustomerDto>>(filterCustomers);
                    return exisitingCustomer;
                }
            }
            // sort the list of customers dependes on first or last name as desc or asc othwewise shows the default
            filterCustomers = sortBy?.ToLower() switch
            {
                "firstname" => sortOrder == "desc" ? filterCustomers.OrderByDescending(c => c.FirstName) : filterCustomers.OrderBy(c => c.FirstName),
                "lastname" => sortOrder == "desc" ? filterCustomers.OrderByDescending(c => c.LastName) : filterCustomers.OrderBy(c => c.LastName),
                _ => filterCustomers.OrderBy(c => c.FirstName) // default 
            };
            // return the result in pagination 
            var paginationResult = filterCustomers.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var customerData = _mapper.Map<List<CustomerDto>>(paginationResult);
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
    public async Task<CustomerDto?> GetCustomerByIdAsyncService(Guid customerId)
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

    public async Task<CustomerDto?> UpdateCustomerByIdAsyncService(Guid customerId, UpdateCustomerDto updateCustomer)
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
                customer.Password = BCrypt.Net.BCrypt.HashPassword(updateCustomer.Password);
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