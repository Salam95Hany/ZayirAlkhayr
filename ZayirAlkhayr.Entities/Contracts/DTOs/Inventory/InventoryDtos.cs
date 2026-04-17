using System;
using System.Collections.Generic;

namespace ZayirAlkhayr.Entities.Contracts.DTOs.Inventory
{
    public class InventoryAdjustmentDetailsDto
    {
        public int InventoryAdjustmentId { get; set; }
        public int InventoryItemId { get; set; }
        public string InventoryItemName { get; set; }
        public int QuantityChange { get; set; }
        public string Reason { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class PurchaseUpsertDto
    {
        public int? PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public string UserId { get; set; }
        public List<PurchaseItemUpsertDto> Items { get; set; } = new List<PurchaseItemUpsertDto>();
    }

    public class PurchaseItemUpsertDto
    {
        public int InventoryItemId { get; set; }
        public int Quantity { get; set; }
        public double CostPrice { get; set; }
    }

    public class PurchaseSummaryDto
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public double TotalAmount { get; set; }
        public int ItemsCount { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class PurchaseDetailsDto
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public double TotalAmount { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<PurchaseItemDetailsDto> Items { get; set; } = new List<PurchaseItemDetailsDto>();
    }

    public class PurchaseItemDetailsDto
    {
        public int PurchaseItemId { get; set; }
        public int PurchaseId { get; set; }
        public int InventoryItemId { get; set; }
        public string InventoryItemName { get; set; }
        public int Quantity { get; set; }
        public double CostPrice { get; set; }
        public double Total { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
