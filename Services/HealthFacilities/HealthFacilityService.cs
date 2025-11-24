using GenericRepository;
using OskApi.Data;
using OskApi.Entities.HealthFacilities;

namespace OskApi.Services.HealthFacilities;

public class HealthFacilityService : Repository<HealthFacility, MyDbContext>, IHealthFacilityService
{
    public HealthFacilityService(MyDbContext context) : base(context)
    {
    }
}

