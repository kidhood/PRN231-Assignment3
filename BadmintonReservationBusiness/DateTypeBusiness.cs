using BadmintonReservationData;
using BadmintonReservationData.DTOs;
using BadmintonReservationData.Enums;
using BadmintonReservationData.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationBusiness
{
    public class DateTypeBusiness
    {
        private readonly UnitOfWork unitOfWork;

        public DateTypeBusiness(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<BusinessResult> GetAllAsync()
        {
            try
            {
                var dataTypes = await this.unitOfWork.DateTypeRepository.GetAllAsync().ConfigureAwait(false);

                if (dataTypes == null)
                {
                    return new BusinessResult(400, "No date type data");
                }
                else
                {
                    return new BusinessResult(200, "Get date type list sucess", dataTypes);
                }
            }
            catch (Exception ex)
            {
                this.unitOfWork.RollbackTransaction();
                return new BusinessResult(500, ex.Message);
            }
        }
    }
}
