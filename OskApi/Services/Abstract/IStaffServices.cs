using GenericRepository;
using OskApi.Entities.Staff;

namespace OskApi.Services.Abstract;

public interface IStaffService : IRepository<Staff>
{
}

public interface ITemporarayStaffService : IRepository<TemporarayStaff>
{
}
