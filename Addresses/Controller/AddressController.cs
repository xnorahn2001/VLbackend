
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/v1/addresses")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    // Post: "/api/v1/adresses" => create new address
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAdressAsync([FromBody] CreateAddressDto newAddress)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("Validation errors:");
            errors.ForEach(error => Console.WriteLine(error));

            // Return a custom response with validation errors
            return BadRequest(new { Message = "Validation failed", Errors = errors });
        }
        try
        {
            var address = await _addressService.CreateAddressAsyncService(newAddress);
            return ApiResponses.Created(address, "Address created successfully");
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, "Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, "Server error: " + ex.Message);
        }
    }

    // Get: "/api/v1/addresses" => get all the addresses 
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAddressAsync(int pageNumber = 1, int pageSize = 3, string? searchQuery = null, string? sortBy = null, string? sortOrder = "asc")
    {
        try
        {
            var addresses = await _addressService.GetAddressesAsyncService(pageNumber, pageSize, searchQuery, sortBy, sortOrder);
            if (addresses.Count() == 0)
            {
                return ApiResponses.NotFound("The list of address is empty or you try to search for not exisiting address name.");
            }
            return ApiResponses.Success(addresses, "Return the list of addresses successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // Get: "api/v1/addresses/{addressId}" => get specific address by Id
    [Authorize]
    [HttpGet("{addressId}")]
    public async Task<IActionResult> GetAddressByIdAsync(Guid addressId)
    {
        try
        {
            var address = await _addressService.GetAddressByIdAsyncService(addressId);
            if (address == null)
            {
                return ApiResponses.NotFound("Address does not exisit");
            }
            return ApiResponses.Success(address, "Address is returned successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }


    }
    // Delet: "api/v1/address/{addressId}" => delete address by Id 
    [Authorize]
    [HttpDelete("{addressId}")]
    public async Task<IActionResult> DeleteAddressByIdAsync(Guid addressId)
    {
        try
        {
            var address = await _addressService.DeleteAddressByIdAsyncService(addressId);
            if (!address)
            {
                return ApiResponses.NotFound("Address does not exisit");
            }
            return ApiResponses.Success("Address deleted successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // Put: "api/v1/address/{addressId}" => update address by Id
    [Authorize]
    [HttpPut("{addressId}")]
    public async Task<IActionResult> UpdataAddressByIdAsync(Guid addressId, [FromBody] UpdateAddressDto updateAddress)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("Validation errors:");
            errors.ForEach(error => Console.WriteLine(error));
            return ApiResponses.BadRequest("Invalid Address Data");
        }
        try
        {
            var address = await _addressService.UpdateAddressByIdAsyncService(addressId, updateAddress);
            if (address == null)
            {
                return ApiResponses.NotFound("Address does not exisit");
            }
            return ApiResponses.Success("Address updated successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

}