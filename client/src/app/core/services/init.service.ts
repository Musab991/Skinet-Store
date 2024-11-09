import { Injectable, inject } from '@angular/core';
import { CartService } from './cart.service';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private cartService = inject(CartService);

  init() {
    const cartId = localStorage.getItem('cart_id');
    if (cartId) {
      return this.cartService.getCart(cartId);
    }
    return of(null); // Always return an observable
  }
}
