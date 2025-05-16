// services/purchase.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { InteractionResponse, PurchaseRequest, PurchaseResponse } from '../shared/models/purchase/purchase';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {
  private apiUrl = `${environment.apiUrl}/api/Purchase`;

  constructor(private http: HttpClient) { }

  // POST /api/Purchase/{userId}
  purchaseAllCartItems(userId: number): Observable<InteractionResponse> {
    if (userId <= 0) {
      throw new Error('Invalid user ID');
    }
    return this.http.post<InteractionResponse>(`${this.apiUrl}/${userId}`, {});
  }

  // GET /api/Purchase/{userId}
  getUserPurchases(
    userId: number,
    pageSize: number = 10,
    lastLoadedId: number = 0
  ): Observable<PurchaseResponse[]> {
    if (userId <= 0 || pageSize <= 0) {
      throw new Error('Invalid parameters');
    }

    const params = new HttpParams()
      .set('pageSize', pageSize.toString())
      .set('lastLoadedId', lastLoadedId.toString());

    return this.http.get<PurchaseResponse[]>(`${this.apiUrl}/${userId}`, { params });
  }

  // Optional: For individual product purchases
  purchaseProduct(request: PurchaseRequest): Observable<PurchaseResponse> {
    return this.http.post<PurchaseResponse>(this.apiUrl, request);
  }
}
