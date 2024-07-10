
using BadmintonReservationData;
using BadmintonReservationData.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationBusiness
{
    public class PurchasedBusiness
    {
        private readonly UnitOfWork unitOfWork;

        public PurchasedBusiness(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAllPurchased()
        {
            try
            {
                var purchaseds = this.unitOfWork.PurchasedRepository.GetAllWithCustomerAndPayment();

                if (purchaseds == null)
                {
                    return new BusinessResult(404, "No purchased data");
                }
                else
                {
                    var reusult = purchaseds.Select(x => {
                        var cus = x.Customer;
                        cus.PurchasedHoursMonthlies = null;
                        var pay = x.Payment;
                        pay.PurchasedHoursMonthlies = null;
                        return new PurchasedHoursMonthly
                        {
                            Id = x.Id,
                            CustomerId = x.CustomerId,
                            PaymentId = x.PaymentId,
                            AmountHour = x.AmountHour,
                            Status = x.Status,
                            CreatedDate = x.CreatedDate,
                            UpdatedDate = x.UpdatedDate,
                            Customer = cus,
                            Payment = pay
                        };
                    }).ToList();
                    return new BusinessResult(200, "Get purchased list success", reusult);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetPurchasedById(int id)
        {
            try
            {
                var purchased = this.unitOfWork.PurchasedRepository.GetWithCustomerById(id);

                if (purchased == null)
                {
                    return new BusinessResult(404, "No purchased data");
                }
                else
                {
                    var cus = purchased.Customer;
                    cus.PurchasedHoursMonthlies = null;
                    var pay = purchased.Payment;
                    pay.PurchasedHoursMonthlies = null;

                    purchased.Customer = cus;
                    purchased.Payment = pay;
                    return new BusinessResult(200, "Get purchased success", purchased);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(100, ex.Message);
            }
        }

        public async Task<IBusinessResult> CreatePurchased(CreatePurchasedRequestDTO createPurchasedRequest)
        {
            await this.unitOfWork.BeginTransactionAsync();
            try
            {
                var payment = new Payment
                {
                    Amount = createPurchasedRequest.AmountHour * 90000,
                    Status = 1,
                };

                var purchased = new PurchasedHoursMonthly
                {
                    AmountHour = createPurchasedRequest.AmountHour,
                    Status = createPurchasedRequest.Status,
                    CustomerId = createPurchasedRequest.CustomerId,
                    Payment = payment,
                };

                await this.unitOfWork.PurchasedRepository.CreateAsync(purchased);
                await this.unitOfWork.CommitTransactionAsync();
                return new BusinessResult(200, "Create Success");
            }
            catch (Exception ex)
            {
                this.unitOfWork.RollbackTransaction();
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdatePurchased(UpdatePurchasedRequestDTO updatePurchasedRequest)
        {
            await this.unitOfWork.BeginTransactionAsync();
            try
            {
                var purchased = await this.unitOfWork.PurchasedRepository.GetByIdAsync(updatePurchasedRequest.Id);

                if (purchased == null)
                {
                    return new BusinessResult(-1, "No purchased data");
                }

                purchased.AmountHour = updatePurchasedRequest.AmountHour;
                purchased.Status = updatePurchasedRequest.Status;
                purchased.CustomerId = updatePurchasedRequest.CustomerId;
                purchased.UpdatedDate = DateTime.Now;
          
                var payment = await this.unitOfWork.PaymentRepository.GetByIdAsync(purchased.PaymentId);
                if (payment != null)
                {
                    payment.Amount = updatePurchasedRequest.AmountHour * 90000;
                    payment.Status = 1;
                    this.unitOfWork.PaymentRepository.Update(payment);
                }

                this.unitOfWork.PurchasedRepository.Update(purchased);
                await this.unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Update purchased success");
            }
            catch (Exception ex)
            {
                this.unitOfWork.RollbackTransaction();
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> RemovePurchased(int id)
        {
            await this.unitOfWork.BeginTransactionAsync();
            try
            {
                var purchased = await unitOfWork.PurchasedRepository.GetByIdAsync(id);

                if (purchased == null)
                {
                    return new BusinessResult(-1, "No purchased data");
                }

                unitOfWork.PurchasedRepository.Remove(purchased);
                await unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Delete Success");
            }
            catch (Exception ex)
            {
                this.unitOfWork.RollbackTransaction();
                return new BusinessResult(500, ex.Message);
            }
        }
    }
}