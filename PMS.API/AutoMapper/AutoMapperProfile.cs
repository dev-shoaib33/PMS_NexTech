using AutoMapper;
using PMS.API.Models;
using PMS.DB.Model.EF.Models;
using PMS.Services.ServiceModels;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ProductVM, ProductSM>();  // Example mapping configuration
        CreateMap<ProductSM, ProductVM>();
        CreateMap<ProductSM, PmsProduct>();
        CreateMap<PmsProduct, ProductSM>();
        CreateMap<PmsProduct, ProductVM>();
    }
}

