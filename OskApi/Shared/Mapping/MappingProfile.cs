using AutoMapper;
using OskApi.Dtos.HealthFacilities;
using OskApi.Dtos.Personnel;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;
using OskApi.Entities.Staff;
using OskApi.Dtos.Staff;
using OskApi.Dtos.PersonnelMovement;
using OskApi.Entities.Beds;
using OskApi.Dtos.Beds;

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

            CreateMap<CreateHealthFacilityDto, HealthFacility>()
                .ForMember(dest => dest.HfStatus, opt => opt.MapFrom(src => HfStatus.FromValue(src.HfStatus)));
            CreateMap<HealthFacility, CreateHealthFacilityDto>()
                .ForMember(dest => dest.HfStatus, opt => opt.MapFrom(src => src.HfStatus.Value));

            CreateMap<UpdateHealthFacilityDto, HealthFacility>()
                .ForMember(dest => dest.HfStatus, opt => opt.MapFrom(src => HfStatus.FromValue(src.HfStatus)));
            CreateMap<HealthFacility, UpdateHealthFacilityDto>()
                .ForMember(dest => dest.HfStatus, opt => opt.MapFrom(src => src.HfStatus.Value));
            CreateMap<HealthFacility, HfManagementListDto>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.HealthFacilityType != null ? src.HealthFacilityType.Name : ""))
                .ForMember(dest => dest.ShowBed, opt => opt.MapFrom(src => src.HealthFacilityType != null && src.HealthFacilityType.ShowBed))
                .ForMember(dest => dest.ShowDevice, opt => opt.MapFrom(src => src.HealthFacilityType != null && src.HealthFacilityType.ShowDevice))
                .ForMember(dest => dest.ShowStaff, opt => opt.MapFrom(src => src.HealthFacilityType != null && src.HealthFacilityType.ShowStaff))
                .ForMember(dest => dest.ShowTempStaff, opt => opt.MapFrom(src => src.HealthFacilityType != null && src.HealthFacilityType.ShowTempStaff));
            CreateMap<PersonnelMovement, CreatePersonelMovementDto>().ReverseMap()
                .ForMember(dest => dest.SubFacilities, opt => opt.Ignore());
            CreateMap<PersonnelMovement, ListPersonelMovementDto>().ReverseMap();
            CreateMap<PersonnelMovement, UpdatePersonelMovementDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.CreatePmTypeDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.ListPmTypeDto>().ReverseMap();
            CreateMap<PmType, OskApi.Dtos.PmType.UpdatePmTypeDto>().ReverseMap();

            // Staff
            CreateMap<Staff, CreateStaffDto>().ReverseMap();
            CreateMap<Staff, UpdateStaffDto>().ReverseMap();
            CreateMap<Staff, ListStaffDto>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : ""))
                .ForMember(dest => dest.HealthFacilityName, opt => opt.MapFrom(src => src.HealthFacility != null ? src.HealthFacility.Name : ""));

            // TemporarayStaff
            CreateMap<TemporarayStaff, CreateTemporarayStaffDto>().ReverseMap();
            CreateMap<TemporarayStaff, UpdateTemporarayStaffDto>().ReverseMap();
            CreateMap<TemporarayStaff, ListTemporarayStaffDto>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : ""))
                .ForMember(dest => dest.HealthFacilityName, opt => opt.MapFrom(src => src.HealthFacility != null ? src.HealthFacility.Name : ""))
                .ForMember(dest => dest.PmTypeName, opt => opt.MapFrom(src => src.PmType != null ? src.PmType.Name : ""));

            // IcBed
            CreateMap<IcBed, ListIcBedDto>()
                .ForMember(dest => dest.IcBedName, opt => opt.MapFrom(src => src.IcBedName != null ? src.IcBedName.Name : ""))
                .ForMember(dest => dest.IcBedType, opt => opt.MapFrom(src => src.IcBedName != null ? src.IcBedName.IcBedType.Value : 0))
                .ForMember(dest => dest.IcBedTypeName, opt => opt.MapFrom(src => src.IcBedName != null ? src.IcBedName.IcBedType.Description : ""))
                .ForMember(dest => dest.IcBedRegLevelName, opt => opt.MapFrom(src => src.IcBedRegLevel.Description))
                .ForMember(dest => dest.IcBedRegTypeName, opt => opt.MapFrom(src => src.IcBedRegType.Description))
                .ForMember(dest => dest.HealthFacilityName, opt => opt.MapFrom(src => src.HealthFacility != null ? src.HealthFacility.Name : ""));

            CreateMap<CreateIcBedDto, IcBed>()
                .ForMember(dest => dest.IcBedRegLevel, opt => opt.MapFrom(src => IcBedRegLevel.FromValue(src.IcBedRegLevel)))
                .ForMember(dest => dest.IcBedRegType, opt => opt.MapFrom(src => IcBedRegType.FromValue(src.IcBedRegType)));

            CreateMap<UpdateIcBedDto, IcBed>()
                .ForMember(dest => dest.IcBedRegLevel, opt => opt.MapFrom(src => IcBedRegLevel.FromValue(src.IcBedRegLevel)))
                .ForMember(dest => dest.IcBedRegType, opt => opt.MapFrom(src => IcBedRegType.FromValue(src.IcBedRegType)));
        }
    }
}
