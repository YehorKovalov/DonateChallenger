import { inject, injectable } from "inversify";
import { GetPayPalPaymentUrlResponse } from "../dtos/response/GetPayPalPaymentUrlResponse";
import iocServices from "../utilities/ioc/iocServices";
import { HttpService, MethodType } from "./HttpService";

export interface PaymentService {
     getPayPalPaymentUrl(unitPrice: number, currencyCode: string, merchantId: string): Promise<GetPayPalPaymentUrlResponse>;
}

@injectable()
export default class DefaultPaymentService implements PaymentService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;

     private readonly paymentRoute = process.env.REACT_APP_PAYMENT_CONTROLLER_ROUTE;

     public async getPayPalPaymentUrl(unitPrice: number, currencyCode: string, merchantId: string): Promise<GetPayPalPaymentUrlResponse> {

          const url = `${this.paymentRoute}/paymenturl?unitPrice=${unitPrice}&currencyCode=${currencyCode}&merchantId=${merchantId}`;
          const response = await this.httpService.send<GetPayPalPaymentUrlResponse>(url, MethodType.GET);
          return response.data;
     }
}