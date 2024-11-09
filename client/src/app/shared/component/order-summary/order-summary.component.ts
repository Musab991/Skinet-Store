import { CurrencyPipe } from '@angular/common';
import { Component, input, inject, OnInit } from '@angular/core';
import { Cart } from '../../../models/cart';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-order-summary',
  standalone: true,
  imports: [
    CurrencyPipe,
    MatButton,
    RouterLink,
    MatFormField,
    MatLabel,
    MatInput,
  ],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss',
})
export class OrderSummaryComponent {
  cartService = inject(CartService);
}