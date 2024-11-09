import { Injectable, computed, inject, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart } from '../../models/cart';
import { CartItem } from '../../models/cart';
import { Product } from '../../models/product';
import { map, Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);
  itemCount = computed(() => {
    if (!this.cart()) return;

    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0);
  });
  totals = computed(() => {
    if (!this.cart()) return;
    const cart = this.cart();
    if (!cart) return null;
    const subtotal = this.cart()?.items.reduce(
      (total, item) => total + item.quantity * item.price,
      0
    );
    const shipping = 0;
    const discount = 0;
    return {
      subtotal,
      shipping,
      discount,
      total: subtotal ?? 0 + shipping - discount,
    };
  });
  getCart(id: string): Observable<Cart> {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map((cart) => {
        this.cart.set(cart); // Updating cart state
        return cart; // Returning the cart
      })
    );
  }

  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: (cart) => this.cart.set(cart),
    });
  }

  addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCart();

    if (this.isProduct(item)) {
      item = this.mapProductToCartItem(item);
    }

    cart.items = this.addOrUpdateItem(cart.items, item, quantity);

    this.setCart(cart);
  }

  removeItemFromCart(productId: number, quantity = 1) {
    const cart = this.cart();

    if (!cart) {
      return;
    }
    const index = cart.items.findIndex((it) => it.productId === productId);
    if (index === -1) {
      return;
    }
    console.log('quantity?', quantity);
    if (cart.items[index].quantity > quantity) {
      console.log('HERE?');
      cart.items[index].quantity -= quantity;
    } else {
      cart.items.splice(index, 1);
    }
    if (cart.items.length === 0) {
      this.deleteCart();
    } else {
      this.setCart(cart); //update cart
    }
  }
  deleteCart() {
    const cartId = this.cart()?.id;
    if (!cartId) {
      console.error('Cart ID is undefined');
      return;
    }
    console.log(this.cart()?.id);

    this.http.delete(`${this.baseUrl}cart?id=${cartId}`).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      },
      error: (error) => {
        console.error('Error deleting cart:', error);
      },
    });
  }

  private addOrUpdateItem(
    items: CartItem[] = [], // Default value is an empty array
    item: CartItem,
    quantity: any
  ): CartItem[] {
    // Ensure items is always an array (initialize if null or undefined)
    items = items ?? []; // If items is null or undefined, initialize it as an empty array    console.log('items:', items);
    const index = items.findIndex((i) => i.productId === item.productId);

    if (index === -1) {
      // Add a new item if it doesn't exist in the cart
      item.quantity = quantity;
      items.push(item);
    } else {
      // Update quantity if item already exists

      items[index].quantity += quantity;
    }

    return items;
  }

  private mapProductToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      brand: item.brand,
      pictureUrl: item.pictureUrl,
      price: item.price,
      productName: item.name,
      quantity: 0,
      type: item.type,
    };
  }

  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
    //cartItem has no ('id')proprty ,while product has one
    //so if after casting to product still not exists (property[id]) then this is ,item
    //otherwise it's a product
  }

  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }
}
