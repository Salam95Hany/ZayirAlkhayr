import { FilterModel } from "./FilterModel";

export interface PagingFilterModel {
    pagesize?: number;
    currentpage?: number;
    filterList: FilterModel[];
    supplierId?: number;
    userId?: string;
}