namespace OskApi.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeteled { get; set; }
    }
}
