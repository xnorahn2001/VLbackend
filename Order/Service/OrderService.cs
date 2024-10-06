using AutoMapper;
using Microsoft.EntityFrameworkCore;


public interface IOrderService
{
    // Task<OrderDto> CreateOrderAsyncService(CreateOrderDto newOrder);
    Task<OrderDto> CheckoutOrderAsyncService(CreateOrderDto checkoutItem);
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


    public async Task<OrderDto> CheckoutOrderAsyncService(CreateOrderDto checkoutItem)
    {
        try
        {
            // find user existed or not in db 
            var userFound = _appDbContext.Users.FirstOrDefault(u => u.UserId == checkoutItem.UserId);
            if (userFound == null)
            {
                throw new Exception($"There is no  user with this id {checkoutItem.UserId}");
            }
            // [12323, 232332,54545,7676]
            var order = new Order();
            System.Console.WriteLine("create order instance successfully");

            // save this order in database in case error happen 
            order.User = userFound;
            order.UserId = userFound.UserId;
            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();
            // run for loop to loop through list of order product id 
            decimal totalPriceOrder = 0;
            var totalQuantityOrder = 0;

            foreach (var orderdetails in checkoutItem.OrderDetailses)
            {            // find product that user wanna checkout is existed in db 
                System.Console.WriteLine("in for loop - order details");
                var foundProduct = _appDbContext.Products.FirstOrDefault(p => p.ProductId == orderdetails.ProductId);
                if (foundProduct == null)
                {
                    throw new Exception($"There is no product with this id {orderdetails.ProductId}.");
                }
                if (foundProduct.Quantity < orderdetails.Quantity)
                {
                    throw new Exception("You try to order more than we have.");
                }
                // if product is existed in db, check quantity in db 
                var orderDetail = new OrderDetails();
                // orderDetail.ProductId =  productFound 
                // 
                System.Console.WriteLine("Create an order details");
                System.Console.WriteLine($"Order id {order.OrderId}");

                orderDetail.OrderId = order.OrderId;
                orderDetail.Quantity = orderdetails.Quantity;
                orderDetail.ProductId = foundProduct.ProductId;
                orderDetail.TotalPrice = foundProduct.Price * orderdetails.Quantity;
                System.Console.WriteLine("Before save item in db");
                await _appDbContext.OrderDetailses.AddAsync(orderDetail);
                await _appDbContext.SaveChangesAsync();

                totalPriceOrder += orderDetail.TotalPrice;
                totalQuantityOrder += orderdetails.Quantity;
            }
            order.TotalPrice = totalPriceOrder;
            order.Quantity = totalQuantityOrder;

            var payment = new Payment();

            payment.PaymentMethod = checkoutItem.PaymentMethod ?? PaymentMethod.ApplePay;
            payment.CardNumber = checkoutItem.CardNumber;
            payment.User = order.User;
            payment.UserId = order.UserId;
            payment.Order = order;
            payment.OrderId = order.OrderId;

            await _appDbContext.Payments.AddAsync(payment);
            await _appDbContext.SaveChangesAsync();

            var shipment = new Shipment();
            shipment.ShipmentDate = DateTime.UtcNow;
            shipment.DeliveryDate = DateTime.UtcNow.AddMonths(3);
            shipment.OrderId = order.OrderId;
            shipment.Status = Status.Shipped;
            Random random = new Random();
            shipment.TrackingNumber = random.Next(999999, 99999999);

            await _appDbContext.Shipments.AddAsync(shipment);
            _appDbContext.SaveChanges();

            order.ShipmentId = shipment.ShipmentId;
            _appDbContext.Orders.Update(order);
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
            var orders = await _appDbContext.Orders.Include(o => o.User).Include(o => o.OrderDetails).Include(o => o.Payment).Include(o => o.Shipment).ToListAsync();
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