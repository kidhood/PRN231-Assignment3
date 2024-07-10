using BadmintonReservationData.Base;
using BadmintonReservationData.DTO;
using BadmintonReservationData.Enums;
using BadmintonReservationData.Utils;
using Microsoft.EntityFrameworkCore;

namespace BadmintonReservationData.Repository
{
    public class FrameRepository : GenericRepository<Frame>
    {
        public FrameRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Frame>> GetAllFrameAvailableForDate()
        {
            return await this._dbSet
                .IgnoreAutoIncludes()
                .Where(item => item.Status == (int)FrameStatus.Active)
                .Include(item => item.Court)
                .ToListAsync();
        }

        public async Task<List<Frame>> GetAllFrameAvailableOfCourtForDate(int id)
        {
            return await this._dbSet
                .IgnoreAutoIncludes()
                .Where(item => item.CourtId == id)
                .Where(item => item.Status == (int)FrameStatus.Active)
                .Include(item => item.Court)
                .ToListAsync();
        }

        public async Task<PageableResponseDTO<Frame>> GetAllWithCourtAsync(int pageIndex, int pageSize, FrameFilterDTO filterDTO)
        {
            var query = _dbSet
                .Include(item => item.Court)
                .Where(item => item.Id.ToString().Contains(filterDTO.SearchText)
                        || item.Label.ToLower().Contains(filterDTO.SearchText.ToLower())
                        || item.Note.ToLower().Contains(filterDTO.SearchText.ToLower())
                        || item.Court.Name.ToLower().Contains(filterDTO.SearchText.ToLower())
                )
                .Where(item => item.Price >= filterDTO.Price)
                .Where(item => filterDTO.FrameStatus == 0 || item.Status == filterDTO.FrameStatus)
                .Where(item =>
                    (filterDTO.TimeFrom != 0 && filterDTO.TimeTo != 0 && item.TimeFrom >= filterDTO.TimeFrom && item.TimeTo <= filterDTO.TimeTo) ||
                    (filterDTO.TimeFrom != 0 && filterDTO.TimeTo == 0 && item.TimeFrom >= filterDTO.TimeFrom) ||
                    (filterDTO.TimeFrom == 0 && filterDTO.TimeTo != 0 && item.TimeFrom <= filterDTO.TimeTo) ||
                    (filterDTO.TimeFrom == 0 && filterDTO.TimeTo == 0)
                )
                .Where(item => item.Status != (int)FrameStatus.Delete)
                .OrderByDescending(item => item.CreatedDate);
            var totalItemCount = await query.CountAsync();
            var totalOfPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

            // Apply pagination
            var list = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageableResponseDTO<Frame>()
            {
                List = list.ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalOfPages = totalOfPages
            };
        }

        public async Task<Frame?> GetByIdWithCourtAsync(int id)
        {
            return await _dbSet
                .Where(item => item.Id == id && item.Status != (int)FrameStatus.Delete)
                .Include(item => item.Court)
                .SingleOrDefaultAsync();
        }

        public async Task<bool?> GetExistedFrameForUpdate(int oldTimeFrom, int oldTimeTo, int oldCourtId, int timeFrom,
            int timeTo, int courtId)
        {
            return await _dbSet
                .AnyAsync(item =>
                    item.CourtId == courtId
                    && item.Status != (int)FrameStatus.Delete
                    && !(item.TimeFrom == oldTimeFrom && item.TimeTo == oldTimeTo && item.CourtId == oldCourtId)
                    && (
                        (item.TimeFrom <= timeFrom && item.TimeTo >= timeTo) || // Case 1: encompasses the input range
                        (item.TimeFrom <= timeFrom && item.TimeTo > timeFrom &&
                         item.TimeTo < timeTo) || // Case 2: starts before and ends within the input range
                        (item.TimeTo >= timeTo && item.TimeFrom < timeTo &&
                         item.TimeFrom > timeFrom) || // Case 3: starts within and ends after the input range
                        (item.TimeFrom > timeFrom && item.TimeTo < timeTo) // Case 4: completely inside the input range
                    )
                );
        }

        public async Task<bool?> GetExistedFrameForCreate(int timeFrom, int timeTo, int courtId)
        {
            return await _dbSet
                .AnyAsync(item =>
                    item.CourtId == courtId
                    && item.Status != (int)FrameStatus.Delete
                    && (
                        (item.TimeFrom <= timeFrom && item.TimeTo >= timeTo) || // Case 1: encompasses the input range
                        (item.TimeFrom <= timeFrom && item.TimeTo > timeFrom &&
                         item.TimeTo < timeTo) || // Case 2: starts before and ends within the input range
                        (item.TimeTo >= timeTo && item.TimeFrom < timeTo &&
                         item.TimeFrom > timeFrom) || // Case 3: starts within and ends after the input range
                        (item.TimeFrom > timeFrom && item.TimeTo < timeTo) // Case 4: completely inside the input range
                    )
                );
        }

        public List<Frame> GetAllWithCourtId(int courtId)
        {
            return this._dbSet.Where(x => x.CourtId == courtId).ToList();
        }

        public List<Frame> GetListByCourtId(int id)
        {
            return this._dbSet.Where(x => x.CourtId == id).ToList();
        }

        public async Task<Frame?> GetExistedFrame(int timeFrom, int timeTo, int courtId)
        {
            return await _dbSet.Where(
                item => item.TimeFrom == timeFrom
                        && item.TimeTo == timeTo
                        && item.CourtId == courtId
                        && item.Status != (int)FrameStatus.Delete
                        )
                        .Include(item => item.Court)
                        .SingleOrDefaultAsync();
        }

        public List<Court> GetAllCourtWithFrameIds(List<int> ids)
        {
            return this._dbSet.Where(x => ids.Contains(x.Id)).Select(x => x.Court).ToList();
        }
    }
}