import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { CartService } from './cart.service';
import { environment } from '../../../environments/environment';
import { DeliveryMethod } from '../../models/deliveryMethod';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CheckOutService {
  baseUrl = environment.apiUrl;
  http = inject(HttpClient);
  deliveryMethods: DeliveryMethod[] = [];
  constructor() {}

  getDeliveryMethod() {
    if (this.deliveryMethods.length > 0) {
      return of(this.deliveryMethods); // Returns cached delivery methods as an Observable
    } else {
      return this.http
        .get<DeliveryMethod[]>(this.baseUrl + 'payments/delivery-methods')
        .pipe(
          map((methods) => {
            this.deliveryMethods = methods.sort((a, b) => b.price - a.price);
            return methods; // Returns fetched delivery methods
          })
        );
    }
  }
}
