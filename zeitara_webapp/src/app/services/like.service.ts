import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Product } from '../shared/models/products/products';
import { InteractionRequestDto } from '../shared/models/request/interaction-request';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  private apiUrl = `${environment.apiUrl}/Like`; // Added /api to match backend route

  constructor(private http: HttpClient) { }

  likeProduct(request: InteractionRequestDto): Observable<void> {
    if (!request || request.userId <= 0 || request.productId <= 0) {
      throw new Error('Invalid request parameters');
    }
    return this.http.post<void>(this.apiUrl, request);
  }

  unlikeProduct(request: InteractionRequestDto): Observable<void> {
    if (!request || request.userId <= 0 || request.productId <= 0) {
      throw new Error('Invalid request parameters');
    }
    return this.http.delete<void>(this.apiUrl, { body: request });
  }

  getLikedProductsChunk(
    userId: number,
    pageSize: number,
    lastLoadedId: number = 0
  ): Observable<Product[]> {
    if (userId <= 0 || pageSize <= 0) {
      throw new Error('Invalid parameters');
    }

    const params = new HttpParams()
      .set('pageSize', pageSize.toString())
      .set('lastLoadedId', lastLoadedId.toString());

    return this.http.get<Product[]>(`${this.apiUrl}/${userId}/chunk`, { params });
  }
}
