using BadmintonReservationData;
using BadmintonReservationData.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationBusiness
{
    public class CustomerBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetCustomerList()
        {
            try
            {
                var customers = await this._unitOfWork.CustomerRepository.GetAllAsync();
                if (customers == null)
                {
                    return new BusinessResult(400, "No customer data");
                }
                else
                {
                    return new BusinessResult(200, "Get available customer list sucess", customers);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, ex.Message);
            }
        }

        public BusinessResult LoginLogic(LoginDTO login)
        {
            var loginResult = this._unitOfWork.CustomerRepository.GetAccount(login.PhoneNumber, login.Password);
            if (loginResult != null)
            {
                return new BusinessResult(200, "Login Success", loginResult);
            }
            else
            {
                return new BusinessResult(401, "Phone number or password fail", null);
            }

        }
    }
}
