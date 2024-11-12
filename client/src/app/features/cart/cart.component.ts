import { Component, inject } from '@angular/core';
import { CartService } from '../../core/services/cart.service';
import { CartItemComponent } from './cart-item/cart-item.component';
import { OrderSummaryComponent } from '../../shared/component/order-summary/order-summary.component';
import { CommonModule } from '@angular/common';
import { EmptyStateComponent } from '../../shared/component/empty-state/empty-state.component';
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CartItemComponent,
    OrderSummaryComponent,
    CommonModule,
    EmptyStateComponent,
    EmptyStateComponent,
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent {
  cartService = inject(CartService);
}
