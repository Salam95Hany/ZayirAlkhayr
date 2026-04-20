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

  GetAllInventoryAdjustments(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'InventoryAdjustment/GetAllInventoryAdjustments', Model);
  }

  GetInventoryAdjustmentById(inventoryAdjustmentId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'InventoryAdjustment/GetInventoryAdjustmentById?inventoryAdjustmentId=' + inventoryAdjustmentId);
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
}
