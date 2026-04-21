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
    }
}
