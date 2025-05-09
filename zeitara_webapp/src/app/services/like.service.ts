import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Product } from '../shared/models/products/products';
import { InteractionRequestDto } from '../shared/models/request/interaction-request';
import { PaginationRequestDto } from '../shared/models/request/pagination-requests';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  private apiUrl = `${environment.apiUrl}/Like`;

  constructor(private http: HttpClient) { }

  likeProduct(productId: number, userId: number): Observable<void> {
    const request: InteractionRequestDto = { productId, userId };
    return this.http.post<void>(`${this.apiUrl}`, request);
  }

  unlikeProduct(productId: number, userId: number): Observable<void> {
    const request: InteractionRequestDto = { productId, userId };
    return this.http.delete<void>(`${this.apiUrl}`, { body: request });
  }

  getLikedProducts(
    userId: number,
    pagination: PaginationRequestDto
  ): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/${userId}`, {
      params: {
        pageNumber: pagination.pageNumber.toString(),
        lastLoadedId: pagination.lastLoadedId?.toString() || ''
      }
    });
  }

  checkLikeStatus(userId: number, productIds: number[]): Observable<number[]> {
    return this.http.post<number[]>(`${this.apiUrl}/status`, { userId, productIds });
  }
}
