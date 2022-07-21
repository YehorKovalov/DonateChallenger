import { injectable, inject } from "inversify";
import { UserProfileDto } from "../dtos/DTOs/UserProfileDto";
import { ChangeProfileDataResponse } from "../dtos/response/ChangeProfileDataResponse";
import { GetUserProfileResponse } from "../dtos/response/GetUserProfileResponse";
import AuthStore from "../oidc/AuthStore";
import iocServices from "../utilities/ioc/iocServices";
import iocStores from "../utilities/ioc/iocStores";
import { HttpService, MethodType } from "./HttpService";

export interface UserService {
     getUserProfileById(userId: string) : Promise<GetUserProfileResponse<UserProfileDto>>;
     changeNickname(userId: string, changeOn: string) : Promise<ChangeProfileDataResponse<string>>;
}

@injectable()
export default class DefaultUserService implements UserService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     private readonly userApiRoute = process.env.REACT_APP_USER_PROFILE_CONTROLLER_ROUTE;

     public async changeNickname(userId: string, changeOn: string): Promise<ChangeProfileDataResponse<string>> {

          const url = `${this.userApiRoute}/changeNickname?userId=${userId}&&newNickname=${changeOn}`;
          const method = MethodType.POST;
          const headers = await this.authStore.tryGetAuthorizedHeaders();

          var result = await this.httpService.send<ChangeProfileDataResponse<string>>(url, method, headers);

          return { ... result.data };
     }

     public async getUserProfileById(userId: string): Promise<GetUserProfileResponse<UserProfileDto>> {
          
          const url = `${this.userApiRoute}/userProfile?userId=${userId}`;
          const method = MethodType.GET;
          const headers = await this.authStore.tryGetAuthorizedHeaders();

          var result = await this.httpService.send<GetUserProfileResponse<UserProfileDto>>(url, method, headers);

          return { ... result.data };
     }
}