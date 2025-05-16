import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../shared/models/products/products';
import { DialogService } from 'primeng/dynamicdialog';
import { ProductDetailOverlayComponent } from '../product-detail-overlay/product-detail-overlay.component';
import { LikesService } from '../../services/like.service';
import { UserSessionService } from '../../services/user-session.service';
import { CommonModule } from '@angular/common';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-product-card',
  standalone: true,
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css'],
  imports: [CommonModule, ButtonModule],
  providers: [DialogService, MessageService]
})
export class ProductCardComponent {
  @Input() product!: Product;
  isProcessingLike = false;

  constructor(
    private dialogService: DialogService,
    private likesService: LikesService,
    private userSession: UserSessionService,
    private messageService: MessageService
  ) { }



  showDetails() {
    this.dialogService.open(ProductDetailOverlayComponent, {
      data: { product: this.product },
      width: '80%',
      contentStyle: { overflow: 'auto' },
      baseZIndex: 10000,
      dismissableMask: true
    });
  }

  toggleLike(event: Event): void {
    event.stopPropagation();

    const user = this.userSession.getCurrentUser();
    if (!user) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Login Required',
        detail: 'Please login to like products',
        life: 3000
      });
      return;
    }

    this.isProcessingLike = true;
    const request = {
      userId: user.user_Id,
      productId: this.product.id
    };

    const action = this.product.isLiked
      ? this.likesService.unlikeProduct(request)
      : this.likesService.likeProduct(request);

    action.subscribe({
      next: () => {
        this.product.isLiked = !this.product.isLiked;
        this.isProcessingLike = false;
      },
      error: (err) => {
        console.error('Error toggling like:', err);
        this.isProcessingLike = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to update like status',
          life: 3000
        });
      }
    });
  }
}
