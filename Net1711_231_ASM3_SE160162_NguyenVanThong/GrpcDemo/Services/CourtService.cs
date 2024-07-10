using BadmintonReservationBusiness;
using BadmintonReservationData;
using BadmintonReservationData.DTO;
using BadmintonReservationWebApp.Models;
using Grpc.Core;
using GrpcDemo.Protos;
using System;
using System.Collections.Generic;

namespace GrpcDemo.Services
{
    public class CourtService : CourtCrud.CourtCrudBase
    {
        private readonly CourtBusiness _business;

        public CourtService(CourtBusiness business)
        {
            _business = business;
        }

        public override async Task<ReadCourtResponseList> ReadCourtList(ReadCourtRequestList request, ServerCallContext context)
        {
            var list = await this._business.GetAll();
            var response = new ReadCourtResponseList();
            foreach (var court in (List<Court>)list.Data)
            {
                response.Cours.Add(new ReadCourtResponse
                {
                    Id = court.Id,
                    Name = court.Name,
                    SurfaceType = court.SurfaceType,
                    Status = court.Status,
                    TotalBooking = court.TotalBooking,
                    OpeningHours = court.OpeningHours,
                    CloseHours = court.CloseHours,
                    Amentities = court.Amentities,
                    Capacity = court.Capacity,
                    CourtType = court.CourtType,

                });
            }
            return await Task.FromResult(response);
        }


        public override async Task<ReadCourtResponse> ReadCourt(ReadCourtRequest request, ServerCallContext context)
        {
            var business = await this._business.GetById(request.Id);
            var court = (Court)business.Data;
            var response = new ReadCourtResponse()
            {
                Id = court.Id,
                Name = court.Name,
                SurfaceType = court.SurfaceType,
                Status = court.Status,
                TotalBooking = court.TotalBooking,
                OpeningHours = court.OpeningHours,
                CloseHours = court.CloseHours,
                Amentities = court.Amentities,
                Capacity = court.Capacity,
                CourtType = court.CourtType,
            };
            return await Task.FromResult<ReadCourtResponse>(response);
        }

        public override async Task<UpdateCourtResponse> UpdateCourt(UpdateCourtRequest request, ServerCallContext context)
        {
            var updateModel = new UpdateCourtDTO
            {
                Id = request.Id,
                Name = request.Name,
                SurfaceType = request.SurfaceType,
                Status = request.Status,
                OpeningHours = request.OpeningHours,
                CloseHours = request.CloseHours,
                Amentities = request.Amentities,
                Capacity = request.Capacity,
                CourtType = request.CourtType,
            };
            var result = await this._business.UpdateCourt(updateModel);
            var response = new UpdateCourtResponse
            {
                Status = result.Status,
                Message = result.Message
            };

            return await Task.FromResult(response);
        }

        public override async Task<CreateCourtResponse> CreateCourt(CreateCourtRequest request, ServerCallContext context)
        {
            var court = new CreateCourtDTO()
            {
                Name = request.Name,
                SurfaceType = request.SurfaceType,
                Status = request.Status,
                OpeningHours = request.OpeningHours,
                CloseHours = request.CloseHours,
                Amentities = request.Amentities,
                Capacity = request.Capacity,
                CourtType = request.CourtType,
            };
            var result = await this._business.CreateCourt(court);
            var response = new CreateCourtResponse { Status = result.Status, Message = result.Message };
            return await Task.FromResult(response);
        }

        public override async Task<DeleteCourtResponse> DeleteCourt(DeleteCourtRequest request, ServerCallContext context)
        {
            var reusult = await this._business.RemoveCourt(request.Id);
            var response = new DeleteCourtResponse { Status = reusult.Status, Message = reusult.Message };
            return await Task.FromResult(response);
        }

        public override async Task<ReadCourtListJsonResponse> ReadCourtListJson(ReadCourtListJsonRequest request, ServerCallContext context)
        {
            var filter = new CourtSearchConditionDTO
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SearchText = request.SearchText,
                SearchMode = request.SearchMode,
                StatusFilter = request.StatusFilter,
                SurfaceTypeFilter = request.SurfaceTypeFilter,
                CourtType = request.CourtType,
                OpeningHours = request.OpeningHours,
                CloseHours = request.CloseHours,
            };

            var business = await this._business.GetAllWithCondition(filter);
            var response = new ReadCourtListJsonResponse();
            PageableResponseDTO<Court> cast = (PageableResponseDTO<Court>)business.Data;
            foreach (var court in cast.List)
            {
                response.Cours.Add(new ReadCourtResponse
                {
                    Id = court.Id,
                    Name = court.Name,
                    SurfaceType = court.SurfaceType,
                    Status = court.Status,
                    TotalBooking = court.TotalBooking,
                    OpeningHours = court.OpeningHours,
                    CloseHours = court.CloseHours,
                    Amentities = court.Amentities,
                    Capacity = court.Capacity,
                    CourtType = court.CourtType,

                });
            }
            return await Task.FromResult(response);
        }
    }
}
