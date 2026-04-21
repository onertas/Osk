using GenericRepository;
using OskApi.Data;
using OskApi.Entities.HealthFacilities;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class HealthFacilityTypeService : Repository<HealthFacilityType, MyDbContext>, IHealthFacilityTypeService
{
    public HealthFacilityTypeService(MyDbContext context) : base(context)
    {
    }
}

