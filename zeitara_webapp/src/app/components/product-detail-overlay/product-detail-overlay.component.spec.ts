import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductDetailOverlayComponent } from './product-detail-overlay.component';

describe('ProductDetailOverlayComponent', () => {
  let component: ProductDetailOverlayComponent;
  let fixture: ComponentFixture<ProductDetailOverlayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductDetailOverlayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductDetailOverlayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
