import { injectable, inject } from "inversify";
import { SearchedStreamerByNicknameDto } from "../dtos/DTOs/SearchedStreamerByNicknameDto";
import { StreamerProfileDto } from "../dtos/DTOs/StreamerProfileDto";
import { ChangeStreamerProfileDataResponse } from "../dtos/response/ChangeStreamerProfileDataResponse";
import { GetMinDonatePriceResponse } from "../dtos/response/GetMinDonatePriceResponse";
import { GetStreamerProfileResponse } from "../dtos/response/GetStreamerProfileResponse";
import { SearchStreamersByNicknameResponse } from "../dtos/response/SearchStreamersNicknamesResponse";
import iocServices from "../utilities/ioc/iocServices";
import { HttpService, MethodType } from "./HttpService";

export interface StreamerService {
     searchStreamersByNickname(nicknameAsFilter: string) : Promise<SearchStreamersByNicknameResponse<SearchedStreamerByNicknameDto>>;
     getMinDonatePrice(streamerId: string) : Promise<GetMinDonatePriceResponse>;
     getStreamerProfileById(streamerId: string) : Promise<GetStreamerProfileResponse<StreamerProfileDto>>;
     changeMinDonatePrice(streamerId: string, changeOn: number) : Promise<ChangeStreamerProfileDataResponse<number>>;
     changeStreamerNickname(streamerId: string, changeOn: string) : Promise<ChangeStreamerProfileDataResponse<string>>;
}

@injectable()
export default class DefaultStreamerService implements StreamerService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;

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
          var result = await this.httpService.send<GetMinDonatePriceResponse>(url, method);
          return { ... result.data };
     }

     public async changeMinDonatePrice(streamerId: string, changeOn: number) : Promise<ChangeStreamerProfileDataResponse<number>> {
          const url = `${this.streamerApiRoute}/changeMinDonatePrice?streamerId=${streamerId}&&changeOn=${changeOn}`;
          const method = MethodType.POST;
          var result = await this.httpService.send<ChangeStreamerProfileDataResponse<number>>(url, method);
          return { ... result.data };
     }

     public async getStreamerProfileById(streamerId: string): Promise<GetStreamerProfileResponse<StreamerProfileDto>> {
          const url = `${this.streamerApiRoute}/userProfile?streamerId=${streamerId}`;
          const method = MethodType.GET;
          var result = await this.httpService.send<GetStreamerProfileResponse<StreamerProfileDto>>(url, method);
          return { ... result.data };
     }

     public async changeStreamerNickname(streamerId: string, changeOn: string): Promise<ChangeStreamerProfileDataResponse<string>> {
          const url = `${this.streamerApiRoute}/changeNickname?streamerId=${streamerId}&&newNickname=${changeOn}`;
          const method = MethodType.POST;
          var result = await this.httpService.send<ChangeStreamerProfileDataResponse<string>>(url, method);
          return { ... result.data };
     }
}