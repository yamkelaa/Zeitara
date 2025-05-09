import { Component, OnInit, Inject } from '@angular/core';
import { Product } from '../../shared/models/products/products';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-product-detail-overlay',
  standalone: true,
  imports: [CommonModule, ButtonModule],
  templateUrl: './product-detail-overlay.component.html',
  styleUrls: ['./product-detail-overlay.component.css']
})
export class ProductDetailOverlayComponent implements OnInit {
  product!: Product;

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig
  ) { }

  ngOnInit(): void {
    this.product = this.config.data.product;
    console.log(this.product);
  }

  close() {
    this.ref.close();
  }
}
