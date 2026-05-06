using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;

namespace OskApi.Entities.Staff
{
    public class Staff : Entity
    {

        public int StaffNo { get; set; }
        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }

        public Guid HealthFacilityId { get; set; }
        public HealthFacility? HealthFacility { get; set; }

        public DateOnly Date { get; set; }

        public string Reason { get; set; } = string.Empty;

        public int Count { get; set; }

    }
}
