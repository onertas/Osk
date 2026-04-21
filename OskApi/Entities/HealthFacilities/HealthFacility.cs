namespace OskApi.Entities.HealthFacilities
{
    public class HealthFacility:Entity
    {

        public Guid HealthFacilityTypeId { get; set; }
        public HealthFacilityType? HealthFacilityType { get; set; }
        public string Name { get; set; }=null!;
        public string? Address { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; } 
        public string? TaxNumber { get; set; }
        public string? CorporationName { get; set; }

    }
}
