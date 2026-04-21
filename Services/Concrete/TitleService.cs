using GenericRepository;
using OskApi.Data;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class TitleService : Repository<Title, MyDbContext>, ITitleService
{
    public TitleService(MyDbContext context) : base(context)
    {
    }
}


