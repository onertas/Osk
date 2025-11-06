namespace OskApi.Entities.HealthFacilities
{
    public class IcBedName : Entity
    {

        public string Name { get; set; } = null!;
        public IcBedType IcBedType { get; set; } = IcBedType.Eriskin;
        
    }

}
