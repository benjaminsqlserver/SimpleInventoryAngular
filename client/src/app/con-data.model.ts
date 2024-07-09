export interface Category {
  CategoryID: number;
  CategoryName: string;
}

export interface InventoryTransaction {
  TransactionID: number;
  ProductID: number;
  Quantity: number;
  TransactionDate: string;
  TransactionType: string;
}

export interface Product {
  ProductID: number;
  ProductName: string;
  SupplierID: number;
  CategoryID: number;
  QuantityPerUnit: string;
  UnitPrice: number;
  UnitsInStock: number;
  UnitsOnOrder: number;
  ReorderLevel: number;
  Discontinued: boolean;
}

export interface Supplier {
  SupplierID: number;
  SupplierName: string;
  ContactName: string;
  Address: string;
  City: string;
  PostalCode: string;
  Country: string;
  Phone: string;
}
