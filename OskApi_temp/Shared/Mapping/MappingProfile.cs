using AutoMapper;
using OskApi.Dtos.HealthFacilities;
using OskApi.Dtos.Personnel;
using OskApi.Dtos.Beds;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Beds;
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
            CreateMap<PersonnelMovement, OskApi.Dtos.PersonnelMovement.UpdatePersonelMovementDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.CreatePmTypeDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.ListPmTypeDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.UpdatePmTypeDto>().ReverseMap();

            CreateMap<IcBed, CreateIcBedDto>().ReverseMap()
                .ForMember(dest => dest.IcBedRegLevel, opt => opt.Ignore())
                .ForMember(dest => dest.IcBedRegType, opt => opt.Ignore());

            CreateMap<IcBed, UpdateIcBedDto>().ReverseMap()
                .ForMember(dest => dest.IcBedRegLevel, opt => opt.Ignore())
                .ForMember(dest => dest.IcBedRegType, opt => opt.Ignore());

            CreateMap<IcBed, ListIcBedDto>()
                .ForMember(dest => dest.IcBedRegLevel, opt => opt.MapFrom(src => src.IcBedRegLevel.Value))
                .ForMember(dest => dest.IcBedRegLevelName, opt => opt.MapFrom(src => src.IcBedRegLevel.Description))
                .ForMember(dest => dest.IcBedRegType, opt => opt.MapFrom(src => src.IcBedRegType.Value))
                .ForMember(dest => dest.IcBedRegTypeName, opt => opt.MapFrom(src => src.IcBedRegType.Description))
                .ForMember(dest => dest.IcBedName, opt => opt.MapFrom(src => src.IcBedName != null ? src.IcBedName.Name : ""))
                .ForMember(dest => dest.IcBedType, opt => opt.MapFrom(src => src.IcBedName != null ? src.IcBedName.IcBedType.Value : 0))
                .ForMember(dest => dest.IcBedTypeName, opt => opt.MapFrom(src => src.IcBedName != null ? src.IcBedName.IcBedType.Description : ""));
        }
    }
}
