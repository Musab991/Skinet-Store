import { Component, inject, OnInit, output } from '@angular/core';
import { CheckOutService } from '../../../core/services/check-out.service';
import { MatRadioModule } from '@angular/material/radio';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { CartService } from '../../../core/services/cart.service';
import { DeliveryMethod } from '../../../models/deliveryMethod';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-checkout-delivery',
  standalone: true,
  imports: [MatRadioModule, CurrencyPipe, FormsModule, CommonModule],
  templateUrl: './checkout-delivery.component.html',
  styleUrl: './checkout-delivery.component.scss',
})
export class CheckoutDeliveryComponent implements OnInit {
  cartService = inject(CartService);
  checkoutService = inject(CheckOutService);
  deliveryComplete = output<boolean>();
  ngOnInit(): void {
    this.checkoutService.getDeliveryMethod()?.subscribe({
      next: (methods) => {
        if (this.cartService.cart()?.deliveryMethodId) {
          const method = methods.find(
            (x) => x.id === this.cartService.cart()?.deliveryMethodId
          );
          if (method) {
            this.cartService.selectedDelivery.set(method);
            this.sendNotificationThatDeliveryWasSelected();
          }
        }
      },
    });
  }
  sendNotificationThatDeliveryWasSelected() {
    this.deliveryComplete.emit(true);
  }
  updateDeliveryMethod(method: DeliveryMethod) {
    this.cartService.selectedDelivery.set(method);
    const cart = this.cartService.cart();

    if (cart) {
      cart.deliveryMethodId = method.id; // Corrected property name
      this.cartService.setCart(cart);
      this.sendNotificationThatDeliveryWasSelected();
    }
  }
}
