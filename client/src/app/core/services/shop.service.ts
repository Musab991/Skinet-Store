import { Injectable, inject } from '@angular/core';
import { HttpClientModule, HttpClient, HttpParams } from '@angular/common/http';
import { Product } from '../../models/product';
import { Pagination } from '../../models/pagination';
import { ShopParams } from '../../models/shopParams';
@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  productController: string = 'products';
  types: string[] = [];
  brands: string[] = [];
  totalProducts: Number | undefined;
  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();
    console.log('ShopParams;\t', shopParams);
    if (shopParams.brands.length > 0) {
      params = params.append('brands', shopParams.brands.join(','));
    }
    if (shopParams.types.length > 0) {
      params = params.append('types', shopParams.types.join(','));
    }
    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }
    if (shopParams.pageSize && shopParams.pageIndex) {
      params = params.append('pageSize', shopParams.pageSize);
      params = params.append('pageIndex', shopParams.pageIndex);
    }
    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }
    console.log(this.baseUrl + 'products' + params);
    return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {
      params,
    });
  }

  getProductById(id: number) {
    return this.http.get<Product>(this.baseUrl + 'products' + `/${id}`);
  }
  getBrands() {
    if (this.brands.length > 0) {
      return;
    }
    this.http
      .get<string[]>(this.baseUrl + this.productController + '/brands')
      .subscribe({
        next: (response: any) => (this.brands = response),
      });
  }
  getTypes() {
    if (this.types.length > 0) {
      return;
    }
    if (this.types.length === 0) {
      this.http
        .get<string[]>(this.baseUrl + this.productController + '/types')
        .subscribe({
          next: (response: any) => (this.types = response),
        });
    }
  }
}
