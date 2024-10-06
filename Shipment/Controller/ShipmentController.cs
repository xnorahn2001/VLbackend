using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/shipments")]
public class ShipmentController : ControllerBase
{
    private readonly IShipmentService _shipmentService;

    public ShipmentController(IShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    // Post: "/api/v1/shipments" => create new shipment
    [Authorize (Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateShipmentAsync([FromBody] CreateShipmentDto newShipment)
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
            var shipment = await _shipmentService.CreateShipmentAsyncService(newShipment);
            return ApiResponses.Created(shipment, "Shipment created successfully");
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

    // Get: "/api/v1/shipments" => get all shipments with pagination
    [Authorize (Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetShipmentsAsync(int pageNumber = 1, int pageSize = 3, int? searchQuery = null, string? sortBy = null, string? sortOrder = "asc")
    {
        try
        {
            var shipments = await _shipmentService.GetShipmentsAsyncService(pageNumber, pageSize, searchQuery, sortBy, sortOrder);

            if (shipments.Count() == 0)
            {
                return ApiResponses.NotFound("The list of shipments is empty or you try to search for not exisiting shipment.");
            }

            return ApiResponses.Success(shipments, "Shipments returned successfully.");
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

    // Get: "api/v1/shipments/{shipmentId}" => get specific shipment by id
    [Authorize (Roles = "Admin")]
    [HttpGet("{shipmentId}")]
    public async Task<IActionResult> GetShipmentByIdAsync(Guid shipmentId)
    {
        try
        {
            var shipment = await _shipmentService.GetShipmentByIdAsyncService(shipmentId);
            if (shipment == null)
            {
                return ApiResponses.NotFound("Shipment does not exist.");
            }
            return ApiResponses.Success(shipment, "Shipment returned successfully.");
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

    // Delete: "api/v1/shipments/{shipmentId}" => delete shipment by id
    [Authorize (Roles = "Admin")]
    [HttpDelete("{shipmentId}")]
    public async Task<IActionResult> DeleteShipmentAsync(Guid shipmentId)
    {
        try
        {
            var deleted = await _shipmentService.DeleteShipmentByIdAsyncService(shipmentId);
            if (!deleted)
            {
                return ApiResponses.NotFound("Shipment does not exist.");
            }
            return ApiResponses.Success("Shipment deleted successfully.");
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

    // Put: "api/v1/shipments/{shipmentId}" => update shipment by id
    [Authorize (Roles = "Admin")]
    [HttpPut("{shipmentId}")]
    public async Task<IActionResult> UpdateShipmentByIdAsync(Guid shipmentId, [FromBody] UpdateShipmentDto updateShipment)
    {
        if (!ModelState.IsValid)
        {
            return ApiResponses.BadRequest("Invalid shipment data.");
        }
        try
        {
            var shipment = await _shipmentService.UpdateShipmentByIdAsyncService(shipmentId, updateShipment);
            if (shipment == null)
            {
                return ApiResponses.NotFound("Shipment does not exist.");
            }
            return ApiResponses.Success("Shipment updated successfully.");
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