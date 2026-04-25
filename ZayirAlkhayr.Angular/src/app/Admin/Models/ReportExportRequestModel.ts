import { FilterModel } from './FilterModel';

export interface ReportExportRequestModel {
  reportType: string;
  userName?: string | null;
  outputFormat?: string | null;
  culture?: string | null;
  dateFormat?: string | null;
  fileNamePrefix?: string | null;
  queryString: ReportQueryStringModel[];
  filterList: FilterModel[];
}

export interface ReportQueryStringModel {
  key: string;
  value: string;
}
