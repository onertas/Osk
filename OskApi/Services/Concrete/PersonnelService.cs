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

        if (!string.IsNullOrWhiteSpace(query))
        {
            var s = query.Trim();
            var sLower = s.ToLower();
            var sLowerTr = s.ToLower(new System.Globalization.CultureInfo("tr-TR"));

            result = result.Where(x =>
                EF.Functions.Like(x.FirstName, $"%{s}%") ||
                EF.Functions.Like(x.LastName, $"%{s}%") ||
                EF.Functions.Like(x.FirstName + " " + x.LastName, $"%{s}%") ||
                x.FirstName.ToLower().Contains(sLower) ||
                x.LastName.ToLower().Contains(sLower) ||
                (x.FirstName + " " + x.LastName).ToLower().Contains(sLower) ||
                x.FirstName.ToLower().Contains(sLowerTr) ||
                x.LastName.ToLower().Contains(sLowerTr) ||
                (x.FirstName + " " + x.LastName).ToLower().Contains(sLowerTr)
            );
        }

        return result;
    }
}


