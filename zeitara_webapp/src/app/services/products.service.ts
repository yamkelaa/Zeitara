import { Observable } from "rxjs";
import { FashionProductSearchRequestDto, Product } from "../shared/models/products/products";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";

// product.service.ts
@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = `${environment.apiUrl}/FashionProduct`; // Single /api

  constructor(private http: HttpClient) { }

  getProductsChunk(pageSize: number, lastId = 0): Observable<Product[]> {
    const params = new HttpParams()
      .set('pageSize', pageSize.toString())
      .set('lastId', lastId.toString());

    return this.http.get<Product[]>(this.apiUrl, { params });
  }

  searchProducts(query: string): Observable<Product[]> {
    const body: FashionProductSearchRequestDto = { query };
    return this.http.post<Product[]>(`${this.apiUrl}/search`, body);
  }


  getProductDetails(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/details/${id}`);
  }

  getFilteredProductsChunk(
    pageSize: number,
    lastId = 0,
    filters: { category?: string; color?: string }
  ): Observable<Product[]> {
    let params = new HttpParams()
      .set('pageSize', pageSize.toString())
      .set('lastId', lastId.toString());

    if (filters.category) params = params.set('category', filters.category);
    if (filters.color) params = params.set('color', filters.color);

    return this.http.get<Product[]>(this.apiUrl, { params });
  }
}
