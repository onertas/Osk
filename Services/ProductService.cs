using GenericRepository;
using OskApi.Data;
using OskApi.Entities;

namespace OskApi.Services;

public class ProductService : Repository<Product, MyDbContext>, IProductService
{
    public ProductService(MyDbContext context) : base(context)
    {
    }
}

