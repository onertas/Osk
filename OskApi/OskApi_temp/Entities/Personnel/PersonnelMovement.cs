using OskApi.Entities.HealthFacilities;

namespace OskApi.Entities.Personnel;

public class PersonnelMovement : Entity
{
    public DateTime Start { get; set; }
    public DateTime? Finish { get; set; }
    public DateTime? ContractStart { get; set; }
    public DateTime? ContractFinish { get; set; }
    public string Description { get; set; }= string.Empty;


    public Guid PmTypeId { get; set; }
    public PmType? PmType { get; set; }

    public Guid BranchId { get; set; }
    public Branch? Branch { get; set; }

    public  Guid HealthFacilityId { get; set; }
    public  HealthFacility? HealthFacility { get; set; }
    public Guid AfiliatedUnitId { get; set; }


    public Guid PersonnelId { get; set; }
    public Personnel? Personnel { get; set; }

    public bool IsSgk { get; set; } = true;

}


