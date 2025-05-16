// models/product.model.ts
export interface Product {
  id: number;
  gender: string;
  masterCategory: string;
  subCategory: string;
  articleType: string;
  baseColour: string;
  season: string;
  year: number;
  usage: string;
  productDisplayName: string;
  imageUrl: string;
  isLiked: boolean;
  isInCart: boolean;
  isPurchased: boolean;
  userRating: number | null;
}

export interface ProductFilters {
  category?: string;
  color?: string;
  sortBy?: string;
  ascending?: boolean;
}

export interface FashionProductSearchRequestDto {
  query: string;
}




