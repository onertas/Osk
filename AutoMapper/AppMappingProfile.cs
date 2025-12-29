using AutoMapper;
using OskApi.Dtos.HealthFacilities;
using OskApi.Entities.HealthFacilities;


namespace OskApi.AutoMapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<HealthFacility, CreateHealthFacilityDto>().ReverseMap();
        }
    }
}
