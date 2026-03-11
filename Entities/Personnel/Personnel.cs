using OskApi.Entities.HealthFacilities;
using System.Collections;

namespace OskApi.Entities.Personnel;

public class Personnel : Entity
{
    public string IdentityNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsBanned { get; set; } = false;
    public ICollection<PersonnelBranch>? PersonnelBranches { get; set; }


}

public class PersonnelMovement : Entity
{
    public DateTime Start { get; set; }
    public DateTime? Finish { get; set; }
    public DateTime? ContractStart { get; set; }
    public DateTime? ContractFinish { get; set; }
    public string Description { get; set; }


    public int PmTypeId { get; set; }
    public virtual PmType PmType { get; set; }

    public int BranchId { get; set; }
    public virtual Branch Branch { get; set; }

    public int HealthFacilityId { get; set; }
    public virtual HealthFacility HealthFacility { get; set; }
    public int AfiliatedUnitId { get; set; }


    public int PersonnelId { get; set; }
    public virtual Personnel Personnel { get; set; }

    public bool IsUsingQuota { get; set; }
    public bool IsSgk { get; set; } = true;

}

public class PmType : Entity
{
    public string Code { get; set; }
    public string Name { get; set; }
   
}


