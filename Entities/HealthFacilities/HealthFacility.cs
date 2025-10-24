namespace OskApi.Entities.HealthFacilities
{
    public class HealthFacility:Entity
    {

        public int HealthFacilityTypeId { get; set; }
        public HealthFacilityType? HealthFacilityType { get; set; }
        public string Name { get; set; }=null!;
        public string? Code { get; set; }
        public string? Address { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; } 
    }

    public class HealthFacilityType : Entity
    {

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        
    }

    public class PrivateHospital : HealthFacility
    {

        

    }
    public class Bed : Entity
    {



    }
}
