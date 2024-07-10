using BadmintonReservationData.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.Repository
{
    public class PurchasedRepository : GenericRepository<PurchasedHoursMonthly>
    {
        public PurchasedRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<PurchasedHoursMonthly> GetAllWithCustomerAndPayment()
        {
            return this._dbSet
                .IgnoreAutoIncludes()
                .Include(x => x.Customer)
                .Include(x => x.Payment)
                .ToList();
        }

        public PurchasedHoursMonthly GetWithCustomerById(int id)
        {
            return this._dbSet.Include(x => x.Customer).Include(x => x.Payment).Where(x => x.Id == id).FirstOrDefault();
        }
    }
}