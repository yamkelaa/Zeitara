// shared/models/purchases/purchase.model.ts
import { Product } from '../products/products'; // Import your existing Product interface

// Request DTO (matches backend PurchaseRequestDto)
export interface PurchaseRequest {
  userId: number;
  productId: number;
  quantity: number;
}

// Response DTO (matches backend PurchaseResponseDto)
export interface PurchaseResponse {
  purchaseId: number;
  productId: number;
  quantity: number;
  status: string;
  purchaseDate: Date;
  productDisplayName: string;
  imageUrl: string;
  isLiked: boolean;
  isInCart: boolean;
  isPurchased: boolean;
  userRating?: number;
}

// For the cart purchase response (matches InteractionResponseDto)
export interface InteractionResponse {
  message: string;
}
