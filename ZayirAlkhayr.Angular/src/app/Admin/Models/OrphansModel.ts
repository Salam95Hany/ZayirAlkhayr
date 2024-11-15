export interface Orphans {
    id: number;
    familyStatusId: number;
    benefactorId: number | null;
    familyDetailsId: number;
    name: string;
    dateOfBirth: string;
    familyName: string;
    jop: string;
    nationalId: string;
    phone: string;
    address: string;
    income: string;
    academicStage: string;
    familyMembersCount: string;
    rankingBrothers: string;
    healthStatus: string;
    nationality: string;
    notes: string;
    benefactorPhone: string;
    benefactorAddress: string;
    benefactorType: string;
    isGuaranteed: boolean;
    insertUser: string;
    insertDate: string | null;
    updateUser: string;
    updateDate: string | null;
}


export interface BeneFactorOrphansModel {
    orphansId: number;
    benefactorId: number;
    benefactorPhone: string;
    benefactorAddress: string;
    benefactorType: string;
    isGuaranteed: boolean;
}

export interface OrphansDetailsModel {
    familyStatusId: number;
    familyStatusName: string;
    orphansDetails: OrphansDetails[];
}

export interface OrphansDetails {
    familyDetailsId: number;
    familyDetailsName: string;
}