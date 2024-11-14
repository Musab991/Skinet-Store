import { Component, inject, Input } from '@angular/core';
import { CartService } from '../../../core/services/cart.service';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CartItem } from '../../../models/cart';
import { ConfirmationToken } from '@stripe/stripe-js';
import { AddressPipe } from '../../../shared/pipes/address.pipe';
import { CardPipe } from '../../../shared/pipes/card.pipe';

@Component({
  selector: 'app-checkout-review',
  standalone: true,
  imports: [
    CurrencyPipe,
    FormsModule,
    CommonModule,
    RouterModule,
    AddressPipe,
    CardPipe,
  ],
  templateUrl: './checkout-review.component.html',
  styleUrl: './checkout-review.component.scss',
})
export class CheckoutReviewComponent {
  cartService = inject(CartService);

  @Input() confirmationToken?: ConfirmationToken;
  //TrackBy function to optimze rendering
  trackByProductId(index: number, item: CartItem) {
    return item.productId; // Return the unique identifier (productId) of each item
  }
}
