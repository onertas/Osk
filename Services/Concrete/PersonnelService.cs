using GenericRepository;
using Microsoft.EntityFrameworkCore;
using OskApi.Data;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class PersonnelService : Repository<Personnel, MyDbContext>, IPersonnelService
{
    public PersonnelService(MyDbContext context) : base(context)
    {
    }

    public IQueryable<Personnel> Search(string? query)
    {
        var result = GetAll()
            .Include(i => i.PersonnelBranches!)
                .ThenInclude(i => i.Branch!)
                    .ThenInclude(b => b.Title)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            query = query.ToLower();
            result = result.Where(x => 
                x.FirstName.ToLower().Contains(query) || 
                x.LastName.ToLower().Contains(query));
        }

        return result;
    }
}


