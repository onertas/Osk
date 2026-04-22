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
                        src.PersonnelBranches == null ? "" : 
                        src.PersonnelBranches.Select(pb => pb.Branch != null && pb.Branch.Title != null ? pb.Branch.Title.Name : "").FirstOrDefault() ?? ""))
                .ForMember(dest => dest.Branches, opt => opt.MapFrom(src =>
                        src.PersonnelBranches == null ? new List<string>() :
                        src.PersonnelBranches.Select(pb => pb.Branch != null ? pb.Branch.Name : "").ToList()))
                .ForMember(dest => dest.BranchIds, opt => opt.MapFrom(src => 
                        src.PersonnelBranches == null ? new List<Guid>() :
                        src.PersonnelBranches.Select(pb => pb.BranchId).ToList()));

            CreateMap<HealthFacility, CreateHealthFacilityDto>().ReverseMap();
            CreateMap<PersonnelMovement, OskApi.Dtos.PersonnelMovement.CreatePersonelMovementDto>().ReverseMap();
            CreateMap<PersonnelMovement, OskApi.Dtos.PersonnelMovement.ListPersonelMovementDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.CreatePmTypeDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.ListPmTypeDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.UpdatePmTypeDto>().ReverseMap();
        }
    }
}
