export interface ReceiptModel {
    orderNo: number;
    orderType: string;
    date: string;
    cashier: string;
    agent?: string;
    items: ReceiptItemsModel[];
    deliveryFee?: number;
    grandTotal: number;
}

export interface ReceiptItemsModel {
    nameAr: string;
    nameEn: string;
    qty: number;
    price?: number;
    total?: number;
    categoryId: number;
}
