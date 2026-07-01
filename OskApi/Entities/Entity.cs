namespace OskApi.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Code { get; set; }
        public bool IsDeleted { get; set; }
    }
}
