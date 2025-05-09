import { Component, Input } from '@angular/core';
import { FashionProductWithStatusDto, Product } from '../../shared/models/products/products';
import { DialogService } from 'primeng/dynamicdialog';
import { ProductDetailOverlayComponent } from '../product-detail-overlay/product-detail-overlay.component';
import { LikesService } from '../../services/like.service';
import { UserSessionService } from '../../services/user-session.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css'],
  providers: [DialogService, LikesService]
})
export class ProductCardComponent {
  @Input() product!: FashionProductWithStatusDto;
  isProcessingLike = false;

  constructor(
    private dialogService: DialogService,
    private likesService: LikesService,
    private userSession: UserSessionService
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
    if (this.isProcessingLike) return;

    const currentUser = this.userSession.getCurrentUser();
    if (!currentUser) return;

    this.isProcessingLike = true;
    const userId = currentUser.userId;
    const productId = this.product.id;

    const action$ = this.product.isLiked
      ? this.likesService.unlikeProduct(productId, userId)
      : this.likesService.likeProduct(productId, userId);
    action$.subscribe({
      next: () => {
        this.product.isLiked = !this.product.isLiked;
      },
      error: () => {
      },
      complete: () => {
        this.isProcessingLike = false;
      }
    });
  }
}
