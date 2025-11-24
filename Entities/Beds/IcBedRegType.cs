using Ardalis.SmartEnum;

namespace OskApi.Entities.Beds
{
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

}
