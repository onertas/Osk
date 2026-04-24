using AutoMapper;
using OskApi.Dtos.HealthFacilities;
using OskApi.Dtos.Personnel;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;
using OskApi.Entities.Staff;
using OskApi.Dtos.Staff;
using OskApi.Dtos.PersonnelMovement;

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
            CreateMap<PersonnelMovement, CreatePersonelMovementDto>().ReverseMap();
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
        }
    }
}
