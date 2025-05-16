import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Product } from '../shared/models/products/products';
import { FashionProductSearchRequestDto } from '../shared/models/products/products';
import { UserSessionService } from './user-session.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = `${environment.apiUrl}/FashionProduct`;

  constructor(
    private http: HttpClient,
    private userSession: UserSessionService
  ) { }

  private getUserIdParam(): HttpParams {
    const userId = this.userSession.getCurrentUser()?.user_Id;
    return new HttpParams().set('userId', userId?.toString() || '');
  }

  getProductsChunk(pageSize: number, lastId = 0): Observable<Product[]> {
    let params = this.getUserIdParam()
      .set('pageSize', pageSize.toString())
      .set('lastId', lastId.toString());

    return this.http.get<Product[]>(this.apiUrl, { params });
  }

  searchProducts(query: string): Observable<Product[]> {
    const body: FashionProductSearchRequestDto = { query };
    const params = this.getUserIdParam();
    return this.http.post<Product[]>(`${this.apiUrl}/search`, body, { params });
  }

  getProductDetails(id: number): Observable<Product> {
    const params = this.getUserIdParam();
    return this.http.get<Product>(`${this.apiUrl}/details/${id}`, { params });
  }

  getFilteredProductsChunk(
    pageSize: number,
    lastId = 0,
    filters: { category?: string; color?: string }
  ): Observable<Product[]> {
    let params = this.getUserIdParam()
      .set('pageSize', pageSize.toString())
      .set('lastId', lastId.toString());

    if (filters.category) params = params.set('category', filters.category);
    if (filters.color) params = params.set('color', filters.color);

    return this.http.get<Product[]>(`${this.apiUrl}/filtered`, { params });
  }
}
