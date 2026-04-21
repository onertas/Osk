namespace OskApi.Dtos.HealthFacilities
{
    public class UpdateHealthFacilityDto
    {
        public Guid Id { get; set; }
        public Guid HealthFacilityTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? CorporationName { get; set; }
    }
}
