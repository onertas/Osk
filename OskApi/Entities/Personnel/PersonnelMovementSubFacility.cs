using OskApi.Entities.HealthFacilities;

namespace OskApi.Entities.Personnel;

public class PersonnelMovementSubFacility : Entity
{
    public Guid PersonnelMovementId { get; set; }
    public PersonnelMovement? PersonnelMovement { get; set; }

    public Guid SubFacilityId { get; set; }
    public HealthFacility? SubFacility { get; set; }
}
