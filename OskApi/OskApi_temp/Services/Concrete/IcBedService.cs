using OskApi.Entities.Beds;
using OskApi.Services.Abstract;
using GenericRepository;
using OskApi.Data;

namespace OskApi.Services.Concrete
{
    public class IcBedService : Repository<IcBed, MyDbContext>, IIcBedService
    {
        public IcBedService(MyDbContext context) : base(context)
        {
        }
    }
}
