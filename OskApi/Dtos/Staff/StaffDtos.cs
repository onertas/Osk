using System;

namespace OskApi.Dtos.Staff
{
    public class CreateStaffDto
    {
        public string Code { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public Guid HealthFacilityId { get; set; }
        public int Count { get; set; }
    }

    public class UpdateStaffDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public Guid HealthFacilityId { get; set; }
        public int Count { get; set; }
    }

    public class ListStaffDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public Guid HealthFacilityId { get; set; }
        public string HealthFacilityName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class CreateTemporarayStaffDto
    {
        public string Code { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public Guid HealthFacilityId { get; set; }
        public Guid PmTypeId { get; set; }
        public int Count { get; set; }
    }

    public class UpdateTemporarayStaffDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public Guid HealthFacilityId { get; set; }
        public Guid PmTypeId { get; set; }
        public int Count { get; set; }
    }

    public class ListTemporarayStaffDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public Guid HealthFacilityId { get; set; }
        public string HealthFacilityName { get; set; } = string.Empty;
        public Guid PmTypeId { get; set; }
        public string PmTypeName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
