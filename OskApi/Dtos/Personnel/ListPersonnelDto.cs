namespace OskApi.Dtos.Personnel
{
    public class ListPersonnelDto
    {

        public Guid Id { get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsBanned { get; set; } = false;
        public DateTime? BirthDate { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<string>? Branches { get; set; } 
        public List<Guid>? BranchIds { get; set; }



    }
}
