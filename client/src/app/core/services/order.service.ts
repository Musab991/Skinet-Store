import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Order, OrderToCreate } from '../../models/order';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  orderComplete: boolean | undefined;

  createOrder(orderToCreate: OrderToCreate) {
    console.log('orderToCreate:', orderToCreate);
    return this.http.post<Order>(this.baseUrl + 'orders', { orderToCreate });
  }

  getOrdersForUser() {
    return this.http.get<Order[]>(this.baseUrl + 'orders');
  }
  getOrderDetailed(id: number) {
    return this.http.get<Order>(this.baseUrl + 'orders/' + id);
  }
}
