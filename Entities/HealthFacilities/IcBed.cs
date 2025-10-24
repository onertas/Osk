namespace OskApi.Entities.HealthFacilities
{
    public class IcBed : Entity
    {
        public int UnitId { get; set; }
        public int IcBedClassId { get; set; }
        public IcBedRegLevel IcBedRegLevel { get; set; }
        public IcBedRegType IcBedRegType { get; set; }

        public int Quantity { get; set; }
        public DateTime IcBedRegDate { get; set; }
        public string IcBedRegNumber { get; set; }

        public IcBedClass IcBedClass { get; set; }
        public bool IsActive { get; set; } = true;



    }
    public enum IcBedRegType
    {
        Kesin = 1,
        Geçici = 2,
        Pandemi = 3
    }

    public enum IcBedRegLevel
    {
        Seviye1 = 1,
        Seviye2 = 2,
        Seviye3 = 3,
        Seviye4A = 4,
        Seviye4B = 5
    }

}
