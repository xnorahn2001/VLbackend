using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // mapping for customers
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, UserDto>();
        CreateMap<RegisterUserDto, User>();

        // mapping for address
        CreateMap<Address, AddressDto>();
        CreateMap<UpdateAddressDto, AddressDto>();
        CreateMap<CreateAddressDto, Address>();

        // mapping for payment
        CreateMap<Payment, PaymentDto>();
        CreateMap<UpdatePaymentDto, PaymentDto>();
        CreateMap<CreatePaymentDto, Payment>();

        // mapping for product
        CreateMap<Product, ProductDto>();
        CreateMap<UpdateProductDto, ProductDto>();
        CreateMap<CreateProductDto, Product>();

        // mapping for Order
        // CreateMap<Order, OrderDto>();
        CreateMap<CreateOrderDto, Order>();

        // mapping for Order Details
        CreateMap<OrderDetails, OrderDetailsDto>();
        CreateMap<UpdateOrderDetailsDot, OrderDetailsDto>();
        CreateMap<CreateOrderDetailsDto, OrderDetails>();


        CreateMap<Order, OrderDto>()
        .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment))
        .ForMember(dest => dest.Shipment, opt => opt.MapFrom(src => src.Shipment));


        // mapping for shipments
        CreateMap<Shipment, ShipmentDto>();
        CreateMap<UpdateShipmentDto, ShipmentDto>();
        CreateMap<CreateShipmentDto, ShipmentDto>();

    }
}