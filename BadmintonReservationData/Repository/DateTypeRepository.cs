using BadmintonReservationData.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.Repository
{
    public class DateTypeRepository : GenericRepository<DateType>
    {
        public DateTypeRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
