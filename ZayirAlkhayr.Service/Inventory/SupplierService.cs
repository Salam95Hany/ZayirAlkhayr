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
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<Supplier>>> GetAllSuppliers(PagingFilterModel model)
        {
            var searchText = GetFilterValue(model, "SearchText");

            var query = _unitOfWork.Repository<Supplier>()
                                   .GetAllAsQueryable()
                                   .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(i => i.Name.Contains(searchText)
                                      || i.Phone.Contains(searchText)
                                      || i.Address.Contains(searchText));

            query = query.OrderByDescending(i => i.SupplierId);
            var totalCount = await query.CountAsync();
            var results = await ApplyPaging(query, model).ToListAsync();

            return ApiResponseModel<List<Supplier>>.Success(GenericErrors.GetSuccess, results, totalCount);
        }

        public async Task<ApiResponseModel<Supplier>> GetSupplierById(int supplierId)
        {
            var entity = await _unitOfWork.Repository<Supplier>()
                                          .GetAllAsQueryable()
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(i => i.SupplierId == supplierId);

            if (entity == null)
                return ApiResponseModel<Supplier>.Failure(GenericErrors.NotFound);

            return ApiResponseModel<Supplier>.Success(GenericErrors.GetSuccess, entity);
        }

        public async Task<ApiResponseModel<string>> AddNewSupplier(Supplier model)
        {
            try
            {
                model.InsertDate = DateTime.Now;
                await _unitOfWork.Repository<Supplier>().AddAsync(model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess, model.SupplierId.ToString());
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateSupplier(Supplier model)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Supplier>().GetByIdAsync(model.SupplierId);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                entity.Name = model.Name;
                entity.Phone = model.Phone;
                entity.Address = model.Address;
                entity.UpdateUser = GetActionUser(model.UpdateUser, model.InsertUser);
                entity.UpdateDate = DateTime.Now;

                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess, entity.SupplierId.ToString());
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> DeleteSupplier(int supplierId)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Supplier>().GetByIdAsync(supplierId);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var hasPurchases = await _unitOfWork.Repository<Purchase>()
                                                    .AnyAsync(i => i.SupplierId == supplierId);
                if (hasPurchases)
                    return ApiResponseModel<string>.Failure(GenericErrors.DeleteRelationRow);

                _unitOfWork.Repository<Supplier>().Delete(entity);
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
