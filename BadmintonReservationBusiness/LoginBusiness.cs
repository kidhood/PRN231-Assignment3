using BadmintonReservationData;
using BadmintonReservationData.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationBusiness
{
    public class LoginBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public LoginBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
