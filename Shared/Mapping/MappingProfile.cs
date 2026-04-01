using AutoMapper;
using OskApi.Dtos.HealthFacilities;
using OskApi.Dtos.Personnel;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;

namespace OskApi.Shared.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Personnel, ListPersonnelDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src =>
                        src.PersonnelBranches!.Select(pb => pb.Branch!.Title!.Name).FirstOrDefault()))
                .ForMember(dest => dest.Branches, opt => opt.MapFrom(src =>
                        src.PersonnelBranches!.Select(pb => pb.Branch!.Name).ToList()));

            CreateMap<HealthFacility, CreateHealthFacilityDto>().ReverseMap();
            CreateMap<PersonnelMovement, OskApi.Dtos.PersonnelMovement.CreatePersonelMovementDto>().ReverseMap();
            CreateMap<PersonnelMovement, OskApi.Dtos.PersonnelMovement.ListPersonelMovementDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.CreatePmTypeDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.ListPmTypeDto>().ReverseMap();
        }
    }
}
