using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class AddressService
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
            await _appDbContext.Addresses.AddAsync(address);
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

    public async Task<List<AddressDto>> GetAddressAsyncService()
    {
        try
        {
            var addresses = await _appDbContext.Addresses.ToListAsync();
            var addressData = _mapper.Map<List<AddressDto>>(addresses);
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

            _appDbContext.Update(address);
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