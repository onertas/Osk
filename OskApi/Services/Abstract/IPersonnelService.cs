using GenericRepository;
using OskApi.Entities.Personnel;

namespace OskApi.Services.Abstract;

public interface IPersonnelService : IRepository<Personnel>
{
    IQueryable<Personnel> Search(string? query);
}
