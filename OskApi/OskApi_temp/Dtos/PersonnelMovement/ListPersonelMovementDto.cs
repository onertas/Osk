using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;

namespace OskApi.Dtos.PersonnelMovement
{
    public class ListPersonelMovementDto
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public DateTime? ContractStart { get; set; }
        public DateTime? ContractFinish { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid PmTypeId { get; set; }
        public OskApi.Entities.Personnel.PmType? PmType { get; set; }

        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }

        public Guid HealthFacilityId { get; set; }
        public HealthFacility? HealthFacility { get; set; }

        public Guid AfiliatedUnitId { get; set; }

        public Guid PersonnelId { get; set; }
        public OskApi.Entities.Personnel.Personnel? Personnel { get; set; }

        public bool IsSgk { get; set; }
    }
}
