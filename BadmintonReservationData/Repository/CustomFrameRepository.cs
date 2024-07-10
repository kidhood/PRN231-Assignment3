using BadmintonReservationData.Base;
using Microsoft.EntityFrameworkCore;

namespace BadmintonReservationData.Repository;

public class CustomFrameRepository: GenericRepository<CustomFrame>
{
    public CustomFrameRepository(UnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public List<CustomFrame> GetAllWithFrame()
    {
        return this._dbSet.Include(x => x.Frame)
            .ThenInclude(frame => frame.Court)
            .ToList();
    }
}
