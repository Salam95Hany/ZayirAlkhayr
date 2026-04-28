using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Inventory;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Inventory
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<Unit>>> GetAllUnits(PagingFilterModel model)
        {
            var searchText = GetFilterValue(model, "SearchText");

            var query = _unitOfWork.Repository<Unit>()
                                   .GetAllAsQueryable()
                                   .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(i => i.Name.Contains(searchText));

            query = query.OrderByDescending(i => i.UnitId);
            var totalCount = await query.CountAsync();
            var results = await ApplyPaging(query, model).ToListAsync();

            return ApiResponseModel<List<Unit>>.Success(GenericErrors.GetSuccess, results, totalCount);
        }

        public async Task<ApiResponseModel<Unit>> GetUnitById(int unitId)
        {
            var entity = await _unitOfWork.Repository<Unit>()
                                          .GetAllAsQueryable()
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(i => i.UnitId == unitId);

            if (entity == null)
                return ApiResponseModel<Unit>.Failure(GenericErrors.NotFound);

            return ApiResponseModel<Unit>.Success(GenericErrors.GetSuccess, entity);
        }

        public async Task<ApiResponseModel<string>> AddNewUnit(Unit model)
        {
            try
            {
                model.InsertDate = DateTime.Now;
                await _unitOfWork.Repository<Unit>().AddAsync(model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess, model.UnitId.ToString());
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateUnit(Unit model)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Unit>().GetByIdAsync(model.UnitId);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                entity.Name = model.Name;
                entity.UpdateUser = GetActionUser(model.UpdateUser, model.InsertUser);
                entity.UpdateDate = DateTime.Now;

                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess, entity.UnitId.ToString());
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> DeleteUnit(int unitId)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Unit>().GetByIdAsync(unitId);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var hasInventoryItems = await _unitOfWork.Repository<InventoryItem>()
                                                         .AnyAsync(i => i.UnitId == unitId);
                if (hasInventoryItems)
                    return ApiResponseModel<string>.Failure(GenericErrors.DeleteRelationRow);

                _unitOfWork.Repository<Unit>().Delete(entity);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Message.Contains("REFERENCE constraint"))
                    return ApiResponseModel<string>.Failure(GenericErrors.DeleteRelationRow);

                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        private static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, PagingFilterModel model)
        {
            var currentPage = model?.Currentpage > 0 ? model.Currentpage : 1;
            var pageSize = model?.Pagesize > 0 ? model.Pagesize : 20;
            return query.Skip((currentPage - 1) * pageSize).Take(pageSize);
        }

        private static string GetFilterValue(PagingFilterModel model, string categoryName)
        {
            var filter = model?.FilterList?.FirstOrDefault(i => i.CategoryName == categoryName);
            return filter?.ItemId ?? filter?.ItemValue ?? string.Empty;
        }

        private static string GetActionUser(string updateUser, string insertUser)
        {
            return !string.IsNullOrWhiteSpace(updateUser) ? updateUser : insertUser;
        }
    }
}
