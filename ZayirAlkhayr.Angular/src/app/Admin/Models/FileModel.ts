
export interface UploadFileModel {
    id?: number;
    files?: File[];
    deletedFiles?: DeletedFileModel[];
}

export interface DeletedFileModel {
    id: number;
    fileName: string;
}