namespace OskApi.Dtos.Beds
{
    public class ListIcBedDto
    {
        public Guid Id { get; set; }
        public Guid HealthFacilityId { get; set; }
        public int IcBedRegLevel { get; set; }
        public string IcBedRegLevelName { get; set; } = null!;
        public int IcBedRegType { get; set; }
        public string IcBedRegTypeName { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime IcBedRegDate { get; set; }
        public string IcBedRegNumber { get; set; } = null!;
        public Guid IcBedNameId { get; set; }
        public string IcBedName { get; set; } = null!;
        public int IcBedType { get; set; }
        public string IcBedTypeName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
