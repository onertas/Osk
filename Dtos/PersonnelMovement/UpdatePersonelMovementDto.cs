using System;

namespace OskApi.Dtos.PersonnelMovement
{
    public class UpdatePersonelMovementDto
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public DateTime? ContractStart { get; set; }
        public DateTime? ContractFinish { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid PmTypeId { get; set; }
        public Guid BranchId { get; set; }
        public Guid HealthFacilityId { get; set; }
        public Guid AfiliatedUnitId { get; set; }
        public Guid PersonnelId { get; set; }
        public bool IsSgk { get; set; }
    }
}
