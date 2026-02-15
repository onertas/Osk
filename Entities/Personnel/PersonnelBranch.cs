namespace OskApi.Entities.Personnel
{
    public class PersonnelBranch:Entity
    {
        public Guid PersonnelId { get; set; }
        public Personnel? Personnel { get; set; }

        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }
    }

}
