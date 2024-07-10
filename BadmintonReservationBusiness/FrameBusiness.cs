using BadmintonReservationData;
using BadmintonReservationData.DTO;
using BadmintonReservationData.DTOs;
using BadmintonReservationData.Enums;
using BadmintonReservationData.Utils;

namespace BadmintonReservationBusiness;

public class FrameBusiness
{
    private readonly UnitOfWork unitOfWork;

    public FrameBusiness(UnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IBusinessResult> GetAll(int pageIndex, int pageSize, FrameFilterDTO filterDTO)
    {
        try
        {
            var frames = await unitOfWork.FrameRepository.GetAllWithCourtAsync(pageIndex, pageSize, filterDTO);
            if (frames == null)
            {
                return new BusinessResult(400, "No frame data");
            }
            else
            {
                return new BusinessResult(200, "Get frame list success", frames);
            }
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, ex.Message);
        }
    }

    public async Task<IBusinessResult> GetAllFrameAvailableForDate(DateTime bookingDate)
    {
        try
        {
            var frames = await this.unitOfWork.FrameRepository.GetAllFrameAvailableForDate();
            var bookedFrameIdList = await this.unitOfWork.BookingDetailRepository.GetBookedFrameIdListAt(bookingDate);

            var availableFrameForBookingDate =
                frames.Where(frame => !bookedFrameIdList.Any(item => item == frame.Id)).ToList();

            if (availableFrameForBookingDate == null)
            {
                return new BusinessResult(400, "No frame data");
            }
            else
            {
                return new BusinessResult(200, "Get available frame list sucess", availableFrameForBookingDate);
            }
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, ex.Message);
        }
    }

    public async Task<IBusinessResult> GetAllFrameAvailableOfCourtForDate(int id, DateTime bookingDate)
    {
        try
        {
            var frames = await this.unitOfWork.FrameRepository.GetAllFrameAvailableOfCourtForDate(id);
            var bookedFrameIdList = await this.unitOfWork.BookingDetailRepository.GetBookedFrameIdListAt(bookingDate);

            var availableFrameForBookingDate =
                frames.Where(frame => !bookedFrameIdList.Any(item => item == frame.Id)).ToList();

            if (availableFrameForBookingDate == null)
            {
                return new BusinessResult(400, "No frame data");
            }
            else
            {
                return new BusinessResult(200, "Get available frame list sucess", availableFrameForBookingDate);
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
            var frame = await unitOfWork.FrameRepository.GetByIdAsync(id);

            if (frame == null)
            {
                return new BusinessResult(400, "No frame data");
            }

            var response = new FrameResponseDTO
            {
                Id = frame.Id,
                Label = frame.Label,
                Note = frame.Note,
                TimeFrom = frame.TimeFrom,
                TimeTo = frame.TimeTo,
                Price = frame.Price,
                Status = frame.Status,
                CourtId = frame.CourtId
            };
            return new BusinessResult(200, "Get frame success", response);
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, ex.Message);
        }
    }

    public async Task<IBusinessResult> GetByIdIncludeCourt(int id)
    {
        try
        {
            var frame = await unitOfWork.FrameRepository.GetByIdWithCourtAsync(id);

            if (frame == null)
            {
                return new BusinessResult(400, "No frame data");
            }

            return new BusinessResult(200, "Get frame success", frame);
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, ex.Message);
        }
    }

    public async Task<IBusinessResult> CreateFrame(CreateFrameRequestDTO createFrameRequestDto)
    {
        //Check existed Frame
        var frameExisted = await unitOfWork.FrameRepository.GetExistedFrameForCreate(createFrameRequestDto.TimeFrom,
            createFrameRequestDto.TimeTo, createFrameRequestDto.CourtId);
        if (frameExisted == true)
        {
            return new BusinessResult(400,
                $"A frame already exists for the court from {TimeConverter.ConvertIntTime(createFrameRequestDto.TimeFrom)} to {TimeConverter.ConvertIntTime(createFrameRequestDto.TimeTo)}.");
        }

        await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var frame = new Frame()
            {
                Label = createFrameRequestDto.Label,
                Note = createFrameRequestDto.Note,
                TimeFrom = createFrameRequestDto.TimeFrom,
                TimeTo = createFrameRequestDto.TimeTo,
                Status = createFrameRequestDto.Status,
                Price = createFrameRequestDto.Price,
                CourtId = createFrameRequestDto.CourtId
            };

            await unitOfWork.FrameRepository.CreateAsync(frame);
            await unitOfWork.CommitTransactionAsync();
            return new BusinessResult(200, "Create Success");
        }
        catch (Exception ex)
        {
            this.unitOfWork.RollbackTransaction();
            return new BusinessResult(500, ex.Message);
        }
    }

    public async Task<IBusinessResult> UpdateFrame(UpdateFrameRequestDTO updateFrameRequestDto)
    {
        if (!(TimeConverter.ConvertToInt(updateFrameRequestDto.TimeFrom) == updateFrameRequestDto.OldTimeFrom &&
              TimeConverter.ConvertToInt(updateFrameRequestDto.TimeTo) == updateFrameRequestDto.OldTimeTo &&
              updateFrameRequestDto.CourtId == updateFrameRequestDto.OldCourtId))
        {
            //Check existed Frame
            var frameExisted = await unitOfWork.FrameRepository.GetExistedFrameForUpdate(
                updateFrameRequestDto.OldTimeFrom, updateFrameRequestDto.OldTimeTo, updateFrameRequestDto.OldCourtId,
                TimeConverter.ConvertToInt(updateFrameRequestDto.TimeFrom),
                TimeConverter.ConvertToInt(updateFrameRequestDto.TimeTo), updateFrameRequestDto.CourtId);
            if (frameExisted == true)
            {
                return new BusinessResult(400,
                    $"A frame already exists for the court from {TimeConverter.ConvertIntTime(updateFrameRequestDto.TimeFrom.Hours * 100 + updateFrameRequestDto.TimeFrom.Minutes)} to {TimeConverter.ConvertIntTime(updateFrameRequestDto.TimeTo.Hours * 100 + updateFrameRequestDto.TimeTo.Minutes)}.");
            }
        }

        await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var frame = await unitOfWork.FrameRepository.GetByIdAsync(updateFrameRequestDto.Id);

            if (frame == null)
            {
                return new BusinessResult(400, "No frame data");
            }

            frame.Note = updateFrameRequestDto.Note;
            frame.Label = updateFrameRequestDto.Label;
            frame.TimeFrom = TimeConverter.ConvertToInt(updateFrameRequestDto.TimeFrom);
            frame.TimeTo = TimeConverter.ConvertToInt(updateFrameRequestDto.TimeTo);
            frame.Status = updateFrameRequestDto.Status;
            frame.Price = updateFrameRequestDto.Price;
            frame.CourtId = updateFrameRequestDto.CourtId;
            unitOfWork.FrameRepository.Update(frame);
            await unitOfWork.CommitTransactionAsync();

            return new BusinessResult(200, "Update frame success");
        }
        catch (Exception ex)
        {
            unitOfWork.RollbackTransaction();
            return new BusinessResult(500, ex.Message);
        }
    }

    public async Task<IBusinessResult> RemoveFrame(int id)
    {
        await this.unitOfWork.BeginTransactionAsync();
        try
        {
            var frame = await unitOfWork.FrameRepository.GetByIdAsync(id);

            if (frame == null)
            {
                return new BusinessResult(-1, "No frame data");
            }

            frame.Status = (int)FrameStatus.Delete;
            unitOfWork.FrameRepository.Update(frame);
            await unitOfWork.CommitTransactionAsync();

            return new BusinessResult(200, "Delete Success");
        }
        catch (Exception ex)
        {
            this.unitOfWork.RollbackTransaction();
            return new BusinessResult(500, ex.Message);
        }
    }
    public List<Frame> GetAllFrameWithCourtId(int id)
    {
        var result = this.unitOfWork.FrameRepository.GetAllWithCourtId(id);
        return result;
    }
}