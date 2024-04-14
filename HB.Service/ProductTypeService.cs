using HB.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Utilities;
using HB.Model;
using HB.Database.DbModels;
using Strateq.Core.Utilities;

namespace HB.Service
{
    public class ProductTypeService : IProductTypeService
    {
        #region Fields
        private readonly IProductTypeRepository _productTypeRepository;
        #endregion

        #region Ctor
        public ProductTypeService(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository ?? throw new Exception(nameof(productTypeRepository));
        }
        #endregion

        #region Methods

        public List<ProductType> GetAllProductTypes()
        {
            return _productTypeRepository.GetAll().ToList();
        }

        #endregion
    }
}
