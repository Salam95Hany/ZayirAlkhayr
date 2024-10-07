import { FamilyDetails, FamilyExpenses, FamilyExtraDetails, FamilyIncome, FamilyNeeds, FamilyPatient, FamilyStatus } from "./AddFamilyStatusModel";
import { FamilyStatusLookups } from "./FamilyStatusLookups";

export interface UpdateFamilyStatusLookups {
    lookups: FamilyStatusLookups;
    familyStatus: FamilyStatus;
    familyIncome: FamilyIncome;
    familyExpenses: FamilyExpenses;
    familyExtraDetails: FamilyExtraDetails;
    familyDetails: FamilyDetails[];
    familyPatient: FamilyPatient[];
    familyNeeds: FamilyNeeds[];
}