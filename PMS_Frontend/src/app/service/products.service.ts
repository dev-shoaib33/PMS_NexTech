import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Product } from '../model/Product.model';
import { dropdown } from '../model/dropdown.model'
import { GridResponseModel, ResponseModel } from '../model/Response.model';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseApiUrl: string = environment.baseApiUrl;
  version: string = 'v1';

  constructor(private http: HttpClient) {
    environment.baseApiUrl
  }

  getAllProducts(pageNumber: number, pageSize: number): Observable<GridResponseModel<Product>> {
    return this.http.get<GridResponseModel<Product>>(this.baseApiUrl + '/api/v1/Product?pageSize=' + pageSize + '&pageNumber=' + pageNumber);
  }

  getProductbyId(id: number): Observable<ResponseModel<Product>> {
    return this.http.get<ResponseModel<Product>>(`${this.baseApiUrl}/api/v1/Product/${id}`);
  }

  createProduct(product: Product): Observable<ResponseModel<number>> {
    return this.http.post<ResponseModel<number>>(this.baseApiUrl + "/api/v1/Product", product);
  }

  updateProduct(editProduct: Product): Observable<ResponseModel<number>> {
    return this.http.put<ResponseModel<number>>(`${this.baseApiUrl}/api/v1/Product`, editProduct);
  }

  deleteProduct(id: number): Observable<ResponseModel<boolean>> {
    return this.http.delete<ResponseModel<boolean>>(`${this.baseApiUrl}/api/v1/Product/${id}`);
  }

  getCategories(): Observable<GridResponseModel<dropdown>> {
    return this.http.get<GridResponseModel<dropdown>>(`${this.baseApiUrl}/api/v1/MasterData/GetProductCategories`);
  }

  getUoms(): Observable<GridResponseModel<dropdown>> {
    return this.http.get<GridResponseModel<dropdown>>(`${this.baseApiUrl}/api/v1/MasterData/GetUoms`);
  }
}
