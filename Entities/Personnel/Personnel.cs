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


