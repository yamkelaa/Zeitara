import { Product } from "../products/products";

export interface ReviewRequest {
  userId: number;
  productId: number;
  rating: number;
  comment: string;
  purchaseId?: number;
}

export interface ReviewResponse {
  reviewId: number;
  userId: number;
  productId: number;
  rating: number;
  comment: string;
  reviewDate: Date;
}

