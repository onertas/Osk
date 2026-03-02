using GenericRepository;
using OskApi.Data;
using OskApi.Entities.Personnel;

namespace OskApi.Services.HealthFacilities;

public class BranchService : Repository<Branch, MyDbContext>, IBranchService
{
    public BranchService(MyDbContext context) : base(context)
    {
    }
}

