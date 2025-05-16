// services/review.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ReviewRequest, ReviewResponse } from '../shared/models/review/review';
import { Product } from '../shared/models/products/products';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = `${environment.apiUrl}/api/Review`;

  constructor(private http: HttpClient) { }

  addReview(request: ReviewRequest): Observable<{ message: string }> {
    if (!request || request.userId <= 0 || request.productId <= 0 || request.rating < 1 || request.rating > 5) {
      throw new Error('Invalid review data');
    }
    return this.http.post<{ message: string }>(this.apiUrl, request);
  }

  getUserProductReviews(
    userId: number,
    pageSize: number = 10,
    lastLoadedId: number = 0
  ): Observable<Product[]> {
    if (userId <= 0) {
      throw new Error('Invalid user ID');
    }

    const params = new HttpParams()
      .set('pageSize', pageSize.toString())
      .set('lastLoadedId', lastLoadedId.toString());

    return this.http.get<Product[]>(`${this.apiUrl}/${userId}/reviews`, { params });
  }

  deleteReview(reviewId: number, userId: number): Observable<{ message: string }> {
    if (reviewId <= 0 || userId <= 0) {
      throw new Error('Invalid review or user ID');
    }

    const params = new HttpParams().set('userId', userId.toString());
    return this.http.delete<{ message: string }>(`${this.apiUrl}/${reviewId}`, { params });
  }

  // Optional: Get reviews for a specific product
  getProductReviews(productId: number): Observable<ReviewResponse[]> {
    return this.http.get<ReviewResponse[]>(`${this.apiUrl}/product/${productId}`);
  }
}
