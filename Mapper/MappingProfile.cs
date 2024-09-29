using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<UpdateCustomerDto, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();

        CreateMap<Address, AddressDto>();
        CreateMap<UpdateAddressDto, AddressDto>();
        CreateMap<CreateAddressDto, Address>();
    }
}