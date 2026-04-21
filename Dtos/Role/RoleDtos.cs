namespace OskApi.Dtos.Role
{
    public class CreateRoleDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateRoleDto : CreateRoleDto
    {
        public Guid Id { get; set; }
    }

    public class ListRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
