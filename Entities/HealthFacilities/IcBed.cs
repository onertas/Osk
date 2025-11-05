using Ardalis.SmartEnum;

namespace OskApi.Entities.HealthFacilities
{
    public class IcBed : Entity
    {
        public int HealthFacilityId { get; set; }
        public IcBedRegLevel IcBedRegLevel { get; set; } = IcBedRegLevel.Seviye1;
        public IcBedRegType IcBedRegType { get; set; } = IcBedRegType.Kesin;

        public int Quantity { get; set; }
        public DateTime IcBedRegDate { get; set; }
        public string IcBedRegNumber { get; set; }= null!;

        public IcBedName? IcBedName { get; set; }
        public int IcBedNameId { get; set; }
        public bool IsActive { get; set; } = true;



    }

    public class IcBedName : Entity
    {

        public string Name { get; set; } = null!;
        public int IcBedClassId { get; set; }
        public IcBedType IcBedType { get; set; } = IcBedType.Eriskin;
        
    }

    public class IcBedRegType : SmartEnum<IcBedRegType>
    {
        public static readonly IcBedRegType Kesin = new(nameof(Kesin), 1, "Kesin");
        public static readonly IcBedRegType Gecici = new(nameof(Gecici), 2, "Geçici");
        public static readonly IcBedRegType Pandemi = new(nameof(Pandemi), 3, "Pandemi Geçici");

        public string Description { get; }

        private IcBedRegType(string name, int value, string description)
            : base(name, value)
        {
            Description = description;
        }


    }

    public class IcBedType : SmartEnum<IcBedType>
    {
        public static readonly IcBedType Eriskin = new(nameof(Eriskin), 1, "Erişkin");
        public static readonly IcBedType Cocuk = new(nameof(Cocuk), 2, "Çocuk");
        public static readonly IcBedType Yenidogan = new(nameof(Yenidogan), 3, "Yenidoğan");

        public string Description { get; }

        private IcBedType(string name, int value, string description)
            : base(name, value)
        {
            Description = description;
        }


    }


    public class IcBedRegLevel : SmartEnum<IcBedRegLevel>
    {

        public static readonly IcBedRegLevel Seviye1 = new(nameof(Seviye1), 1, "Seviye 1");
        public static readonly IcBedRegLevel Seviye2 = new(nameof(Seviye2), 2, "Seviye 2");
        public static readonly IcBedRegLevel Seviye3 = new(nameof(Seviye3), 3, "Seviye 3");
        public static readonly IcBedRegLevel Seviye4a = new(nameof(Seviye4a), 4, "Seviye 4A");
        public static readonly IcBedRegLevel Seviye4b = new(nameof(Seviye4b), 5, "Seviye 4B");


        public string Description { get; }

        private IcBedRegLevel(string name, int value, string description)
            : base(name, value)
        {
            Description = description;
        }
    }

}
