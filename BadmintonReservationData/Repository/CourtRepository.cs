using BadmintonReservationData.Base;
using BadmintonReservationWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.Repository
{
    public class CourtRepository : GenericRepository<Court>
    {
        public CourtRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {            
        }

        public async Task<(List<Court> Courts, int TotalItems)> GetAllWithCondition(CourtSearchConditionDTO searchCondition)
        {
            var query = this._dbSet.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchCondition.SearchText))
            {
                if (searchCondition.SearchMode == 1)
                {
                    query = query.Where(c => c.Name.Contains(searchCondition.SearchText));
                }
                if (searchCondition.SearchMode == 2)
                {
                    query = query.Where(c => c.TotalBooking.Contains(searchCondition.SearchText));
                }
                if (searchCondition.SearchMode == 3)
                {
                    query = query.Where(c => c.Amentities.Contains(searchCondition.SearchText));
                }
                if (searchCondition.SearchMode == 4 && int.TryParse(searchCondition.SearchText, out var capacity))
                {
                    query = query.Where(c => c.Capacity >= capacity);
                }
                // Add more conditions based on search mode if needed
            }

            if (searchCondition.StatusFilter > 0)
            {
                query = query.Where(c => c.Status == searchCondition.StatusFilter);
            }

            if (searchCondition.SurfaceTypeFilter > 0)
            {
                query = query.Where(c => c.SurfaceType == searchCondition.SurfaceTypeFilter);
            }

            if (searchCondition.CourtType > 0)
            {
                query = query.Where(c => c.CourtType == searchCondition.CourtType);
            }

            if (searchCondition.OpeningHours > 0)
            {
                query = query.Where(c => c.OpeningHours <= searchCondition.OpeningHours);
            }

            if (searchCondition.CloseHours > 0)
            {
                query = query.Where(c => c.CloseHours >= searchCondition.CloseHours);
            }

            // Get the total count for paging
            var totalCount = await query.CountAsync();

            // Apply paging
            var courts = await query
                .Skip((searchCondition.PageIndex - 1) * searchCondition.PageSize)
                .Take(searchCondition.PageSize)
                .ToListAsync();

            return (courts, totalCount);
        }

        public List<Court> FindCourtsByFrameId(int[] ids)
        {
            return this._dbSet.Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
