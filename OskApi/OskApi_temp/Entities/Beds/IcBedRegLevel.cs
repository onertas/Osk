using Ardalis.SmartEnum;

namespace OskApi.Entities.Beds
{
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
