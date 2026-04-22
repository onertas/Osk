using GenericRepository;
using OskApi.Data;
using OskApi.Entities.Staff;
using OskApi.Services.Abstract;

namespace OskApi.Services.Concrete;

public class StaffService : Repository<Staff, MyDbContext>, IStaffService
{
    public StaffService(MyDbContext context) : base(context)
    {
    }
}

public class TemporarayStaffService : Repository<TemporarayStaff, MyDbContext>, ITemporarayStaffService
{
    public TemporarayStaffService(MyDbContext context) : base(context)
    {
    }
}
