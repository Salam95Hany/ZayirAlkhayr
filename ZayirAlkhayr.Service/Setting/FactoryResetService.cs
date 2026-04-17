using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Setting;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Setting;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Setting
{
    public class FactoryResetService : IFactoryResetService
    {
        private static readonly Error InvalidTarget = new("المرحلة المطلوبة غير معروفة.");
        private static readonly Error MissingTarget = new("الرجاء تحديد المرحلة المطلوب حذفها.");

        private readonly POSDbContext _dbContext;

        private static readonly IReadOnlyList<FactoryResetDefinition> Definitions = new List<FactoryResetDefinition>
        {
            new FactoryResetDefinition("inventory-adjustments", "حركات المخزون", "حذف كل قيود جدول حركات المخزون.", 1, "Inv", "InventoryAdjustments"),
            new FactoryResetDefinition("order-details", "تفاصيل الطلبات", "حذف كل بيانات جدول تفاصيل الطلبات قبل حذف الطلبات الأساسية.", 2, "POS", "OrderDetails"),
            new FactoryResetDefinition("orders", "الطلبات", "حذف كل بيانات جدول الطلبات بعد تنظيف تفاصيل الطلبات.", 3, "POS", "Orders"),
            new FactoryResetDefinition("purchase-items", "عناصر المشتريات", "حذف كل بيانات جدول عناصر المشتريات قبل حذف سندات المشتريات.", 4, "Inv", "PurchaseItems"),
            new FactoryResetDefinition("purchases", "المشتريات", "حذف كل بيانات جدول المشتريات بعد حذف عناصر المشتريات.", 5, "Inv", "Purchases")
        };

        public FactoryResetService(POSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponseModel<List<FactoryResetPreviewDto>>> GetFactoryResetPreviewAsync()
        {
            var preview = new List<FactoryResetPreviewDto>();

            foreach (var definition in Definitions.OrderBy(i => i.ExecutionOrder))
            {
                preview.Add(new FactoryResetPreviewDto
                {
                    Key = definition.Key,
                    DisplayName = definition.DisplayName,
                    Description = definition.Description,
                    ExecutionOrder = definition.ExecutionOrder,
                    RecordsCount = await GetRecordsCountAsync(definition.Key)
                });
            }

            return ApiResponseModel<List<FactoryResetPreviewDto>>.Success(GenericErrors.GetSuccess, preview, preview.Count);
        }

        public async Task<ApiResponseModel<FactoryResetStepResultDto>> DeleteTargetDataAsync(string targetKey)
        {
            if (string.IsNullOrWhiteSpace(targetKey))
                return ApiResponseModel<FactoryResetStepResultDto>.Failure(MissingTarget);

            var definition = Definitions.FirstOrDefault(i => string.Equals(i.Key, targetKey.Trim(), StringComparison.OrdinalIgnoreCase));
            if (definition == null)
                return ApiResponseModel<FactoryResetStepResultDto>.Failure(InvalidTarget);

            try
            {
                var recordsCountBeforeDelete = await GetRecordsCountAsync(definition.Key);

                await using var transaction = await _dbContext.Database.BeginTransactionAsync();

                var deletedRecordsCount = await _dbContext.Database.ExecuteSqlRawAsync(
                    $"DELETE FROM [{definition.SchemaName}].[{definition.TableName}]");

                await transaction.CommitAsync();

                if (deletedRecordsCount < 0)
                    deletedRecordsCount = recordsCountBeforeDelete;

                var result = new FactoryResetStepResultDto
                {
                    Key = definition.Key,
                    DisplayName = definition.DisplayName,
                    DeletedRecordsCount = deletedRecordsCount,
                    RemainingRecordsCount = await GetRecordsCountAsync(definition.Key),
                    ExecutionOrder = definition.ExecutionOrder,
                    StatusMessage = $"تم حذف {deletedRecordsCount} سجل من {definition.DisplayName} بنجاح."
                };

                return ApiResponseModel<FactoryResetStepResultDto>.Success(new Error(result.StatusMessage), result);
            }
            catch
            {
                return ApiResponseModel<FactoryResetStepResultDto>.Failure(GenericErrors.TransFailed);
            }
        }

        private async Task<int> GetRecordsCountAsync(string targetKey)
        {
            return targetKey switch
            {
                "inventory-adjustments" => await _dbContext.InventoryAdjustments.CountAsync(),
                "order-details" => await _dbContext.OrderDetails.CountAsync(),
                "orders" => await _dbContext.Orders.CountAsync(),
                "purchase-items" => await _dbContext.PurchaseItems.CountAsync(),
                "purchases" => await _dbContext.Purchases.CountAsync(),
                _ => 0
            };
        }

        private sealed class FactoryResetDefinition
        {
            public FactoryResetDefinition(string key, string displayName, string description, int executionOrder, string schemaName, string tableName)
            {
                Key = key;
                DisplayName = displayName;
                Description = description;
                ExecutionOrder = executionOrder;
                SchemaName = schemaName;
                TableName = tableName;
            }

            public string Key { get; }
            public string DisplayName { get; }
            public string Description { get; }
            public int ExecutionOrder { get; }
            public string SchemaName { get; }
            public string TableName { get; }
        }
    }
}
