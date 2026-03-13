using OskApi.Entities;
using OskApi.Entities.Personnel;

namespace OskApi.Dtos.Personnel
{
    public class CreatePersonnelDto
    {
      
            public string IdentityNumber { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public bool IsBanned { get; set; } = false;
            public ICollection<Guid>? PersonnelBranches { get; set; }


        
    }
}
