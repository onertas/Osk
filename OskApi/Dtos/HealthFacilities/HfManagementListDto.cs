namespace OskApi.Dtos.HealthFacilities
{
    public class HfManagementListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? CorporationName { get; set; }
        public int ObservationBedCount { get; set; }
        public int TotalBedCount { get; set; }
        public Guid HealthFacilityTypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public DateTime? OpeningDate { get; set; }
        public bool ShowBed { get; set; }
        public bool ShowDevice { get; set; }
        public bool ShowStaff { get; set; }
        public bool ShowTempStaff { get; set; }
        public bool ShowPm { get; set; }
        public Guid UpperHealthFacilityId { get; set; }
        public int HfStatus { get; set; }
        public string HfStatusName { get; set; } = string.Empty;
    }
}
