using BadmintonReservationData;
using BadmintonReservationData.DTOs;
using BadmintonReservationData.Enums;
using BadmintonReservationData.Utils;

namespace BadmintonReservationBusiness
{
    public class CustomFrameBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomFrameBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                var customFrames = this._unitOfWork.CustomFrameRepository.GetAllWithFrame();          

                if (customFrames == null)
                {
                    return new BusinessResult(400, "No custome frame data");
                }
                else
                {
                    var result = customFrames.Select(x => new CustomFrameDTO
                    {
                        Id = x.Id,
                        DateTypeName = EnumHelper.GetEnumDescription<DateTypes>(x.FixedDateTypeId),
                        Price = x.Price,
                        Status = x.Status,
                        FrameId = x.FrameId,
                        //TimeFrom = x.Frame.TimeFrom,
                        //TimeTo = x.Frame.TimeTo,
                        CourtName = x.Frame.Court.Name,
                        UpdatedDate = x.UpdatedDate,
                        SpecificDate = x.SpecificDate.Date,
                    }).ToList();

                    return new BusinessResult(200, "Get custome frame list sucess", result);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            try
            {
                var customFrames = this._unitOfWork.CustomFrameRepository.GetById(id);

                if (customFrames == null)
                {
                    return new BusinessResult(-1, "No custome frame data");
                }
                else
                {
                    return new BusinessResult(200, "Get custome frame sucess", customFrames);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> CreateCustomFrame(CreateCustomFrameRequestDTO customFrameRequest)
        {
            await this._unitOfWork.BeginTransactionAsync();
            try
            {
                var customFrame = new CustomFrame
                {
                    FrameId = customFrameRequest.FrameId,
                    Price = customFrameRequest.Price,
                    SpecificDate = customFrameRequest.SpecificDate,
                    Status = customFrameRequest.Status,
                    FixedDateTypeId = customFrameRequest.DateType
                };

                await this._unitOfWork.CustomFrameRepository.CreateAsync(customFrame);              
                await this._unitOfWork.CommitTransactionAsync();
                return new BusinessResult(200, "Create Success");
            }
            catch (Exception ex)
            {
                this._unitOfWork.RollbackTransaction();
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateCustomFrame(UpdateCustomFrameRequestDTO updateCustomFrameRequestDto)
        {
            await this._unitOfWork.BeginTransactionAsync();
            try
            {
                var frame = await _unitOfWork.CustomFrameRepository.GetByIdAsync(updateCustomFrameRequestDto.Id);

                if (frame == null)
                {
                    return new BusinessResult(400, "No custom frame data");
                }

                frame.Status = updateCustomFrameRequestDto.Status;
                frame.Price = updateCustomFrameRequestDto.Price;
                frame.FrameId = updateCustomFrameRequestDto.FrameId;
                frame.FixedDateTypeId = updateCustomFrameRequestDto.DateType;
                frame.SpecificDate = updateCustomFrameRequestDto.SpecificDate;

                _unitOfWork.CustomFrameRepository.Update(frame);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Update custom frame success");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> RemoveCustomFrame(int id)
        {
            await this._unitOfWork.BeginTransactionAsync();
            try
            {
                var customFrame = await _unitOfWork.CustomFrameRepository.GetByIdAsync(id);

                if (customFrame == null)
                {
                    return new BusinessResult(-1, "No custom frame data");
                }
                _unitOfWork.CustomFrameRepository.Remove(customFrame);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Delete custom frame Success");
            }
            catch (Exception ex)
            {
                this._unitOfWork.RollbackTransaction();
                return new BusinessResult(500, ex.Message);
            }
        }
    }
}
