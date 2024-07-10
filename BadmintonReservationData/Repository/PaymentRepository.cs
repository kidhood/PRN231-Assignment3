using BadmintonReservationData.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.Repository
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
