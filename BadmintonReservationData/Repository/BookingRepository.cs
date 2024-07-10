using BadmintonReservationData.Base;
using BadmintonReservationData.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.Repository
{
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Booking>> GetAllWithDetailsAsync()
        {
            return await this._dbSet
                              .IgnoreAutoIncludes()
                              .Include(item => item.Customer)                              
                              .Include(item => item.BookingType)
                              .Include(item => item.Payment)
                              .Include(item => item.BookingDetails)
                              .ThenInclude(bookingDetail => bookingDetail.Frame)
                              .ThenInclude(frame => frame.Court)
                              .ToListAsync();
        }

        public async Task<PageableResponseDTO<Booking>> GetAllWithFilterWithDetailsAsync(int pageIndex, int pageSize, BookingFilterDTO filter)
        {
            // Query without pagination to get the total count
            var query = this._dbSet
                              .IgnoreAutoIncludes()
                              .Where(item => item.Id.ToString().Contains(filter.SearchText)
                                  || item.Customer.FullName.ToLower().Contains(filter.SearchText.ToLower())
                                  || item.Customer.PhoneNumber.ToLower().Contains(filter.SearchText.ToLower()))
                              .Where(item => filter.Status == 0 || item.Status == filter.Status)
                              .Where(item => filter.BookingType == 0 || item.BookingTypeId == filter.BookingType)
                              .Where(item => filter.PaymentType == 0 || item.PaymentType == filter.PaymentType)
                              .Where(item => filter.PaymentStatus == 0 || item.PaymentStatus == filter.PaymentStatus)
                              .Where(item => filter.BookingDateFrom == null || item.BookingDetails.Any(detail => detail.BookDate >= filter.BookingDateFrom))
                              .Where(item => filter.BookingDateTo == null || item.BookingDetails.Any(detail => detail.BookDate <= filter.BookingDateTo))
                              .Include(item => item.Customer)
                              .Include(item => item.BookingType)
                              .Include(item => item.Payment)
                              .Include(item => item.BookingDetails)
                              .ThenInclude(bookingDetail => bookingDetail.Frame)
                              .ThenInclude(frame => frame.Court)
                              .OrderByDescending(item => item.CreatedDate);

            var totalItemCount = await query.CountAsync();
            var totalOfPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

            // Apply pagination
            var list = await query.Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return new PageableResponseDTO<Booking>()
            {
                List = list.ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalOfPages = totalOfPages
            };
        }

        public async Task<Booking> GetByIdWithDetailsAsync(int id)
        {
            return await this._dbSet
                              .IgnoreAutoIncludes()
                              .Include(item => item.Customer)
                              .Include(item => item.BookingType)
                              .Include(item => item.Payment)
                              .Include(item => item.BookingDetails)
                              .ThenInclude(bookingDetail => bookingDetail.Frame)
                              .ThenInclude(frame => frame.Court)
                              .FirstOrDefaultAsync(item => item.Id == id);
        }
    }
}
