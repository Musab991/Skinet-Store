export interface Order {
  id: number;
  orderDate: string;
  buyerEmail: string;
  shippingAddress: ShippingAddress;
  deliveryMethod: string;
  shippingPrice: number;
  paymentSummary: PaymentSummary;
  orderItems: OrderItem[];
  subTotal: number;
  status: string;
  paymentIntentId: string;
  total: number;
}

export interface ShippingAddress {
  name: string;
  line1: string;
  line2: any;
  city: string;
  state: string;
  postalCode: string;
  country: string;
}

export interface PaymentSummary {
  last4: number;
  brand: string;
  expMonth: number;
  expYear: number;
}

export interface OrderItem {
  productId: number;
  productName: string;
  PictureUrl: string;
  price: number;
  quantity: number;
}

export interface OrderToCreate {
  cartId: string;
  deliveryMethodId: number;
  shippingAddress: ShippingAddress;
  paymentSummary: PaymentSummary;
}