export interface Product {
    productId: number,
    productName: string,
    productCode: string,
    description: string,
    uom: string,
    price: number
    categoryLkpId: number;
    categoryLkpName: string;
}
