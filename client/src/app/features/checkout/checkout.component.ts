import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { OrderSummaryComponent } from '../../shared/component/order-summary/order-summary.component';
import { MatStepper, MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { StripeService } from '../../core/services/stripe.service';
import {
  ConfirmationToken,
  StripeAddressElement,
  StripeAddressElementChangeEvent,
  StripePaymentElement,
  StripePaymentElementChangeEvent,
} from '@stripe/stripe-js';
import { SnackbarService } from '../../core/services/snackbar.service';
import {
  MatCheckboxChange,
  MatCheckboxModule,
} from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Address } from '../../models/user';
import { AccountService } from '../../core/services/account.service';
import { firstValueFrom } from 'rxjs';
import { CheckoutDeliveryComponent } from './checkout-delivery/checkout-delivery.component';
import { CheckoutReviewComponent } from './checkout-review/checkout-review.component';
import { CartService } from '../../core/services/cart.service';
import { CurrencyPipe, JsonPipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    MatButtonModule, // Use MatButtonModule instead of MatButton
    RouterLink,
    MatCheckboxModule,
    CheckoutDeliveryComponent,
    CheckoutReviewComponent,
    CurrencyPipe,
    JsonPipe,
    MatProgressSpinnerModule,
  ],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'], // Fixed styleUrls property name
})
export class CheckoutComponent implements OnInit, OnDestroy {
  saveAddress: boolean = false;
  accountService = inject(AccountService);
  cartService = inject(CartService);
  paymentElement?: StripePaymentElement | undefined;
  private stripeService = inject(StripeService);
  addressElement?: StripeAddressElement;
  private snackbar = inject(SnackbarService);
  completionStatus = signal<{
    address: boolean;
    card: boolean;
    delivery: boolean;
  }>({ address: false, card: false, delivery: false });

  confirmationToken?: ConfirmationToken;
  private router = inject(Router);
  loading: boolean = false;
  async getConfirmationToken() {
    try {
      if (
        Object.values(this.completionStatus()).every(
          (status) => status === true
        )
      ) {
        const result = await this.stripeService.createConfirmationToken();
        if (result.error) {
          throw new Error(result.error.message);
        }
        this.confirmationToken = result.confirmationToken;
        console.log('token:', this.confirmationToken);
      }
    } catch (error: any) {
      this.snackbar.error(error.message);
    }
    if (this.completionStatus()) {
      if (
        this.completionStatus().address &&
        this.completionStatus().delivery &&
        this.completionStatus().card
      ) {
        this.stripeService.createConfirmationToken();
      }
    }
  }
  onSaveAddressCheckBoxChanged(event: MatCheckboxChange) {
    this.saveAddress = event.checked;
  }
  async onStepChange(event: StepperSelectionEvent) {
    //based -0 index
    if (event.selectedIndex === 1) {
      if (this.saveAddress === true) {
        const address = await this.getAddressFromStripeAdderss();
        if (address) {
          firstValueFrom(this.accountService.updateAdderss(address));
        }
      }
    }
    if (event.selectedIndex === 2) {
      await firstValueFrom(
        await this.stripeService.createOrUpdatePaymentIntent()
      );
    }
    if (event.selectedIndex === 3) {
      await this.getConfirmationToken();
    }
  }

  async confirmPayment(stepper: MatStepper) {
    this.loading = true;
    try {
      if (this.confirmationToken) {
        const result = await this.stripeService.confirmPayment(
          this.confirmationToken
        );
        if (result.error) {
          throw new Error(result.error.message);
        } else {
          this.cartService.deleteCart();
          this.cartService.selectedDelivery();
          this.router.navigateByUrl('/checkout/success');
        }
      }
    } catch (error: any) {
      this.snackbar.error(error.message || 'Something went wrong');
      stepper.previous();
    } finally {
      this.loading = false;
    }
  }

  async ngOnInit() {
    try {
      this.addressElement = await this.stripeService.createAddressElement();
      this.addressElement.mount(`#address-element`);
      this.addressElement.on('change', this.handleAddressChange);
      this.paymentElement = await this.stripeService.createPaymentElement();
      this.paymentElement.mount(`#payment-element`);
      this.paymentElement?.on('change', this.handlePaymentChange);
    } catch (error: any) {
      this.snackbar.error(error.message);
    }
  }
  handlePaymentChange = (event: StripePaymentElementChangeEvent) => {
    this.completionStatus.update((state) => {
      state.card = event.complete;
      return state;
    });
  };
  handleDeliveryChange(event: boolean) {
    this.completionStatus.update((state) => {
      state.delivery = event;
      return state;
    });
  }
  handleAddressChange = (event: StripeAddressElementChangeEvent) => {
    this.completionStatus.update((state) => {
      state.address = event.complete;
      return state;
    });
  };

  ngOnDestroy(): void {
    this.stripeService.disposeElement();
  }

  private async getAddressFromStripeAdderss(): Promise<Address | null> {
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;
    if (address) {
      return {
        line1: address.line1,
        line2: address.line2 || undefined,
        city: address.city,
        country: address.country,
        state: address.state,
        postalCode: address.postal_code,
      };
    } else {
      return null;
    }
  }
}
