using BadmintonReservationData;
using BadmintonReservationData.DTO;
using BadmintonReservationData.DTOs;
using BadmintonReservationData.Enums;
using System.Linq.Expressions;

namespace BadmintonReservationBusiness
{
    public class BookingBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAllAsync()
        {
            try
            {
                var result = await this._unitOfWork.BookingRepository.GetAllWithDetailsAsync();

                if (result == null)
                {
                    return new BusinessResult(404, "No booking data");
                }
                else
                {
                    return new BusinessResult(200, "Get booking list success", result);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAllWithFilterWithDetailsAsync(int pageIndex, int pageSize, BookingFilterDTO filterDTO)
        {
            try
            {
                var result = await this._unitOfWork.BookingRepository.GetAllWithFilterWithDetailsAsync(pageIndex, pageSize, filterDTO);

                if (result == null)
                {
                    return new BusinessResult(404, "No booking data");
                }
                else
                {
                    return new BusinessResult(200, "Get booking list success", result);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await this._unitOfWork.BookingRepository.GetByIdWithDetailsAsync(id);

                if (result == null)
                {
                    return new BusinessResult(404, "No booking data");
                }
                else
                {
                    return new BusinessResult(200, "Get booking success", result);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> CreateBookingAsync(CreateBookingRequestDTO bookingRequest)
        {
            await this._unitOfWork.BeginTransactionAsync();
            try
            {
                var booking = new Booking
                {
                    CustomerId = bookingRequest.CustomerId,
                    BookingTypeId = bookingRequest.BookingTypeId,
                    Status = bookingRequest.Status,
                    PromotionAmount = bookingRequest.PromotionAmount,
                    PaymentType = bookingRequest.PaymentType,
                    PaymentStatus = bookingRequest.PaymentStatus,
                    CreatedDate = DateTime.Now,
                    BookingDetails = bookingRequest.BookingDetails.Select(d => new BookingDetail
                    {
                        BookDate = d.BookDate,
                        FrameId = d.FrameId,
                        TimeFrom = d.TimeFrom,
                        TimeTo = d.TimeTo,
                        Price = d.Price,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    }).ToList(),
                    Payment = new Payment
                    {
                        Amount = Math.Max(0, bookingRequest.BookingDetails.Sum(item => item.Price) - bookingRequest.PromotionAmount),
                        Status = bookingRequest.PaymentStatus,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    }
                };

                if (bookingRequest.BookingTypeId == (int)BookingTypeEnum.Hourly)
                {
                    var customer = this._unitOfWork.CustomerRepository.GetById(bookingRequest.CustomerId);
                    var requestAmountOfTime = bookingRequest.BookingDetails.Sum(item => (item.TimeTo - item.TimeFrom) / 100 + ((item.TimeTo - item.TimeFrom)%100) / 60);
                    if (customer.TotalHoursMonthly < requestAmountOfTime)
                    {
                        return new BusinessResult(400, "Not enough amount of purchased hours for done this booking!");
                    }
                    else
                    {
                        customer.TotalHoursMonthly -= requestAmountOfTime;
                        this._unitOfWork.CustomerRepository.Update(customer);
                    }
                }

                try {
                    var listFrameId = bookingRequest.BookingDetails.Select(x => x.FrameId).ToList();
                    var listCourt = this._unitOfWork.FrameRepository.GetAllCourtWithFrameIds(listFrameId);
                    var listCourtUpdate = new List<int>();
                    foreach(var court in listCourt)
                    {
                        if(listCourtUpdate.All(x => x != court.Id))
                        {
                            int totalBooking = int.Parse(court.TotalBooking);
                            court.TotalBooking = (totalBooking + 1).ToString();
                        }
                    }

                    this._unitOfWork.CourtRepository.UpdateRange(listCourt);
                } catch (Exception ex)
                {
                }
                await this._unitOfWork.BookingRepository.CreateAsync(booking);
                await this._unitOfWork.CommitTransactionAsync();
                return new BusinessResult(200, "Booking created successfully", booking);
            }
            catch (Exception ex)
            {
                this._unitOfWork.RollbackTransaction();
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateBookingAsync(int id, UpdatePutBookingRequestDTO updateRequest)
        {
            await this._unitOfWork.BeginTransactionAsync();
            try
            {
                var booking = await this._unitOfWork.BookingRepository.GetByIdWithDetailsAsync(id);
                if (booking == null)
                {
                    return new BusinessResult(404, "Booking not found");
                }

                booking.CustomerId = updateRequest.CustomerId;
                booking.BookingTypeId = updateRequest.BookingTypeId;
                booking.Status = updateRequest.Status;
                booking.PromotionAmount = updateRequest.PromotionAmount;
                booking.PaymentType = updateRequest.PaymentType;
                booking.PaymentStatus = updateRequest.PaymentStatus;              
                booking.UpdatedDate = DateTime.Now;                

                foreach (var detail in updateRequest.BookingDetails)
                {
                    var bookingDetail = booking.BookingDetails.FirstOrDefault(d => d.Id == detail.Id);
                    if (bookingDetail != null)
                    {
                        bookingDetail.BookDate = detail.BookDate;
                        bookingDetail.FrameId = detail.FrameId;
                        bookingDetail.TimeFrom = detail.TimeFrom;
                        bookingDetail.TimeTo = detail.TimeTo;
                        bookingDetail.Price = detail.Price;
                    }
                    else
                    {
                        // Handle if the booking detail does not exist
                    }
                }

                if (booking.Payment != null)
                {
                    booking.Payment.Amount = Math.Max(0, booking.BookingDetails.Sum(item => item.Price) - booking.PromotionAmount);
                    booking.Payment.Status = booking.PaymentStatus;
                }

                booking.UpdatedDate = DateTime.Now;
                this._unitOfWork.BookingRepository.Update(booking);
                await this._unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Booking updated successfully", booking);
            }
            catch (Exception ex)
            {
                this._unitOfWork.RollbackTransaction();

                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateByPatchBookingAsync(int id, UpdatePatchBookingRequestDTO updateRequest)
        {
            await this._unitOfWork.BeginTransactionAsync();
            try
            {
                var booking = await this._unitOfWork.BookingRepository.GetByIdWithDetailsAsync(id);
                if (booking == null)
                {
                    return new BusinessResult(404, "Booking not found");
                }

                if (updateRequest.Status.HasValue)
                {
                    booking.Status = updateRequest.Status.Value;
                    booking.UpdatedDate = DateTime.Now;
                    switch (updateRequest.Status.Value)
                    {
                        case 1:
                            // pending -> skip
                            break;
                        case 2:
                            // successful -> skip
                            break;
                        case 3:
                            // failed -> update all details' status to failed
                            foreach(var details in booking.BookingDetails)
                            {
                                details.Status = 3;
                            }
                            break;
                        case 4:
                            // cancelled -> update all details' status to cancelled
                            foreach (var details in booking.BookingDetails)
                            {
                                details.Status = 4;
                            }
                            break;
                        default:
                            // default code block
                            break;
                    }                    
                }

                if (updateRequest.CustomerId.HasValue)
                {
                    booking.CustomerId = updateRequest.CustomerId.Value;
                    booking.UpdatedDate = DateTime.Now;                    
                }

                if (updateRequest.BookingTypeId.HasValue)
                {
                    booking.BookingTypeId = updateRequest.BookingTypeId.Value;
                    booking.UpdatedDate = DateTime.Now;
                }

                if (updateRequest.PromotionAmount.HasValue)
                {
                    booking.PromotionAmount = updateRequest.PromotionAmount.Value;
                    booking.UpdatedDate = DateTime.Now;
                }

                if (updateRequest.PaymentType.HasValue)
                {
                    booking.PaymentType = updateRequest.PaymentType.Value;
                    booking.UpdatedDate = DateTime.Now;
                }

                if (updateRequest.PaymentStatus.HasValue)
                {
                    booking.Payment.Status = updateRequest.PaymentStatus.Value;
                    booking.Payment.UpdatedDate = DateTime.Now;
                }

                booking.UpdatedDate = DateTime.Now;
                this._unitOfWork.BookingRepository.Update(booking);
                await this._unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Booking updated successfully", booking);
            }
            catch (Exception ex)
            {
                this._unitOfWork.RollbackTransaction();
                
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteBookingAsync(int id)
        {
            await this._unitOfWork.BeginTransactionAsync();
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetByIdWithDetailsAsync(id);
                if (booking == null)
                {
                    return new BusinessResult(404, "Booking not found");
                }

                _unitOfWork.BookingDetailRepository.RemoveRange(booking.BookingDetails);
                _unitOfWork.BookingRepository.Remove(booking);
                await _unitOfWork.CommitTransactionAsync();
                return new BusinessResult(200, "Booking deleted successfully");
            }
            catch (Exception ex)
            {
                this._unitOfWork.RollbackTransaction();
                return new BusinessResult(500, ex.Message);
            }
        }
    }

}