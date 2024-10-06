using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IShipmentService
{
    Task<ShipmentDto> CreateShipmentAsyncService(CreateShipmentDto newShipment);
    Task<List<ShipmentDto>> GetShipmentsAsyncService(int pageNumber, int pageSize, int? searchQuery, string? sortBy, string? sortOrder);
    Task<ShipmentDto?> GetShipmentByIdAsyncService(Guid shipmentId);
    Task<bool> DeleteShipmentByIdAsyncService(Guid shipmentId);
    Task<ShipmentDto?> UpdateShipmentByIdAsyncService(Guid shipmentId, UpdateShipmentDto updateShipment);
}

public class ShipmentService : IShipmentService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public ShipmentService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<ShipmentDto> CreateShipmentAsyncService(CreateShipmentDto newShipment)
    {
        try
        {
            var shipment = _mapper.Map<Shipment>(newShipment);

            var shipmentOrder = shipment.Order;
            shipmentOrder = await _appDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == newShipment.OrderId);
            if (shipmentOrder == null)
            {
                throw new Exception($"There is no order with this Id {newShipment.OrderId}");
            }

            shipment.ShipmentDate = DateTime.Now;
            shipment.DeliveryDate = DateTime.Now.AddMonths(3);
            Random random = new Random();
            shipment.TrackingNumber = random.Next(999999, 99999999);

            await _appDbContext.Shipments.AddAsync(shipment);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<ShipmentDto>(shipment);
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.", dbEx);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task<List<ShipmentDto>> GetShipmentsAsyncService(int pageNumber, int pageSize, int? searchQuery, string? sortBy, string? sortOrder)
    {
        try
        {
            var shipments = await _appDbContext.Shipments.Include(o => o.Order).ToListAsync();
            // Apply search filter if searchQuery is provided
            var filteredShipments = shipments.AsQueryable();
            if (!string.IsNullOrEmpty(Convert.ToString(searchQuery)))
            {
                filteredShipments = filteredShipments.Where(s => s.TrackingNumber.Equals(searchQuery));
                if (filteredShipments.Count() == 0)
                {
                    return _mapper.Map<List<ShipmentDto>>(filteredShipments);
                }
            }

            // Apply sorting
            filteredShipments = sortBy?.ToLower() switch
            {
                "date" => sortOrder == "desc" ? filteredShipments.OrderByDescending(s => s.ShipmentDate) : filteredShipments.OrderBy(s => s.ShipmentDate),
                "status" => sortOrder == "desc" ? filteredShipments.OrderByDescending(s => s.Status) : filteredShipments.OrderBy(s => s.Status),
                _ => filteredShipments.OrderBy(s => s.ShipmentDate) // default sort
            };

            // Apply pagination
            var paginatedShipments = filteredShipments.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return _mapper.Map<List<ShipmentDto>>(paginatedShipments);
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.", dbEx);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task<ShipmentDto?> GetShipmentByIdAsyncService(Guid shipmentId)
    {
        try
        {
            var shipment = await _appDbContext.Shipments.FindAsync(shipmentId);
            if (shipment == null)
            {
                return null;
            }
            return _mapper.Map<ShipmentDto>(shipment);
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.", dbEx);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task<bool> DeleteShipmentByIdAsyncService(Guid shipmentId)
    {
        try
        {
            var shipment = await _appDbContext.Shipments.FindAsync(shipmentId);
            if (shipment == null)
            {
                return false;
            }
            _appDbContext.Shipments.Remove(shipment);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.", dbEx);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task<ShipmentDto?> UpdateShipmentByIdAsyncService(Guid shipmentId, UpdateShipmentDto updateShipment)
    {
        try
        {
            var shipment = await _appDbContext.Shipments.FindAsync(shipmentId);
            if (shipment == null)
            {
                return null;
            }

            shipment.ShipmentDate = updateShipment.ShipmentDate ?? shipment.ShipmentDate;
            shipment.DeliveryDate = updateShipment.DeliveryDate ?? shipment.DeliveryDate;
            shipment.Status = updateShipment.Status ?? shipment.Status;

            _appDbContext.Shipments.Update(shipment);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<ShipmentDto>(shipment);
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving to the database. Please check the data and try again.", dbEx);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
        }
    }
}