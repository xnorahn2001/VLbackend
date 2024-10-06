using AutoMapper;
using Microsoft.EntityFrameworkCore;


public interface IOrderService
{
    // Task<OrderDto> CreateOrderAsyncService(CreateOrderDto newOrder);
    Task<OrderDto> CheckoutOrderAsyncService(CreateOrderDetailsDto checkoutItem, Guid userId);
    Task<List<OrderDto>> GetOrdersAsyncService(int pageNumber, int pageSize);
    Task<OrderDto?> GetOrderByIdAsyncService(Guid orderId);
    Task<bool> DeleteOrderByIdAsyncService(Guid orderId);

}
public class OrderService : IOrderService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public OrderService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    // // Create a new order
    // public async Task<OrderDto> CreateOrderAsyncService(CreateOrderDto newOrder)
    // {
    //     try
    //     {
    //         var order = _mapper.Map<Order>(newOrder);
    //         var orderUser = order.User;
    //         orderUser = _appDbContext.Users.FirstOrDefault(u => u.UserId == newOrder.UserId);
    //         if (orderUser == null)
    //         {
    //             throw new Exception($"There is no user with this Id {newOrder.UserId}");
    //         }

    //         var orderDetail = order.OrderDetails;
    //         // orderDetail = _appDbContext.OrderDetailses.FirstOrDefault(o => o.OrdersDetailesId == newOrder.OrderDetailsId);
    //         if (orderDetail == null)
    //         {
    //             throw new Exception($"There is no order detail with this Id {newOrder.OrderDetailsId}");
    //         }

    //         await _appDbContext.Orders.AddAsync(order);
    //         await _appDbContext.SaveChangesAsync();
    //         var orderData = _mapper.Map<OrderDto>(order);
    //         return orderData;
    //     }
    //     catch (DbUpdateException dbEx)
    //     {
    //         // Handle database update exceptions
    //         Console.WriteLine($"Database Update Error: {dbEx.Message}");
    //         throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.");
    //     }
    //     catch (Exception ex)
    //     {
    //         // Handle any other unexpected exceptions
    //         Console.WriteLine($"An unexpected error occurred: {ex.Message}");
    //         throw new ApplicationException("An unexpected error occurred. Please try again later.");
    //     }
    // }

    public async Task<OrderDto> CheckoutOrderAsyncService(CreateOrderDetailsDto checkoutItem, Guid userId)
    {
        try
        {
            // find user existed or not in db 
            var userFound = _appDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (userFound == null)
            {
                throw new Exception($"There is no  user with this id {userId}");
            }
            // [12323, 232332,54545,7676]
            var order = new Order();
            order.User = userFound;
            // run for loop to loop through list of order product id 
            foreach (var productId in checkoutItem.ProductList)
            {            // find product that user wanna checkout is existed in db 
                var foundProduct = _appDbContext.Products.FirstOrDefault(p => p.ProductId == productId);
                if (foundProduct == null)
                {
                    throw new Exception($"There is no product with this id {productId}.");
                }
                if (foundProduct.Quantity < order.Quantity)
                {
                    throw new Exception("You try to order more than we have.");
                }
                // if product is existed in db, check quantity in db 
                var orderDetail = new OrderDetails();
                // orderDetail.ProductId =  productFound 
                // 
                orderDetail.OrderId = order.OrderId;
                orderDetail.Quantity = order.Quantity;
                orderDetail.TotalPrice = foundProduct.Price * checkoutItem.Quantity;
            }
            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();
            var orderData = _mapper.Map<OrderDto>(order);
            return orderData;
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

    // Get all orders
    public async Task<List<OrderDto>> GetOrdersAsyncService(int pageNumber, int pageSize)
    {

        try
        {
            var orders = await _appDbContext.Orders.Include(o => o.User).Include(o => o.OrderDetails).ToListAsync();
            // return the pagination result
            var paginationResult = orders.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var ordersData = _mapper.Map<List<OrderDto>>(paginationResult);
            return ordersData;
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

    // Get order by Id
    public async Task<OrderDto?> GetOrderByIdAsyncService(Guid orderId)
    {
        try
        {
            var order = await _appDbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                return null;
            }
            var orderData = _mapper.Map<OrderDto>(order);
            return orderData;
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

    // Delete order by Id
    public async Task<bool> DeleteOrderByIdAsyncService(Guid orderId)
    {

        try
        {
            var order = await _appDbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }
            _appDbContext.Orders.Remove(order);
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

}