using OskApi.Entities.Beds;
using OskApi.Services.Abstract;
using GenericRepository;
using OskApi.Data;

namespace OskApi.Services.Concrete
{
    public class IcBedNameService : Repository<IcBedName, MyDbContext>, IIcBedNameService
    {
        public IcBedNameService(MyDbContext context) : base(context)
        {
        }
    }
}
