import { inject, injectable } from "inversify";
import { ChallengeOrderDto } from "../dtos/DTOs/ChallengeOrderDto";
import { AddChallengeOrderRequest } from "../dtos/requests/AddChallengeOrderRequest";
import { GetPaginatedChallengeOrdersRequest } from "../dtos/requests/GetPaginatedChallengeOrdersRequest";
import { UpdateChallengeOrderRequest } from "../dtos/requests/UpdateChallengeOrderRequest";
import { AddChallengeOrderResponse } from "../dtos/response/AddChallengeOrderResponse";
import { GetChallengeOrderByIdResponse } from "../dtos/response/GetChallengeOrderByIdResponse";
import { GetPaginatedChallengeOrdersResponse } from "../dtos/response/GetPaginatedChallengeOrdersResponse";
import { UpdateChallengeOrderResponse } from "../dtos/response/UpdateChallengeOrderResponse";
import AuthStore from "../oidc/AuthStore";
import iocServices from "../utilities/ioc/iocServices";
import iocStores from "../utilities/ioc/iocStores";
import { HttpService, MethodType } from "./HttpService";

export interface ChallengeOrderManagerService {
     addChallengeOrder(paymentId: string, challengesAmount: number, resultDonationPrice: number): Promise<AddChallengeOrderResponse<string>>;
     updateChallengeOrder(challengeOrderId: string, paymentId: string, challengesAmount: number, resultDonationPrice: number): Promise<UpdateChallengeOrderResponse<string>>;
     getPaginatedOrders(currentPage: number, commentsPerPage: number): Promise<GetPaginatedChallengeOrdersResponse<ChallengeOrderDto>>;
     getOrderById(orderId: string): Promise<GetChallengeOrderByIdResponse<ChallengeOrderDto>>;
}

@injectable()
export default class DefaultChallengeOrderManagerService implements ChallengeOrderManagerService {
     
     @inject(iocServices.httpService) private readonly httpService!: HttpService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     private readonly challengeOrderManagerApiRoute = process.env.REACT_APP_CHALLENGE_ORDER_MANAGER_CONTROLLER_ROUTE;

     public async addChallengeOrder(paymentId: string, challengesAmount: number, resultDonationPrice: number): Promise<AddChallengeOrderResponse<string>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.challengeOrderManagerApiRoute}/add`;
          const request: AddChallengeOrderRequest = {
               paymentId: paymentId,
               challengesAmount: challengesAmount,
               resultDonationPrice: resultDonationPrice
          }

          var response = await this.httpService.send<AddChallengeOrderResponse<string>>(url, MethodType.POST, headers, request);
          return { ...response.data };
     }

     public async updateChallengeOrder(challengeOrderId: string, paymentId: string, challengesAmount: number, resultDonationPrice: number): Promise<UpdateChallengeOrderResponse<string>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.challengeOrderManagerApiRoute}/update`;
          const request: UpdateChallengeOrderRequest = {
               challengeOrderId: challengeOrderId,
               paymentId: paymentId,
               challengesAmount: challengesAmount,
               resultDonationPrice: resultDonationPrice
          }

          var response = await this.httpService.send<UpdateChallengeOrderResponse<string>>(url, MethodType.POST, headers, request);
          return { ...response.data };
     }

     public async getPaginatedOrders(currentPage: number, ordersPerPage: number): Promise<GetPaginatedChallengeOrdersResponse<ChallengeOrderDto>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const method = MethodType.POST;
          const url = `${this.challengeOrderManagerApiRoute}/get`;

          const request: GetPaginatedChallengeOrdersRequest = {
               currentPage: currentPage,
               ordersPerPage: ordersPerPage
               
          };

          const response = await this.httpService.send<GetPaginatedChallengeOrdersResponse<ChallengeOrderDto>>(url, method, headers, request);

          return { ...response.data };
     }

     public async getOrderById(orderId: string): Promise<GetChallengeOrderByIdResponse<ChallengeOrderDto>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.challengeOrderManagerApiRoute}/get?orderId=${orderId}`;

          var response = await this.httpService.send<GetChallengeOrderByIdResponse<ChallengeOrderDto>>(url, MethodType.GET, headers);

          return { ...response.data };
     }

}