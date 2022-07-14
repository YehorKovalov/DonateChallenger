import { inject, injectable } from "inversify";
import iocServices from "../utilities/ioc/iocServices";
import { HttpService, MethodType } from "./HttpService";

export interface PaymentService {
     goToPayPalPayment(unitPrice: number, currencyCode: string, merchantId: string): void;
}

@injectable()
export default class DefaultPaymentService implements PaymentService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;

     private readonly paymentRoute = process.env.REACT_APP_PAYMENT_CONTROLLER_ROUTE;

     public goToPayPalPayment(unitPrice: number, currencyCode: string, merchantId: string): void {
          const url = `${this.paymentRoute}/paymenturl?unitPrice=${unitPrice}&currencyCode=${currencyCode}&merchantId=${merchantId}&`;
          this.httpService.send<void>(url, MethodType.GET);
     }
}