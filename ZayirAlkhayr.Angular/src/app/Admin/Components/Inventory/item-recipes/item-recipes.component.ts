import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { InventoryService } from 'src/app/Admin/Services/inventory.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-item-recipes',
  templateUrl: './item-recipes.component.html',
  styleUrls: ['./item-recipes.component.css']
})
export class ItemRecipesComponent {
  UserModel: any;
  isFilter = false;
  showLoader = false;
  RecipeForm: FormGroup;
  Total = 0;
  LinkedItemsCount = 0;
  LinkedInventoryItemsCount = 0;
  ItemIdToDelete: number | null = null;
  EditingItemId: number | null = null;
  Results: any[] = [];
  Items: any[] = [];
  InventoryItems: any[] = [];
  FilterList: FilterModel[] = [
    {
      categoryName: 'SearchText',
      categoryDisplayName: 'وصفات الأصناف',
      filterType: 'SearchText',
      itemId: ''
    }
  ];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 10
  };
  ItemPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 1000
  };
  InventoryItemsPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 1000
  };

  constructor(
    private modalService: NgbModal,
    private inventoryService: InventoryService,
    private adminService: AdminService,
    private formService: ValidationFormService,
    private fb: FormBuilder,
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || 'null');
    this.FormInit();
    this.GetAllItemRecipes();
    this.GetItems();
    this.GetInventoryItems();
  }

  get RecipeLinesFormArray(): FormArray {
    return this.RecipeForm.get('recipes') as FormArray;
  }

  get IsEditMode(): boolean {
    return this.EditingItemId != null;
  }

  FormInit() {
    this.RecipeForm = this.fb.group({
      itemId: [null, [Validators.required]],
      userId: this.UserModel?.userId,
      recipes: this.fb.array([this.CreateRecipeLineForm()])
    });
  }

  CreateRecipeLineForm(item: any = null): FormGroup {
    return this.fb.group({
      itemRecipeId: [item?.itemRecipeId ?? null],
      inventoryItemId: [item?.inventoryItemId ?? null, [Validators.required]],
      quantityNeeded: [item?.quantityNeeded ?? 1, [Validators.required, Validators.min(0.0001)]]
    });
  }

  ResetForm() {
    this.EditingItemId = null;
    this.RecipeForm.reset();
    this.RecipeForm.setControl('recipes', this.fb.array([this.CreateRecipeLineForm()]));
    this.RecipeForm.patchValue({
      itemId: null,
      userId: this.UserModel?.userId
    });
  }

  FillEditForm(item: any) {
    this.EditingItemId = item.itemId;
    this.RecipeForm.patchValue({
      itemId: item.itemId,
      userId: this.UserModel?.userId
    });

    const recipesArray = this.fb.array(
      (item.recipes || []).map((recipe: any) => this.CreateRecipeLineForm(recipe))
    );

    this.RecipeForm.setControl('recipes', recipesArray.length > 0 ? recipesArray : this.fb.array([this.CreateRecipeLineForm()]));
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (!item) {
      this.modalService.open(content, {
        size: 'xl',
        scrollable: true,
        centered: true
      });
      return;
    }

    this.showLoader = true;
    this.inventoryService.GetItemRecipeById(item.itemId).subscribe({
      next: (data) => {
        if (data.isSuccess && data.results) {
          this.FillEditForm(data.results);
          this.modalService.open(content, {
            size: 'xl',
            scrollable: true,
            centered: true
          });
        } else {
          this.toaster.error(data.message || '');
        }
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل تفاصيل الوصفة');
      }
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.ItemIdToDelete = item.itemId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetItems() {
    this.adminService.GetAllProducts(this.ItemPagingFilter).subscribe({
      next: (data) => this.Items = data.results || [],
      error: () => this.Items = []
    });
  }

  GetInventoryItems() {
    this.inventoryService.GetAllInventoryItems(this.InventoryItemsPagingFilter).subscribe({
      next: (data) => this.InventoryItems = data.results || [],
      error: () => this.InventoryItems = []
    });
  }

  GetAllItemRecipes() {
    this.showLoader = true;
    this.inventoryService.GetAllItemRecipes(this.PagingFilter).subscribe({
      next: (data) => {
        this.Results = data.results || [];
        this.Total = data.totalCount || 0;
        this.LinkedItemsCount = this.Total;
        this.LinkedInventoryItemsCount = this.Results.reduce((sum, item) => sum + (+item.recipesCount || 0), 0);
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل وصفات الأصناف');
      }
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllItemRecipes();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllItemRecipes();
  }

  onItemClicked(item: any) {
    if (this.IsEditMode) {
      return;
    }

    this.RecipeForm.patchValue({ itemId: item.productId });
    this.RecipeForm.get('itemId')?.markAsTouched();
  }

  onInventoryItemClicked(index: number, item: any) {
    this.GetRecipeLineGroup(index).patchValue({ inventoryItemId: item.inventoryItemId });
    this.GetRecipeLineGroup(index).get('inventoryItemId')?.markAsTouched();
  }

  AddRecipeLine() {
    this.RecipeLinesFormArray.push(this.CreateRecipeLineForm());
  }

  RemoveRecipeLine(index: number) {
    if (this.RecipeLinesFormArray.length === 1) {
      this.RecipeLinesFormArray.at(0).reset({
        itemRecipeId: null,
        inventoryItemId: null,
        quantityNeeded: 1
      });
      return;
    }

    this.RecipeLinesFormArray.removeAt(index);
  }

  GetRecipeLineGroup(index: number): FormGroup {
    return this.RecipeLinesFormArray.at(index) as FormGroup;
  }

  GetItemName(id: number): string {
    return this.Items.find(i => i.productId === id)?.productName || 'اختر صنف القائمة';
  }

  GetInventoryItemName(id: number): string {
    return this.InventoryItems.find(i => i.inventoryItemId === id)?.name || 'اختر عنصر المخزون';
  }

  GetIngredientSummary(item: any): string {
    const recipes = item?.recipes || [];
    if (recipes.length === 0) {
      return '-';
    }

    return recipes
      .map((recipe: any) => `${recipe.inventoryItemName} (${recipe.quantityNeeded})`)
      .join('، ');
  }

  AddNewItem() {
    this.RecipeForm = this.formService.TrimFormInputValue(this.RecipeForm);
    this.RecipeForm.markAllAsTouched();
    this.RecipeLinesFormArray.controls.forEach(control => control.markAllAsTouched());

    if (!this.RecipeForm.valid || this.RecipeLinesFormArray.length === 0) {
      return;
    }

    const recipes = this.RecipeLinesFormArray.controls.map(control => ({
      itemRecipeId: control.get('itemRecipeId')?.value ? +control.get('itemRecipeId')?.value : null,
      inventoryItemId: +control.get('inventoryItemId')?.value,
      quantityNeeded: +control.get('quantityNeeded')?.value
    }));

    const hasInvalidLines = recipes.some(recipe => !recipe.inventoryItemId || recipe.quantityNeeded <= 0);
    if (hasInvalidLines) {
      this.toaster.warning('يرجى استكمال بيانات جميع مكونات الوصفة بشكل صحيح');
      return;
    }

    const hasDuplicates = recipes.some((recipe, index) =>
      recipes.findIndex(other => other.inventoryItemId === recipe.inventoryItemId) !== index
    );

    if (hasDuplicates) {
      this.toaster.warning('لا يمكن تكرار نفس عنصر المخزون داخل وصفة الصنف');
      return;
    }

    const payload = {
      itemId: +this.RecipeForm.value.itemId,
      userId: this.UserModel?.userId,
      recipes
    };

    this.showLoader = true;
    const request$ = this.IsEditMode
      ? this.inventoryService.UpdateItemRecipe(payload)
      : this.inventoryService.AddItemRecipe(payload);

    request$.subscribe(data => {
      this.HandleSaveResponse(data);
    });
  }

  DeleteItem() {
    if (!this.ItemIdToDelete) {
      return;
    }

    this.showLoader = true;
    this.inventoryService.DeleteItemRecipe(this.ItemIdToDelete).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message || '');
        this.GetAllItemRecipes();
        this.modalService.dismissAll();
      } else {
        this.toaster.error(data.message || '');
      }
      this.showLoader = false;
    });
  }

  private HandleSaveResponse(data: any) {
    if (data.isSuccess) {
      this.toaster.success(data.message || '');
      this.GetAllItemRecipes();
      this.modalService.dismissAll();
    } else {
      this.toaster.error(data.message || '');
    }

    this.showLoader = false;
  }
}
