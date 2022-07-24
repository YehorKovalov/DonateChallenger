import { inject, injectable } from "inversify";
import { ChallengeDto } from "../dtos/DTOs/ChallengeDto";
import { CompletedChallengeDto } from "../dtos/DTOs/CompletedChallengeDto";
import { CurrentChallengeDto } from "../dtos/DTOs/CurrentChallengeDto";
import { SkippedChallengeDto } from "../dtos/DTOs/SkippedChallengeDto";
import { AddChallengeForStreamerRequest } from "../dtos/requests/AddChallengeForStreamerRequest";
import { GetPaginatedStreamerChallengesRequest } from "../dtos/requests/GetPaginatedStreamerChallengesRequest";
import { UpdateChallengeRequest } from "../dtos/requests/UpdateChallengeRequest";
import { AddChallengeForStreamerResponse } from "../dtos/response/AddChallengeForStreamerResponse";
import { GetChallengeByIdResponse } from "../dtos/response/GetChallengeByIdResponse";
import { GetPaginatedStreamerChallengesResponse } from "../dtos/response/GetPaginatedStreamerChallengesResponse";
import { UpdateChallengeResponse } from "../dtos/response/UpdateChallengeResponse";
import AuthStore from "../oidc/AuthStore";
import iocServices from "../utilities/ioc/iocServices";
import iocStores from "../utilities/ioc/iocStores";
import { ChallengeCatalogService } from "./ChallengeCatalogService";
import { HttpService, MethodType } from "./HttpService";

export interface CatalogChallengeManagerService {
     getPaginatedCurrentChallenges(currentPage: number, challengesPerPage: number, streamerId?: string): Promise<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>>;
     getPaginatedCompletedChallenges(currentPage: number, challengesPerPage: number, streamerId?: string): Promise<GetPaginatedStreamerChallengesResponse<CompletedChallengeDto>>;
     getPaginatedSkippedChallenges(currentPage: number, challengesPerPage: number, streamerId?: string): Promise<GetPaginatedStreamerChallengesResponse<SkippedChallengeDto>>;
     getChallengeById(challengeId: number): Promise<GetChallengeByIdResponse<ChallengeDto>>;
     updateChallenge(challengeId: number, description: string, donatePrice: number, donateFrom: string, title?: string): Promise<UpdateChallengeResponse<number>>;
     addChallenge(description: string, donatePrice: number, donateFrom: string, streamerId: string, title?: string): Promise<AddChallengeForStreamerResponse>;
}

@injectable()
export default class DefaultCatalogChallengeManagerService implements CatalogChallengeManagerService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;
     @inject(iocServices.challengeCatalogService) private readonly challengeCatalogService!: ChallengeCatalogService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     private readonly challengeCatalogManagerApiRoute = process.env.REACT_APP_CHALLENGE_CATALOG_MANAGER_CONTROLLER_ROUTE;

     public async getChallengeById(challengeId: number): Promise<GetChallengeByIdResponse<ChallengeDto>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.challengeCatalogManagerApiRoute}/get?challengeId=${challengeId}`;

          var response = await this.httpService.send<GetChallengeByIdResponse<ChallengeDto>>(url, MethodType.GET, headers);

          return { ...response.data };
     }

     public async updateChallenge(challengeId: number, description: string, donatePrice: number, donateFrom: string, title?: string | undefined): Promise<UpdateChallengeResponse<number>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.challengeCatalogManagerApiRoute}/update`;
          const request: UpdateChallengeRequest = {
               challengeId: challengeId,
               description: description,
               donateFrom: donateFrom,
               donatePrice: donatePrice,
               title: title
          }

          var response = await this.httpService.send<UpdateChallengeResponse<number>>(url, MethodType.POST, headers, request);
          return { ...response.data };
     }

     public async addChallenge(description: string, donatePrice: number, donateFrom: string, streamerId: string, title?: string | undefined): Promise<AddChallengeForStreamerResponse> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.challengeCatalogManagerApiRoute}/add`;
          const request: AddChallengeForStreamerRequest = {
               description: description,
               donateFrom: donateFrom,
               donatePrice: donatePrice,
               streamerId: streamerId,
               title: title
          }

          var response = await this.httpService.send<AddChallengeForStreamerResponse>(url, MethodType.POST, headers, request);
          return { ...response.data };
     }

     public async getPaginatedCurrentChallenges(currentPage: number, challengesPerPage: number, streamerId: string): Promise<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>> {

          const url = `${this.challengeCatalogManagerApiRoute}/current`;
          const result = await this.getPaginatedChallengesInternal<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, streamerId);

          return { ...result }
     }

     public async getPaginatedCompletedChallenges(currentPage: number, challengesPerPage: number, streamerId: string): Promise<GetPaginatedStreamerChallengesResponse<CompletedChallengeDto>> {
          const url = `${this.challengeCatalogManagerApiRoute}/completed`;
          const result = await this.getPaginatedChallengesInternal<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, streamerId);

          return { ...result }
     }

     public async getPaginatedSkippedChallenges(currentPage: number, challengesPerPage: number, streamerId: string): Promise<GetPaginatedStreamerChallengesResponse<SkippedChallengeDto>> {

          const url = `${this.challengeCatalogManagerApiRoute}/skipped`;
          const result = await this.getPaginatedChallengesInternal<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, streamerId);

          return { ...result }
     }

     private async getPaginatedChallengesInternal<T>(url: string, currentPage: number, challengesPerPage: number, streamerId: string)
          : Promise<T> {

          if (!this.authStore.user) {
               await this.authStore.tryGetUser();
          }

          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const method = MethodType.POST;

          const request: GetPaginatedStreamerChallengesRequest = {
               currentPage: currentPage,
               streamerId: streamerId,
               challengesPerPage: challengesPerPage,
          };

          const response = await this.httpService.send<T>(url, method, headers, request);

          return { ...response.data };
     }
}