using BadmintonReservationData;
using BadmintonReservationData.DTOs;
using System.Linq.Expressions;

namespace BadmintonReservationBusiness
{
    public class BookingDetailBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingDetailBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await this._unitOfWork.BookingDetailRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return new BusinessResult(404, "No booking data");
                }
                else
                {
                    return new BusinessResult(200, "Get booking detail success", result);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(100, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateBookingDetailAsync(int id, UpdateBookingDetailRequestDTO updateRequest)
        {
            
            try
            {                
                var bookingDetail = await this._unitOfWork.BookingDetailRepository.GetByIdAsync(id);
                if (bookingDetail == null)
                {
                    return new BusinessResult(404, "Booking detail not found");
                }

                if (updateRequest.Status.HasValue)
                {
                    bookingDetail.Status = updateRequest.Status.Value;
                    bookingDetail.UpdatedDate = DateTime.Now;
                    await this._unitOfWork.BeginTransactionAsync();
                    this._unitOfWork.BookingDetailRepository.Update(bookingDetail);
                    await _unitOfWork.CommitTransactionAsync();

                    var booking = await this._unitOfWork.BookingRepository.GetByIdWithDetailsAsync(id);
                    if (booking.BookingDetails.All(d => d.Status == 3))
                    {
                        booking.Status = 3; // All details cancelled, cancel the booking
                        booking.UpdatedDate = DateTime.Now;
                        await this._unitOfWork.BeginTransactionAsync();
                        this._unitOfWork.BookingRepository.Update(booking);
                        await _unitOfWork.CommitTransactionAsync();
                    }
                    //bookingDetail.Booking = booking;
                }

                return new BusinessResult(200, "Booking detail updated successfully", bookingDetail);
            }
            catch (Exception ex)
            {
                this._unitOfWork.RollbackTransaction();
                return new BusinessResult(500, ex.Message);
            }
        }
    }

}