import { inject, injectable } from "inversify";
import { GetChallengesTemporaryStorageResponse } from "../dtos/response/GetChallengesTemporaryStorageResponse";
import { UpdateChallengesTemporaryStorageRequest } from "../dtos/response/UpdateChallengesTemporaryStorageRequest";
import AuthStore from "../oidc/AuthStore";
import iocServices from "../utilities/ioc/iocServices";
import iocStores from "../utilities/ioc/iocStores";
import { HttpService, MethodType } from "./HttpService";

export interface ChallengesTempStorageService {
     getStorage(): Promise<GetChallengesTemporaryStorageResponse<string>>;
     updateStorage(dataJson: string): Promise<UpdateChallengesTemporaryStorageRequest<string>>;
}

@injectable()
export default class DefaultChallengesTempStorageService implements ChallengesTempStorageService {
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     @inject(iocServices.httpService) private readonly httpService!: HttpService;

     private readonly challengesTempStorageRoute = process.env.REACT_APP_CHALLENGES_TEMP_STORAGE_CONTROLLER_ROUTE;

     public async getStorage(): Promise<GetChallengesTemporaryStorageResponse<string>> {
          const headers = await this.authStore.getAuthorizedHeaders();
          var response = await this.httpService.send<GetChallengesTemporaryStorageResponse<string>>(`${this.challengesTempStorageRoute}/get`, MethodType.POST, headers);
          return { ...response.data };
     }

     public async updateStorage(dataJson: string): Promise<UpdateChallengesTemporaryStorageRequest<string>> {
          const headers = await this.authStore.getAuthorizedHeaders();
          var response = await this.httpService.send<UpdateChallengesTemporaryStorageRequest<string>>(`${this.challengesTempStorageRoute}/update`, MethodType.POST, headers);
          return { ...response.data };
     }
}