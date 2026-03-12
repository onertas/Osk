using OskApi.Entities.HealthFacilities;

namespace OskApi.Entities.Personnel;

public class PersonnelMovement : Entity
{
    public DateTime Start { get; set; }
    public DateTime? Finish { get; set; }
    public DateTime? ContractStart { get; set; }
    public DateTime? ContractFinish { get; set; }
    public string Description { get; set; }


    public Guid PmTypeId { get; set; }
    public virtual PmType PmType { get; set; }

    public Guid BranchId { get; set; }
    public virtual Branch Branch { get; set; }

    public Guid HealthFacilityId { get; set; }
    public virtual HealthFacility HealthFacility { get; set; }
    public int AfiliatedUnitId { get; set; }


    public Guid PersonnelId { get; set; }
    public virtual Personnel Personnel { get; set; }

    public bool IsUsingQuota { get; set; }
    public bool IsSgk { get; set; } = true;

}


