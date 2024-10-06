// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("api/v1/orderDetails")]
// public class OrderDetailsController : ControllerBase
// {

//     private readonly IOrderDetailsService _service;

//     public OrderDetailsController(IOrderDetailsService service)
//     {
//         _service = service;
//     }

//     // GET: api/orderdetails/{id}
//     [Authorize]
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetOrderDetailsById(Guid id)
//     {
//         try
//         {
//             var orderDetails = await _service.GetOrderDetailsByIdAsyncService(id);

//             if (orderDetails == null)
//             {
//                 return ApiResponses.NotFound("Order details not found");
//             }

//             return ApiResponses.Success(orderDetails, "Order details retrieved successfully");
//         }
//         catch (ApplicationException ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }
//         catch (System.Exception ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }
//     }

//     // GET: api/orderdetails
//     [Authorize (Roles = "Admin")]
//     [HttpGet]
//     public async Task<IActionResult> GetAllOrderDetails(int pageNumber = 1, int pageSize = 3)
//     {
//         try
//         {

//             var orderDetailsList = await _service.GetOrderDetailsAsyncService(pageNumber, pageSize);

//             if (orderDetailsList.Count() == 0)
//             {
//                 return ApiResponses.NotFound("No order details found");
//             }

//             return ApiResponses.Success(orderDetailsList, "Order details retrieved successfully");
//         }
//         catch (ApplicationException ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }
//         catch (System.Exception ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }

//     }

//     // POST: api/orderdetails
//     [Authorize]
//     [HttpPost]
//     public async Task<IActionResult> CreateOrderDetails([FromBody] CreateOrderDetailsDto neworderDetails)
//     {
//         try
//         {
//             {
//                 var orderDetails = await _service.CreateOrderDetailsAsyncService(neworderDetails);
//                 return ApiResponses.Created(orderDetails, "Order created successfully");
//             }
//         }
//         catch (ApplicationException ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }
//         catch (System.Exception ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }

//     }
//     // PUT: api/order/{id}
//     [Authorize]
//     [HttpPut("{id}")]
//     public async Task<IActionResult> UpdateorderDetails(Guid id, [FromBody] UpdateOrderDetailsDot UpdateorderDetails)
//     {
//         try
//         {
//             var UpdateOrderDetails = await _service.UpdateOrderByIdAsyncService(id, UpdateorderDetails);
//             if (UpdateorderDetails == null)
//             {
//                 return ApiResponses.NotFound("Order not found");
//             }


//             return ApiResponses.Success(UpdateorderDetails, "Order updated successfully");
//         }
//         catch (ApplicationException ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }
//         catch (System.Exception ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }
//     }

//     // DELETE: api/order/{id}
//     [Authorize]
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteOrderDetails(Guid id)
//     {
//         try
//         {
//             var OrderDetails = await _service.DeleteOrderDetailsByIdAsyncService(id);
//             if (!OrderDetails)
//             {
//                 return ApiResponses.NotFound("Order not found");
//             }


//             return ApiResponses.Success("Order deleted successfully");
//         }
//         catch (ApplicationException ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }
//         catch (System.Exception ex)
//         {
//             return ApiResponses.ServerError("Server error: " + ex.Message);
//         }
//     }

// }