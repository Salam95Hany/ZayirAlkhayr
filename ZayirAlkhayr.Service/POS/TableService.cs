using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.POS;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.POS
{
    public class TableService : ITableService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<Table>>> GetAllTable()
        {
            var results = await _unitOfWork.Repository<Table>().GetAllAsync();
            return ApiResponseModel<List<Table>>.Success(GenericErrors.GetSuccess, results);
        }

        public async Task<ApiResponseModel<string>> AddNewTable(Table Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<Table>().AnyAsync(i => i.TableName == Model.TableName);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                Model.InsertDate = DateTime.UtcNow.ToQatarTime();
                await _unitOfWork.Repository<Table>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateTable(Table Model)
        {
            try
            {
                var CheckExist = await _unitOfWork.Repository<Table>().AnyAsync(i => i.TableName == Model.TableName && i.TableId != Model.TableId);
                if (CheckExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                var Entity = await _unitOfWork.Repository<Table>().GetByIdAsync(Model.TableId);
                if (Entity != null)
                {
                    Entity.TableName = Model.TableName;
                    Entity.UpdateUser = Model.InsertUser;
                    Entity.UpdateDate = DateTime.UtcNow.ToQatarTime();
                    await _unitOfWork.CompleteAsync();
                    return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            }
            catch
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ApiResponseModel<string>> DeleteTable(int TableId)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<Table>().GetByIdAsync(TableId);
                if (Entity != null)
                {
                    _unitOfWork.Repository<Table>().Delete(Entity);
                    await _unitOfWork.CompleteAsync();
                    return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            }
            catch
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
