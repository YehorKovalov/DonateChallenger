import { injectable, inject } from "inversify";
import { StreamerProfileDto } from "../dtos/DTOs/StreamerProfileDto";
import { ChangeMinDonatePriceResponse } from "../dtos/response/ChangeMinDonatePriceResponse";
import { GetMinDonatePriceResponse } from "../dtos/response/GetMinDonatePriceResponse";
import { GetStreamerProfileResponse } from "../dtos/response/GetStreamerProfileResponse";
import { SearchStreamersNicknamesResponse } from "../dtos/response/SearchStreamersNicknamesResponse";
import iocServices from "../utilities/ioc/iocServices";
import { HttpService, MethodType } from "./HttpService";

export interface StreamerService {
     searchStreamersNicknames(nicknameAsFilter: string) : Promise<SearchStreamersNicknamesResponse>;
     getMinDonatePrice(streamerId: string) : Promise<GetMinDonatePriceResponse>;
     getStreamerProfileById(streamerId: string) : Promise<GetStreamerProfileResponse<StreamerProfileDto>>;
     changeMinDonatePrice(streamerId: string, changeOn: number) : Promise<ChangeMinDonatePriceResponse>;
}

@injectable()
export default class DefaultStreamerService implements StreamerService {
     @inject(iocServices.httpService) private readonly httpService!: HttpService;

     private readonly STREAMER_API_ROUTE = process.env.REACT_APP_STREAMER_CONTROLLER_ROUTE;

     public async searchStreamersNicknames(nicknameAsFilter: string) : Promise<SearchStreamersNicknamesResponse> {
          const url = `${this.STREAMER_API_ROUTE}/searchNicknames?nicknameAsFilter=${nicknameAsFilter}`;
          const method = MethodType.GET;
          var result = await this.httpService.send<SearchStreamersNicknamesResponse>(url, method);
          return { ...result.data };
     }

     public async getMinDonatePrice(streamerId: string) : Promise<GetMinDonatePriceResponse> {
          const url = `${this.STREAMER_API_ROUTE}/getMinDonatePriceResponse?streamerId=${streamerId}`;
          const method = MethodType.GET;
          var result = await this.httpService.send<GetMinDonatePriceResponse>(url, method);
          return { ... result.data };
     }

     public async changeMinDonatePrice(streamerId: string, changeOn: number) : Promise<ChangeMinDonatePriceResponse> {
          const url = `${this.STREAMER_API_ROUTE}/changeMinDonatePrice?streamerId=${streamerId}&&changeOn=${changeOn}`;
          const method = MethodType.POST;
          var result = await this.httpService.send<ChangeMinDonatePriceResponse>(url, method);
          return { ... result.data };
     }

     public async getStreamerProfileById(streamerId: string): Promise<GetStreamerProfileResponse<StreamerProfileDto>> {
          const url = `${this.STREAMER_API_ROUTE}/userProfile?streamerId=${streamerId}`;
          const method = MethodType.GET;
          var result = await this.httpService.send<GetStreamerProfileResponse<StreamerProfileDto>>(url, method);
          return { ... result.data };
     }
}