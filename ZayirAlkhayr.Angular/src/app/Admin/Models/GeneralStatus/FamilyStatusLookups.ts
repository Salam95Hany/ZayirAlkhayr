import { FamilyNeeds } from "./AddFamilyStatusModel";

export interface FamilyStatusLookups {
    categories: FamilyCategories[];
    nationalities: FamilyNationalities[];
    familyNeeds: FamilyNeedCategoryGroups[];
}

export interface FamilyNeedCategoryGroups {
    categoryName: string;
    needs: FamilyNeedTypes[];
    selectedNeeds: FamilyNeeds[];
}

export interface FamilyCategories {
    id: number;
    name: string;
    insertUser: string;
    insertDate: string | null;
    updateUser: string;
    updateDate: string | null;
}

export interface FamilyNationalities {
    id: number;
    name: string;
    insertUser: string;
    insertDate: string | null;
    updateUser: string;
    updateDate: string | null;
}

export interface FamilyNeedTypes {
    id: number;
    categoryId: number;
    name: string;
    insertUser: string;
    insertDate: string | null;
    updateUser: string;
    updateDate: string | null;
}