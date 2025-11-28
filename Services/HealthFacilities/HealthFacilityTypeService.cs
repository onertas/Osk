using GenericRepository;
using OskApi.Data;
using OskApi.Entities.HealthFacilities;

namespace OskApi.Services.HealthFacilities;

public class HealthFacilityTypeService : Repository<HealthFacilityType, MyDbContext>, IHealthFacilityTypeService
{
    public HealthFacilityTypeService(MyDbContext context) : base(context)
    {
    }
}

