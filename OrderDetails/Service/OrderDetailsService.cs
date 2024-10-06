// using AutoMapper;
// using Microsoft.EntityFrameworkCore;


// public interface IOrderDetailsService
// {
//     Task<OrderDetailsDto> CreateOrderDetailsAsyncService(CreateOrderDetailsDto newoOrdersDetilrs);
//     Task<List<OrderDetailsDto>> GetOrderDetailsAsyncService(int pageNumber, int pageSize);
//     Task<OrderDetailsDto?> GetOrderDetailsByIdAsyncService(Guid orderId);
//     Task<bool> DeleteOrderDetailsByIdAsyncService(Guid OrdersDetailesId);
//     Task<OrderDetailsDto?> UpdateOrderByIdAsyncService(Guid orderId, UpdateOrderDetailsDot updateOrder);
// }
// public class OrderDetailsService : IOrderDetailsService
// {
//     private readonly AppDBContext _appDbContext;
//     private readonly IMapper _mapper;

//     public OrderDetailsService(AppDBContext appDbContext, IMapper mapper)
//     {

//         _appDbContext = appDbContext;
//         _mapper = mapper;
//     }

//     // Create a new order
//     public async Task<OrderDetailsDto> CreateOrderDetailsAsyncService(CreateOrderDetailsDto newoOrdersDetails)
//     {
//         try
//         {
//             var orderDetails = _mapper.Map<OrderDetails>(newoOrdersDetails);
//             var orderDetailsProduct = _appDbContext.Products.FirstOrDefault(p => p.ProductId == newoOrdersDetails.ProductId);
//             if (orderDetailsProduct == null)
//             {
//                 throw new Exception($"There is no product with this Id {newoOrdersDetails.ProductId}");
//             }
//             orderDetails.Product = orderDetailsProduct;
//             orderDetails.Quantity = orderDetailsProduct.Quantity;
//             orderDetails.TotalPrice = orderDetailsProduct.Price * orderDetailsProduct.Quantity;

//             await _appDbContext.OrderDetailses.AddAsync(orderDetails);
//             await _appDbContext.SaveChangesAsync();

//             var ordersDetialsData = _mapper.Map<OrderDetailsDto>(orderDetails);
//             return ordersDetialsData;

//         }
//         catch (DbUpdateException dbEx)
//         {
//             // Handle database update exceptions
//             Console.WriteLine($"Database Update Error: {dbEx.Message}");
//             throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
//         }
//         catch (Exception ex)
//         {
//             // Handle any other unexpected exceptions
//             Console.WriteLine($"An unexpected error occurred: {ex.Message}");
//             throw new ApplicationException("An unexpected error occurred. Please try again later.");
//         }
//     }

//     // Get all orders
//     public async Task<List<OrderDetailsDto>> GetOrderDetailsAsyncService(int pageNumber, int pageSize)

//     {
//         try
//         {
//             var ordersList = await _appDbContext.OrderDetailses.Include(p => p.Product).ToListAsync();
//             // return the pagination result
//             var paginationResult = ordersList.Skip((pageNumber - 1) * pageSize).Take(pageSize);
//             var OrderDetailsDate = _mapper.Map<List<OrderDetailsDto>>(paginationResult);
//             return OrderDetailsDate;
//         }
//         catch (DbUpdateException dbEx)
//         {
//             // Handle database update exceptions
//             Console.WriteLine($"Database Update Error: {dbEx.Message}");
//             throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
//         }
//         catch (Exception ex)
//         {
//             // Handle any other unexpected exceptions
//             Console.WriteLine($"An unexpected error occurred: {ex.Message}");
//             throw new ApplicationException("An unexpected error occurred. Please try again later.");
//         }
//     }

//     // Get order by Id
//     public async Task<OrderDetailsDto?> GetOrderDetailsByIdAsyncService(Guid orderId)
//     {
//         try
//         {
//             var order = await _appDbContext.OrderDetailses.FindAsync(orderId);
//             if (order == null)
//             {
//                 return null;
//             }
//             var orderData = _mapper.Map<OrderDetailsDto>(order);
//             return orderData;
//         }
//         catch (DbUpdateException dbEx)
//         {
//             // Handle database update exceptions
//             Console.WriteLine($"Database Update Error: {dbEx.Message}");
//             throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
//         }
//         catch (Exception ex)
//         {
//             // Handle any other unexpected exceptions
//             Console.WriteLine($"An unexpected error occurred: {ex.Message}");
//             throw new ApplicationException("An unexpected error occurred. Please try again later.");
//         }
//     }

//     // Delete order by Id
//     public async Task<bool> DeleteOrderDetailsByIdAsyncService(Guid OrdersDetailesId)
//     {
//         try
//         {
//             var ordersDetailesId = await _appDbContext.OrderDetailses.FindAsync(OrdersDetailesId);
//             if (ordersDetailesId == null)
//             {
//                 return false;
//             }
//             _appDbContext.OrderDetailses.Remove(ordersDetailesId);
//             await _appDbContext.SaveChangesAsync();
//             return true;
//         }

//         catch (DbUpdateException dbEx)
//         {
//             // Handle database update exceptions
//             Console.WriteLine($"Database Update Error: {dbEx.Message}");
//             throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
//         }
//         catch (Exception ex)
//         {
//             // Handle any other unexpected exceptions
//             Console.WriteLine($"An unexpected error occurred: {ex.Message}");
//             throw new ApplicationException("An unexpected error occurred. Please try again later.");
//         }
//     }

//     // Update order by Id
//     public async Task<OrderDetailsDto?> UpdateOrderByIdAsyncService(Guid orderId, UpdateOrderDetailsDot updateOrder)
//     {
//         try
//         {
//             var order = await _appDbContext.OrderDetailses.FindAsync(orderId);
//             if (order == null)
//             {
//                 return null;
//             }

//             _appDbContext.OrderDetailses.Update(order);
//             await _appDbContext.SaveChangesAsync();
//             var orderData = _mapper.Map<OrderDetailsDto>(order);
//             return orderData;
//         }
//         catch (DbUpdateException dbEx)
//         {
//             // Handle database update exceptions
//             Console.WriteLine($"Database Update Error: {dbEx.Message}");
//             throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
//         }
//         catch (Exception ex)
//         {
//             // Handle any other unexpected exceptions
//             Console.WriteLine($"An unexpected error occurred: {ex.Message}");
//             throw new ApplicationException("An unexpected error occurred. Please try again later.");
//         }

//     }
// }