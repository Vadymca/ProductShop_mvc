import { CartItem } from "./CartItem";

export class Order {
    id: number= 0;
    userId:  string = "";
    items: CartItem[] = [];
    totalPrice: number = 0;
    status: string = "";
    orderDate: Date = new Date();;
  }