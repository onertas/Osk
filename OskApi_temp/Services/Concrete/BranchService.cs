using GenericRepository;
using OskApi.Data;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class BranchService : Repository<Branch, MyDbContext>, IBranchService
{
    public BranchService(MyDbContext context) : base(context)
    {
    }
}

