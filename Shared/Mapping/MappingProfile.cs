using AutoMapper;
using OskApi.Dtos.Personnel;
using OskApi.Entities.Personnel;

namespace OskApi.Shared.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Personnel,ListPersonnelDto>();
           
        }
    }
}
