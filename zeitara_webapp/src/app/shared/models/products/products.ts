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

export interface FashionProductWithStatusDto extends Product {
  isLiked?: boolean;
  isInCart?: boolean;
}



