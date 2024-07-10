using BadmintonReservationData.Base;
using BadmintonReservationData.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.Repository
{
    public class BookingDetailRepository : GenericRepository<BookingDetail>
    {
        public BookingDetailRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<int>> GetBookedFrameIdListAt(DateTime bookingDate)
        {
            return await this._dbSet
                            .Where(item => item.Status != (int)BookingStatus.Cancelled && item.Status != (int)BookingStatus.Failed)
                            .Where(item => item.BookDate.Date == bookingDate.Date)
                            .Select(item =>  item.FrameId)
                            .ToListAsync();
        }
    }
}
