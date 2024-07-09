import { Injectable } from '@angular/core';
import { Location } from '@angular/common';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';

import { ConfigService } from './config.service';
import { ODataClient } from './odata-client';
import * as models from './con-data.model';

@Injectable()
export class ConDataService {
  odata: ODataClient;
  basePath: string;

  constructor(private http: HttpClient, private config: ConfigService) {
    this.basePath = config.get('conData');
    this.odata = new ODataClient(this.http, this.basePath, { legacy: false, withCredentials: true });
  }

  getCategories(filter: string | null, top: number | null, skip: number | null, orderby: string | null, count: boolean | null, expand: string | null, format: string | null, select: string | null) : Observable<any> {
    return this.odata.get(`/Categories`, { filter, top, skip, orderby, count, expand, format, select });
  }

  createCategory(expand: string | null, category: models.Category | null) : Observable<any> {
    return this.odata.post(`/Categories`, category, { expand }, []);
  }

  deleteCategory(categoryId: number | null) : Observable<any> {
    return this.odata.delete(`/Categories(${categoryId})`, item => !(item.CategoryID == categoryId));
  }

  getCategoryByCategoryId(expand: string | null, categoryId: number | null) : Observable<any> {
    return this.odata.getById(`/Categories(${categoryId})`, { expand });
  }

  updateCategory(expand: string | null, categoryId: number | null, category: models.Category | null) : Observable<any> {
    return this.odata.patch(`/Categories(${categoryId})`, category, item => item.CategoryID == categoryId, { expand }, []);
  }

  getInventoryTransactions(filter: string | null, top: number | null, skip: number | null, orderby: string | null, count: boolean | null, expand: string | null, format: string | null, select: string | null) : Observable<any> {
    return this.odata.get(`/InventoryTransactions`, { filter, top, skip, orderby, count, expand, format, select });
  }

  createInventoryTransaction(expand: string | null, inventoryTransaction: models.InventoryTransaction | null) : Observable<any> {
    return this.odata.post(`/InventoryTransactions`, inventoryTransaction, { expand }, ['Product']);
  }

  deleteInventoryTransaction(transactionId: number | null) : Observable<any> {
    return this.odata.delete(`/InventoryTransactions(${transactionId})`, item => !(item.TransactionID == transactionId));
  }

  getInventoryTransactionByTransactionId(expand: string | null, transactionId: number | null) : Observable<any> {
    return this.odata.getById(`/InventoryTransactions(${transactionId})`, { expand });
  }

  updateInventoryTransaction(expand: string | null, transactionId: number | null, inventoryTransaction: models.InventoryTransaction | null) : Observable<any> {
    return this.odata.patch(`/InventoryTransactions(${transactionId})`, inventoryTransaction, item => item.TransactionID == transactionId, { expand }, ['Product']);
  }

  getProducts(filter: string | null, top: number | null, skip: number | null, orderby: string | null, count: boolean | null, expand: string | null, format: string | null, select: string | null) : Observable<any> {
    return this.odata.get(`/Products`, { filter, top, skip, orderby, count, expand, format, select });
  }

  createProduct(expand: string | null, product: models.Product | null) : Observable<any> {
    return this.odata.post(`/Products`, product, { expand }, ['Supplier', 'Category']);
  }

  deleteProduct(productId: number | null) : Observable<any> {
    return this.odata.delete(`/Products(${productId})`, item => !(item.ProductID == productId));
  }

  getProductByProductId(expand: string | null, productId: number | null) : Observable<any> {
    return this.odata.getById(`/Products(${productId})`, { expand });
  }

  updateProduct(expand: string | null, productId: number | null, product: models.Product | null) : Observable<any> {
    return this.odata.patch(`/Products(${productId})`, product, item => item.ProductID == productId, { expand }, ['Supplier','Category']);
  }

  getSuppliers(filter: string | null, top: number | null, skip: number | null, orderby: string | null, count: boolean | null, expand: string | null, format: string | null, select: string | null) : Observable<any> {
    return this.odata.get(`/Suppliers`, { filter, top, skip, orderby, count, expand, format, select });
  }

  createSupplier(expand: string | null, supplier: models.Supplier | null) : Observable<any> {
    return this.odata.post(`/Suppliers`, supplier, { expand }, []);
  }

  deleteSupplier(supplierId: number | null) : Observable<any> {
    return this.odata.delete(`/Suppliers(${supplierId})`, item => !(item.SupplierID == supplierId));
  }

  getSupplierBySupplierId(expand: string | null, supplierId: number | null) : Observable<any> {
    return this.odata.getById(`/Suppliers(${supplierId})`, { expand });
  }

  updateSupplier(expand: string | null, supplierId: number | null, supplier: models.Supplier | null) : Observable<any> {
    return this.odata.patch(`/Suppliers(${supplierId})`, supplier, item => item.SupplierID == supplierId, { expand }, []);
  }
}
