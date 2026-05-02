import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PagingFilterModel } from '../Models/PagingFilterModel';
import { ApiResponseModel } from '../Models/ApiResponseModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  apiURL = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // ============================= Purchase ==============================

  GetAllPurchases(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Purchase/GetAllPurchases', Model);
  }

  GetPurchaseById(purchaseId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Purchase/GetPurchaseById?purchaseId=' + purchaseId);
  }

  AddPurchase(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Purchase/AddPurchase', Model);
  }

  UpdatePurchase(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Purchase/UpdatePurchase', Model);
  }

  DeletePurchase(purchaseId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Purchase/DeletePurchase?purchaseId=' + purchaseId);
  }

  // ============================= InventoryAdjustment ==============================

  GetAllInventoryAdjustmentData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'InventoryAdjustment/GetAllInventoryAdjustmentData', Model);
  }

  GetAllInventoryAdjustmentFilters(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'InventoryAdjustment/GetAllInventoryAdjustmentFilters', Model);
  }

  GetInventoryAdjustmentById(inventoryAdjustmentId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'InventoryAdjustment/GetInventoryAdjustmentById?inventoryAdjustmentId=' + inventoryAdjustmentId);
  }

  GetAdjustmentDetailsById(inventoryAdjustmentId: number) {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'InventoryAdjustment/GetAdjustmentDetailsById?inventoryAdjustmentId=' + inventoryAdjustmentId);
  }

  AddInventoryAdjustment(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'InventoryAdjustment/AddInventoryAdjustment', Model);
  }

  UpdateInventoryAdjustment(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'InventoryAdjustment/UpdateInventoryAdjustment', Model);
  }

  DeleteInventoryAdjustment(inventoryAdjustmentId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'InventoryAdjustment/DeleteInventoryAdjustment?inventoryAdjustmentId=' + inventoryAdjustmentId);
  }

  // ============================= Unit ==============================

  GetAllUnits(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Unit/GetAllUnits', Model);
  }

  GetUnitById(unitId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Unit/GetUnitById?unitId=' + unitId);
  }

  AddNewUnit(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Unit/AddNewUnit', Model);
  }

  UpdateUnit(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Unit/UpdateUnit', Model);
  }

  DeleteUnit(unitId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Unit/DeleteUnit?unitId=' + unitId);
  }

  // ============================= InventoryItem ==============================

  GetAllInventoryItems(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'InventoryItem/GetAllInventoryItems', Model);
  }

  GetInventoryItemById(inventoryItemId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'InventoryItem/GetInventoryItemById?inventoryItemId=' + inventoryItemId);
  }

  GetLowStockInventoryItems() {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'InventoryItem/GetLowStockInventoryItems');
  }

  AddNewInventoryItem(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'InventoryItem/AddNewInventoryItem', Model);
  }

  UpdateInventoryItem(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'InventoryItem/UpdateInventoryItem', Model);
  }

  DeleteInventoryItem(inventoryItemId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'InventoryItem/DeleteInventoryItem?inventoryItemId=' + inventoryItemId);
  }

  // ============================= ItemRecipe ==============================

  GetAllItemRecipes(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemRecipe/GetAllItemRecipes', Model);
  }

  GetItemRecipeById(itemId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'ItemRecipe/GetItemRecipeById?itemId=' + itemId);
  }

  AddItemRecipe(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'ItemRecipe/AddItemRecipe', Model);
  }

  UpdateItemRecipe(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'ItemRecipe/UpdateItemRecipe', Model);
  }

  DeleteItemRecipe(itemId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'ItemRecipe/DeleteItemRecipe?itemId=' + itemId);
  }

  // ============================= Supplier ==============================

  GetAllSuppliers(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Supplier/GetAllSuppliers', Model);
  }

  GetSupplierById(supplierId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Supplier/GetSupplierById?supplierId=' + supplierId);
  }

  AddNewSupplier(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Supplier/AddNewSupplier', Model);
  }

  UpdateSupplier(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Supplier/UpdateSupplier', Model);
  }

  DeleteSupplier(supplierId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Supplier/DeleteSupplier?supplierId=' + supplierId);
  }

  // ============================= SupplierPayment ==============================

  GetAllSupplierInvicesData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SupplierPayment/GetAllSupplierInvicesData', Model);
  }

  GetAllSupplierInvicesFilters(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SupplierPayment/GetAllSupplierInvicesFilters', Model);
  }

  GetSupplierPaymentLogData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'SupplierPayment/GetSupplierPaymentLogData', Model);
  }

  GetSupplierNameById(SupplierId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'SupplierPayment/GetSupplierNameById?SupplierId=' + SupplierId);
  }

  GetPurchaseNumberBySupplierId(SupplierId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'SupplierPayment/GetPurchaseNumberBySupplierId?SupplierId=' + SupplierId);
  }

  AddNewSupplierPayment(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'SupplierPayment/AddNewSupplierPayment', Model);
  }

  UpdateSupplierPayment(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'SupplierPayment/UpdateSupplierPayment', Model);
  }

  DeleteSupplierPayment(SupplierPaymentId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'SupplierPayment/DeleteSupplierPayment?SupplierPaymentId=' + SupplierPaymentId);
  }
}
