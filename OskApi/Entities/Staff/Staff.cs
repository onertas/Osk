using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;

namespace OskApi.Entities.Staff
{
    public class Staff : Entity
    {

        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }

        public Guid HealthFacilityId { get; set; }
        public HealthFacility? HealthFacility { get; set; }

        public int Count { get; set; }

    }

    public class TemporarayStaff : Entity
    {

        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }

        public Guid HealthFacilityId { get; set; }
        public HealthFacility? HealthFacility { get; set; }

        public Guid PmTypeId { get; set; }

        public PmType? PmType { get; set; }


        public int Count { get; set; }

    }
}
