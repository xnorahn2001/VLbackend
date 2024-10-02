using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // mapping for customers
        CreateMap<Customer, CustomerDto>();
        CreateMap<UpdateCustomerDto, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();

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