import { inject, injectable } from "inversify";
import { StreamerProfileDto } from "../dtos/DTOs/StreamerProfileDto";
import { UserProfileDto } from "../dtos/DTOs/UserProfileDto";
import { ManagerGetPortionedUsersRequest } from "../dtos/requests/ManagerGetPortionedUsersRequest";
import { ManagerUpdateStreamerProfileRequest } from "../dtos/requests/ManagerUpdateStreamerProfileRequest";
import { ManagerUpdateUserProfileRequest } from "../dtos/requests/ManagerUpdateUserProfileRequest";
import { DeleteUserByIdResponse } from "../dtos/response/DeleteUserByIdResponse";
import { ManagerGetPortionedUsersResponse } from "../dtos/response/ManagerGetPortionedUsersResponse";
import { ManagerUpdateProfileResponse } from "../dtos/response/ManagerUpdateProfileResponse";
import AuthStore from "../oidc/AuthStore";
import iocServices from "../utilities/ioc/iocServices";
import iocStores from "../utilities/ioc/iocStores";
import { HttpService, MethodType } from "./HttpService";

export interface UserManagerService {
     updateStreamerProfile(userId: string, nickname: string, email: string, merchantId: string, minDonatePrice: number): Promise<ManagerUpdateProfileResponse>;
     updateUserProfile(userId: string, nickname: string, email: string):  Promise<ManagerUpdateProfileResponse>;
     delete(userId: string):  Promise<DeleteUserByIdResponse<boolean>>;
}

@injectable()
export default class DefaultUserManagerService implements UserManagerService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     private readonly userManagerApiRoute = process.env.REACT_APP_USER_MANAGER_CONTROLLER_ROUTE;

     public async updateStreamerProfile(userId: string, nickname: string, email: string, merchantId: string, minDonatePrice: number): Promise<ManagerUpdateProfileResponse> {
          const url = `${this.userManagerApiRoute}/updateStreamerProfile`;
          const method = MethodType.POST;
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const request: ManagerUpdateStreamerProfileRequest = {
               userId: userId,
               nickname: nickname,
               email: email,
               merchantId: merchantId,
               minDonatePrice: minDonatePrice
          }

          var result = await this.httpService.send<ManagerUpdateProfileResponse>(url, method, headers, request);

          return { ... result.data };
     }

     public async updateUserProfile(userId: string, nickname: string, email: string): Promise<ManagerUpdateProfileResponse> {
          const url = `${this.userManagerApiRoute}/updateUserProfile`;
          const method = MethodType.POST;
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const request: ManagerUpdateUserProfileRequest = {
               userId: userId,
               nickname: nickname,
               email: email
          }

          var result = await this.httpService.send<ManagerUpdateProfileResponse>(url, method, headers, request);

          return { ... result.data };
     }

     public async delete(userId: string): Promise<DeleteUserByIdResponse<boolean>> {
          const url = `${this.userManagerApiRoute}/delete?userId=${userId}`;
          const method = MethodType.POST;
          const headers = await this.authStore.tryGetAuthorizedHeaders();

          var result = await this.httpService.send<DeleteUserByIdResponse<boolean>>(url, method, headers);

          return { ... result.data };
     }

     public async getAllUsers(currentPortion: number, usersPerPortion: number): Promise<ManagerGetPortionedUsersResponse<UserProfileDto>> {
          const url = `${this.userManagerApiRoute}/all`;
          const method = MethodType.GET;
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const request: ManagerGetPortionedUsersRequest = {
               currentPortion: currentPortion,
               usersPerPortion: usersPerPortion
          }

          var result = await this.httpService.send<ManagerGetPortionedUsersResponse<UserProfileDto>>(url, method, headers, request);

          return { ... result.data };
     }

     public async getStreamers(currentPortion: number, usersPerPortion: number): Promise<ManagerGetPortionedUsersResponse<StreamerProfileDto>> {
          const url = `${this.userManagerApiRoute}/streamers`;
          const method = MethodType.GET;
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const request: ManagerGetPortionedUsersRequest = {
               currentPortion: currentPortion,
               usersPerPortion: usersPerPortion
          }

          var result = await this.httpService.send<ManagerGetPortionedUsersResponse<StreamerProfileDto>>(url, method, headers, request);

          return { ... result.data };
     }
}