using GenericRepository;
using OskApi.Data;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class PmService : Repository<PersonnelMovement, MyDbContext>, IPmService
{
    public PmService(MyDbContext context) : base(context)
    {
    }
}


