namespace OskApi.Entities.Beds
{
    public class IcBed : Entity
    {
        public Guid HealthFacilityId { get; set; }
        public IcBedRegLevel IcBedRegLevel { get; set; } = IcBedRegLevel.Seviye1;
        public IcBedRegType IcBedRegType { get; set; } = IcBedRegType.Kesin;
        public int Quantity { get; set; }
        public DateTime IcBedRegDate { get; set; }
        public string IcBedRegNumber { get; set; }= null!;
        public IcBedName? IcBedName { get; set; }
        public Guid IcBedNameId { get; set; }
        public bool IsActive { get; set; } = true;

    }

}
