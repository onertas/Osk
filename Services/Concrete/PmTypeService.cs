using GenericRepository;
using OskApi.Data;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class PmTypeService : Repository<PmType, MyDbContext>, IPmTypeService
{
    public PmTypeService(MyDbContext context) : base(context)
    {
    }
}


