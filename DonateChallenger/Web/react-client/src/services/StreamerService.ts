import { injectable, inject } from "inversify";
import { SearchedStreamerByNicknameDto } from "../dtos/DTOs/SearchedStreamerByNicknameDto";
import { StreamerProfileDto } from "../dtos/DTOs/StreamerProfileDto";
import { ChangeProfileDataResponse } from "../dtos/response/ChangeProfileDataResponse";
import { GetMinDonatePriceResponse } from "../dtos/response/GetMinDonatePriceResponse";
import { GetStreamerProfileResponse } from "../dtos/response/GetStreamerProfileResponse";
import { SearchStreamersByNicknameResponse } from "../dtos/response/SearchStreamersNicknamesResponse";
import AuthStore from "../oidc/AuthStore";
import iocServices from "../utilities/ioc/iocServices";
import iocStores from "../utilities/ioc/iocStores";
import { HttpService, MethodType } from "./HttpService";

export interface StreamerService {
     searchStreamersByNickname(nicknameAsFilter: string) : Promise<SearchStreamersByNicknameResponse<SearchedStreamerByNicknameDto>>;
     getMinDonatePrice(streamerId: string) : Promise<GetMinDonatePriceResponse>;
     getStreamerProfileById(streamerId: string) : Promise<GetStreamerProfileResponse<StreamerProfileDto>>;
     changeMinDonatePrice(streamerId: string, changeOn: number) : Promise<ChangeProfileDataResponse<number>>;
}

@injectable()
export default class DefaultStreamerService implements StreamerService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     private readonly streamerApiRoute = process.env.REACT_APP_STREAMER_CONTROLLER_ROUTE;

     public async searchStreamersByNickname(nicknameAsFilter: string) : Promise<SearchStreamersByNicknameResponse<SearchedStreamerByNicknameDto>> {
          const url = `${this.streamerApiRoute}/searchNicknames?nicknameAsFilter=${nicknameAsFilter}`;
          const method = MethodType.GET;
          var result = await this.httpService.send<SearchStreamersByNicknameResponse<SearchedStreamerByNicknameDto>>(url, method);
          return { ...result.data };
     }

     public async getMinDonatePrice(streamerId: string) : Promise<GetMinDonatePriceResponse> {
          const url = `${this.streamerApiRoute}/getMinDonatePriceResponse?streamerId=${streamerId}`;
          const method = MethodType.GET;
          const headers = await this.authStore.tryGetAuthorizedHeaders();

          var result = await this.httpService.send<GetMinDonatePriceResponse>(url, method, headers);
          return { ... result.data };
     }

     public async changeMinDonatePrice(streamerId: string, changeOn: number) : Promise<ChangeProfileDataResponse<number>> {
          const url = `${this.streamerApiRoute}/changeMinDonatePrice?streamerId=${streamerId}&&changeOn=${changeOn}`;
          const method = MethodType.POST;
          const headers = await this.authStore.tryGetAuthorizedHeaders();

          var result = await this.httpService.send<ChangeProfileDataResponse<number>>(url, method, headers);
          return { ... result.data };
     }

     public async getStreamerProfileById(streamerId: string): Promise<GetStreamerProfileResponse<StreamerProfileDto>> {
          
          const url = `${this.streamerApiRoute}/userProfile?streamerId=${streamerId}`;
          const method = MethodType.GET;
          const headers = await this.authStore.tryGetAuthorizedHeaders();

          var result = await this.httpService.send<GetStreamerProfileResponse<StreamerProfileDto>>(url, method, headers);
          return { ... result.data };
     }
}