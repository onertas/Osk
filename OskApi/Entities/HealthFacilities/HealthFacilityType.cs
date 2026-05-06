namespace OskApi.Entities.HealthFacilities
{
    public class HealthFacilityType : Entity
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public int MenuOrder { get; set; }

        public bool ShowBed { get; set; }
        public bool ShowDevice { get; set; }
        public bool ShowStaff { get; set; }
        public bool ShowTempStaff { get; set; }
        public bool ShowPm { get; set; }


    }
}
