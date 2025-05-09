import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Product } from '../shared/models/products/products';
import { InteractionRequestDto } from '../shared/models/request/interaction-request';
import { PaginationRequestDto } from '../shared/models/request/pagination-requests';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = `${environment.apiUrl}/Cart`;

  constructor(private http: HttpClient) { }

  // Add to cart
  addToCart(request: InteractionRequestDto): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}`, request);
  }

  // Remove from cart
  removeFromCart(request: InteractionRequestDto): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}`, { body: request });
  }

  // Get paginated cart items
  getCartItems(
    userId: number,
    pagination: PaginationRequestDto
  ): Observable<Product[]> {
    const params = new HttpParams()
      .set('pageNumber', pagination.pageNumber.toString())
      .set('lastLoadedId', pagination.lastLoadedId?.toString() || '');

    return this.http.get<Product[]>(`${this.apiUrl}/${userId}`, { params });
  }

  // Optional: Check if products are in cart (bulk)
  checkCartStatus(
    userId: number,
    productIds: number[]
  ): Observable<number[]> {
    return this.http.post<number[]>(
      `${this.apiUrl}/status`,
      { userId, productIds }
    );
  }
}
