using Ardalis.SmartEnum;

namespace OskApi.Entities.Beds
{
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

}
