using GenericRepository;
using OskApi.Data;
using OskApi.Entities.Personnel;

namespace OskApi.Services.HealthFacilities;

public class TitleService : Repository<Title, MyDbContext>, ITitleService
{
    public TitleService(MyDbContext context) : base(context)
    {
    }
}

