import { Component, HostListener, OnInit } from '@angular/core';
import { Product } from '../../shared/models/products/products';
import { ProductService } from '../../services/products.service';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag'; // For displaying colored tags
import { ImageModule } from 'primeng/image'; // For better image handling
import { ProductCardComponent } from "../product-card/product-card.component";
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { Button } from 'primeng/button';
import { AutoCompleteSelectEvent } from 'primeng/autocomplete';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-products-page',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, TagModule, ImageModule, ProductCardComponent, ProgressSpinnerModule, Button], // Added PrimeNG modules
  templateUrl: './products-page.component.html',
  styleUrl: './products-page.component.css'
})
export class ProductsPageComponent implements OnInit {
  onSuggestionSelected(event: AutoCompleteSelectEvent) {
    this.selectedProduct = event.value;
    console.log('Selected product ID:', this.selectedProduct?.id); // record the product ID
  }
  products: Product[] = [];
  loading = false;
  error: string | null = null;
  lastLoadedId = 0;
  pageSize = 12;
  hasMore = true;
  searchTerm: string = '';
  filteredSuggestions: Product[] = [];
  selectedProduct: Product | null = null;

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  filterProducts(event: any) {
    const query = event.query.toLowerCase();
    this.filteredSuggestions = this.products.filter(p =>
      p.productDisplayName.toLowerCase().includes(query)
    );
  }


  loadProducts(): void {
    if (this.loading || !this.hasMore) return;

    this.loading = true;
    this.productService.getProductsChunk(this.pageSize, this.lastLoadedId)
      .subscribe({
        next: (newProducts) => {
          this.products = [...this.products, ...newProducts];
          this.lastLoadedId = newProducts[newProducts.length - 1]?.id || 0;
          this.hasMore = newProducts.length === this.pageSize;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load products. Please try again.';
          this.loading = false;
        }
      });
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 500) {
      this.loadProducts();
    }
  }
}
