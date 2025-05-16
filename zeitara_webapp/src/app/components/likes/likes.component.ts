import { Component, OnInit } from '@angular/core';
import { UserSessionService } from '../../services/user-session.service';
import { LikesService } from '../../services/like.service';
import { Product } from '../../shared/models/products/products';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ProductCardComponent } from '../product-card/product-card.component';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-likes',
  standalone: true,
  imports: [
    CommonModule,
    ProductCardComponent,
    ProgressSpinnerModule,
    ButtonModule
  ],
  templateUrl: './likes.component.html',
  styleUrl: './likes.component.css'
})
export class LikesComponent implements OnInit {
  likedProducts: Product[] = [];
  loading = false;
  error: string | null = null;
  hasMore = true;
  currentPage = 0;
  pageSize = 12;
  lastLoadedId = 0;

  constructor(
    private userSessionService: UserSessionService,
    private likesService: LikesService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadLikedProducts();
  }

  loadLikedProducts(): void {
    const user = this.userSessionService.getCurrentUser();
    if (!user?.user_Id) {
      this.error = 'Please login to view your liked products';
      return;
    }

    this.loading = true;
    this.error = null;

    this.likesService.getLikedProductsChunk(user.user_Id, this.pageSize, this.lastLoadedId)
      .subscribe({
        next: (products) => {
          this.likedProducts = [...this.likedProducts, ...products];
          this.loading = false;
          this.hasMore = products.length === this.pageSize;
          if (products.length > 0) {
            this.lastLoadedId = products[products.length - 1].id;
          }
        },
        error: (err) => {
          this.error = 'Failed to load liked products';
          this.loading = false;
          console.error('Error loading liked products:', err);
        }
      });
  }

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }

  // Optional: Infinite scroll handler
  onScroll(): void {
    if (!this.loading && this.hasMore) {
      this.loadLikedProducts();
    }
  }
}
