using GenericRepository;
using OskApi.Data;
using OskApi.Entities.HealthFacilities;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class HealthFacilityService : Repository<HealthFacility, MyDbContext>, IHealthFacilityService
{
    public HealthFacilityService(MyDbContext context) : base(context)
    {
    }
}

