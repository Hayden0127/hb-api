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
    public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<Booking> _dbSet;
        #endregion

        #region Ctor
        public BookingRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.Booking;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<Booking> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<Booking> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
