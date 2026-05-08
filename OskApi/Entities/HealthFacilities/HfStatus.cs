using Ardalis.SmartEnum;

namespace OskApi.Entities.HealthFacilities
{
    public class HfStatus : SmartEnum<HfStatus>
    {
        public static readonly HfStatus Kapanma = new(nameof(Kapanma), 0, "Kapanma");
        public static readonly HfStatus Aktif = new(nameof(Aktif), 1, "Aktif");
        public static readonly HfStatus Tasinma = new(nameof(Tasinma), 3, "Taşınma");
        public static readonly HfStatus RuhsatAski = new(nameof(RuhsatAski), 4, "Ruhsat Askı");
        public static readonly HfStatus FaaliyetDurdurma = new(nameof(FaaliyetDurdurma), 5, "Faaliyet Durdurma");

        public string Description { get; }

        private HfStatus(string name, int value, string description)
            : base(name, value)
        {
            Description = description;
        }
    }
}
