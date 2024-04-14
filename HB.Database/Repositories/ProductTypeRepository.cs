using HB.Database.DbModels;
using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Database.Repositories
{
    public class ProductTypeRepository : RepositoryBase<ProductType>, IProductTypeRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<ProductType> _dbSet;
        #endregion

        #region Ctor
        public ProductTypeRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.ProductType;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<ProductType> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<ProductType> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
