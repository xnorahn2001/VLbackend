using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // mapping for customers
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, UserDto>();
        CreateMap<CreateUserDto, User>();

        // mapping for address
        CreateMap<Address, AddressDto>();
        CreateMap<UpdateAddressDto, AddressDto>();
        CreateMap<CreateAddressDto, Address>();

        // mapping for payment
        CreateMap<Payment, PaymentDto>();
        CreateMap<UpdatePaymentDto, PaymentDto>();
        CreateMap<CreatePaymentDto, Payment>();

    }
}