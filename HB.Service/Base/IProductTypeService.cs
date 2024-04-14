using HB.Database.DbModels;
using HB.Model;

namespace HB.Service
{
    public interface IProductTypeService
    {
        List<ProductType> GetAllProductTypes();
    }
}