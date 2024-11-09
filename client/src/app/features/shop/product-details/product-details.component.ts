import { Component, OnInit, inject } from '@angular/core';
import { Product } from '../../../models/product';
import {
  MatCard,
  MatCardActions,
  MatCardContent,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle,
} from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatDivider } from '@angular/material/divider';
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    MatCard,
    MatCardContent,
    MatCardActions,
    MatIcon,
    CurrencyPipe,
    MatCardHeader,
    MatCardTitle,
    MatCardSubtitle,
    MatButton,
    MatFormField,
    MatLabel,
    MatDivider,
    CommonModule,
    FormsModule,
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
})
export class ProductDetailsComponent implements OnInit {
  cartService = inject(CartService);
  product?: Product;
  productId: number | undefined;
  private shopService = inject(ShopService);
  private route = inject(ActivatedRoute);
  quantityInCart: number = 0;
  quantity: number = 0;
  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    console.log('Loaded product ID:', this.productId); // Debug line
    if (this.productId) {
      this.shopService.getProductById(this.productId).subscribe({
        next: (response) => {
          this.product = response;
          this.updateQuantityInCart();
          console.log('Product fetched:', this.product); // Debug line
        },
        error: (err) =>
          console.error('Comes out of product-details-fetch:', err),
        complete: () => console.log('product-details Done'),
      });
    } else {
      console.error('Product ID is undefined or invalid.');
    }
  }

  updateCart() {
    if (!this.product) return;

    if (this.quantity > this.quantityInCart) {
      const itemsToAdd = this.quantity - this.quantityInCart;
      this.cartService.addItemToCart(this.product, itemsToAdd);
    } else {
      if (this.productId) {
        //items to remove
        const itemsToRemove = this.quantityInCart - this.quantity;
        this.cartService.removeItemFromCart(this.productId, itemsToRemove);
      }
    }
    this.updateQuantityInCart();
  }

  updateQuantityInCart() {
    this.quantityInCart =
      this.cartService
        .cart()
        ?.items.find((it) => it.productId === this.productId)?.quantity || 0;
    this.quantity = this.quantityInCart || 1;
  }

  getButtonText() {
    return this.quantityInCart > 0 ? 'Update cart' : 'Add to cart';
  }
}
