using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    // GET: api/order/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        try
        {
            var order = await _service.GetOrderByIdAsyncService(id);

            if (order == null)
            {
                return ApiResponses.NotFound("Order not found");
            }

            return ApiResponses.Success(order, "Order returned successfully");
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

    // GET: api/order
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllOrders(int pageNumber = 1, int pageSize = 3)
    {
        try
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return ApiResponses.BadRequest("Page number and page size should be greater than 0.");
            }
            var orders = await _service.GetOrdersAsyncService(pageNumber, pageSize);

            if (orders.Count() == 0)
            {
                return ApiResponses.NotFound("No orders found");
            }

            return ApiResponses.Success(orders, "Orders retrieved successfully");


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

    // POST: api/order
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto newOrder)
    {
        try
        {
            
          //  var authenticatedClaims = HttpContext.User;
           // var userId = authenticatedClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
           // var userIdForOrder = new Guid(userId);
           // Console.WriteLine($"{userIdForOrder}");
            
            var order = await _service.CheckoutOrderAsyncService(newOrder);
            return ApiResponses.Created(order, "Order created successfully");
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

    // DELETE: api/order/{id}
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        try
        {
            var order = await _service.DeleteOrderByIdAsyncService(id);
            if (!order)
            {
                return ApiResponses.NotFound("Order not found");
            }

            return ApiResponses.Success("Order deleted successfully");
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