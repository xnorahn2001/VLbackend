using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IAddress
{
    Task<AddressDto> CreateAddressAsyncService(CreateAddressDto newAddress);
    Task<List<AddressDto>> GetAddressesAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder);
    Task<AddressDto?> GetAddressByIdAsyncService(Guid addressId);
    Task<bool> DeleteAddressByIdAsyncService(Guid addressId);
    Task<AddressDto?> UpdateAddressByIdAsyncService(Guid addressId, UpdateAddressDto updateAddress);
}
public class AddressService : IAddress
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public AddressService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<AddressDto> CreateAddressAsyncService(CreateAddressDto newAddress)
    {
        try
        {
            var address = _mapper.Map<Address>(newAddress);
            var user = await _appDbContext.Users.FirstOrDefaultAsync(c => c.UserId == newAddress.UserId);
            if (user == null)
            {
                throw new Exception("this user does not exisit");
            }
            address.User = user;
            var addressAdded = await _appDbContext.Addresses.AddAsync(address);
            await _appDbContext.SaveChangesAsync();
            var addressData = _mapper.Map<AddressDto>(address);
            return addressData;
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

    public async Task<List<AddressDto>> GetAddressesAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder)
    {
        try
        {
            var addresses = await _appDbContext.Addresses.Include(a => a.User).ToListAsync();
            // using query to search for all the users whos matching the name otherwise return null
            var filterAddresses = addresses.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterAddresses = filterAddresses.Where(a => a.AddressName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                if (filterAddresses.Count() == 0)
                {
                    var exisitingAddress = _mapper.Map<List<AddressDto>>(filterAddresses);
                    return exisitingAddress;
                }
            }
            // sort the list of address dependes on their city or state as desc or asc othwewise shows the default
            filterAddresses = sortBy?.ToLower() switch
            {
                "city" => sortOrder == "desc" ? filterAddresses.OrderByDescending(a => a.City) : filterAddresses.OrderBy(a => a.City),
                "state" => sortOrder == "desc" ? filterAddresses.OrderByDescending(a => a.State) : filterAddresses.OrderBy(a => a.State),
                _ => filterAddresses.OrderBy(a => a.City) // default 
            };
            // return the pagination result
            var paginationResult = filterAddresses.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var addressData = _mapper.Map<List<AddressDto>>(paginationResult);
            return addressData;
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
    public async Task<AddressDto?> GetAddressByIdAsyncService(Guid addressId)
    {
        try
        {
            var address = await _appDbContext.Addresses.FindAsync(addressId);
            if (address == null)
            {
                return null;
            }
            var addressData = _mapper.Map<AddressDto>(address);
            return addressData;
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

    public async Task<bool> DeleteAddressByIdAsyncService(Guid addressId)
    {
        try
        {
            var address = await _appDbContext.Addresses.FindAsync(addressId);
            if (address == null)
            {
                return false;
            }
            _appDbContext.Addresses.Remove(address);
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

    public async Task<AddressDto?> UpdateAddressByIdAsyncService(Guid addressId, UpdateAddressDto updateAddress)
    {
        try
        {
            var address = await _appDbContext.Addresses.FindAsync(addressId);
            if (address == null)
            {
                return null;
            }

            address.AddressName = updateAddress.AddressName ?? address.AddressName;
            address.StreetName = updateAddress.StreetName ?? address.StreetName;
            address.StreetNumber = updateAddress.StreetNumber ?? address.StreetNumber;
            address.City = updateAddress.City ?? address.City;
            address.State = updateAddress.State ?? address.State;

            _appDbContext.Addresses.Update(address);
            await _appDbContext.SaveChangesAsync();

            var addressData = _mapper.Map<AddressDto>(address);
            return addressData;
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