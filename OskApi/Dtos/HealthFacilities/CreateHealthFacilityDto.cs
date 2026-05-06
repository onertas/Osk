using OskApi.Entities.HealthFacilities;

namespace OskApi.Dtos.HealthFacilities
{
    public class CreateHealthFacilityDto
    {

        public Guid HealthFacilityTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? CorporationName { get; set; }
        public int ObservationBedCount { get; set; }
        public int TotalBedCount { get; set; }
        public DateTime? OpeningDate { get; set; }
        public Guid UpperHealthFacilityId { get; set; }
    }
}
