using OskApi.Entities.Beds;

namespace OskApi.Dtos.Beds
{
    public class CreateIcBedDto
    {
        public Guid HealthFacilityId { get; set; }
        public int IcBedRegLevel { get; set; }
        public int IcBedRegType { get; set; }
        public int Quantity { get; set; }
        public DateTime IcBedRegDate { get; set; }
        public string IcBedRegNumber { get; set; } = null!;
        public Guid IcBedNameId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
