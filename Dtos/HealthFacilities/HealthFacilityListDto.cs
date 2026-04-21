using OskApi.Entities.HealthFacilities;

namespace OskApi.Dtos.HealthFacilities
{
    public class HealthFacilityListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string TypeName { get; set; } = null!;
       
    }
}
