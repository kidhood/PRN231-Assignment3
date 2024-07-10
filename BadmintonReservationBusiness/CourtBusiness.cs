using BadmintonReservationData;
using BadmintonReservationData.DTO;
using BadmintonReservationData.DTOs;
using BadmintonReservationData.Enums;
using BadmintonReservationData.Repository;
using BadmintonReservationWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationBusiness
{
    public class CourtBusiness
    {
        private readonly UnitOfWork unitOfWork;

        public CourtBusiness(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                var courts = await this.unitOfWork.CourtRepository.GetAllAsync();

                if (courts == null)
                {
                    return new BusinessResult(400, "No court data");
                }
                else
                {
                    return new BusinessResult(200, "Get court list sucess", courts);
                }
            }
            catch (Exception ex) {
                return new BusinessResult(500, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAllWithCondition(CourtSearchConditionDTO condition)
        {
            try
            {
                var paging = await this.unitOfWork.CourtRepository.GetAllWithCondition(condition);


                if (paging.Courts == null)
                {
                    return new BusinessResult(400, "No court data");
                }
                else
                {
                    PageableResponseDTO<Court> result = new PageableResponseDTO<Court>
                    {
                        List = paging.Courts,
                        PageIndex = condition.PageIndex,
                        PageSize = condition.PageSize,
                        TotalOfPages = paging.TotalItems
                    };
                    return new BusinessResult(200, "Get court list sucess", result);
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
                var court = this.unitOfWork.CourtRepository.GetById(id);

                if (court == null)
                {
                    return new BusinessResult(404, "No court data");
                }
                else
                {
                    return new BusinessResult(200, "Get court sucess", court);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> CreateCourt(CreateCourtDTO courtRequest)
        { 
            await this.unitOfWork.BeginTransactionAsync();
            try
            {
                var court = new Court
                {
                    Name = courtRequest.Name,
                    Status = courtRequest.Status,
                    CloseHours = courtRequest.CloseHours,
                    OpeningHours = courtRequest.OpeningHours,
                    Capacity = courtRequest.Capacity,
                    SurfaceType = courtRequest.SurfaceType,
                    TotalBooking = "0",
                    Amentities = courtRequest.Amentities,
                    CourtType = courtRequest.CourtType,
                };

                await this.unitOfWork.CourtRepository.CreateAsync(court);
                await this.unitOfWork.CommitTransactionAsync();
                return new BusinessResult(200, "Create Success");     
            }
            catch (Exception ex)
            {
                this.unitOfWork.RollbackTransaction();
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateCourt(UpdateCourtDTO courtRequest)
        {
            await this.unitOfWork.BeginTransactionAsync();
            try
            {
                var court = this.unitOfWork.CourtRepository.GetById(courtRequest.Id);

                if (court == null)
                {
                    return new BusinessResult(-1, "No court data");
                }

                court.Name = courtRequest.Name;
                court.Status = courtRequest.Status;
                court.Amentities = courtRequest.Amentities;
                court.Capacity = courtRequest.Capacity;
                court.CourtType = courtRequest.CourtType;
                court.SurfaceType = courtRequest.SurfaceType;
                court.OpeningHours = courtRequest.OpeningHours;
                court.CloseHours = courtRequest.CloseHours;

                this.unitOfWork.CourtRepository.Update(court);
                await this.unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Update court success");
            }
            catch (Exception ex)
            {
                this.unitOfWork.RollbackTransaction();
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> RemoveCourt(int id)
        {
            await this.unitOfWork.BeginTransactionAsync();
            try
            {
                var court = this.unitOfWork.CourtRepository.GetById(id);

                if (court == null)
                {
                    return new BusinessResult(-1, "No court data");
                }

                court.Status = (int)CourtStatus.Delete;

                List<Frame> listFrame = this.unitOfWork.FrameRepository.GetListByCourtId(court.Id);
                if (listFrame != null && listFrame.Count > 0)
                {
                    foreach (Frame f in listFrame)
                    {
                        f.Status = (int)FrameStatus.Delete;
                    }
                }

                this.unitOfWork.CourtRepository.Update(court);
                this.unitOfWork.FrameRepository.UpdateRange(listFrame);
                await this.unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Delete Success");
            }
            catch (Exception ex)
            {
                this.unitOfWork.RollbackTransaction();
                return new BusinessResult(-4, ex.Message);
            }
        }
    } 
}