using GenericRepository;
using OskApi.Data;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class PersonnelService : Repository<Personnel, MyDbContext>, IPersonnelService
{
    public PersonnelService(MyDbContext context) : base(context)
    {
    }
}


