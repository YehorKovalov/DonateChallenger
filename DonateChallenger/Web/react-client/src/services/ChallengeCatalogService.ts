import { injectable, inject } from "inversify";
import { SkippedChallengeDto } from "../dtos/DTOs/SkippedChallengeDto";
import { CurrentChallengeDto } from "../dtos/DTOs/CurrentChallengeDto";
import { CompletedChallengeDto } from "../dtos/DTOs/CompletedChallengeDto";
import { GetPaginatedStreamerChallengesRequest } from "../dtos/requests/GetPaginatedStreamerChallengesRequest";
import { ChallengesBoardFilter } from "../models/ChallengesBoardFilter";
import iocServices from "../utilities/ioc/iocServices";
import { ApiHeader, ContentType, HttpService, MethodType } from "./HttpService";
import AuthStore from "../oidc/AuthStore";
import iocStores from "../utilities/ioc/iocStores";
import { GetPaginatedStreamerChallengesResponse } from "../dtos/response/GetPaginatedStreamerChallengesResponse";
import { SortChallengeBy } from "../models/ChallengeSortByEnum";

export interface ChallengeCatalogService {
     getPaginatedCurrentChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>>;
     getPaginatedCompletedChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number)
     : Promise<GetPaginatedStreamerChallengesResponse<CompletedChallengeDto>>;
     getPaginatedSkippedChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedStreamerChallengesResponse<SkippedChallengeDto>>;
     skipChallengeByChallengeId(challengeId: number) : Promise<boolean>;
     completeChallengeByChallengeId(challengeId: number) : Promise<boolean>;
}

@injectable()
export default class DefaultChallengeCatalogService implements ChallengeCatalogService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     private readonly ChallengeBoardApiRoute = process.env.REACT_APP_CHALLENGES_BOARD_CONTROLLER_ROUTE;
     
     public async getPaginatedCurrentChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>> {
          
          const url = `${this.ChallengeBoardApiRoute}/current`;
          const response = await this.getPaginatedChallengesInternal<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter);

          return { ...response };
     }


     public async getPaginatedCompletedChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedStreamerChallengesResponse<CompletedChallengeDto>> {

          const url = `${this.ChallengeBoardApiRoute}/completed`;
          const response = await this.getPaginatedChallengesInternal<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter);

          return { ...response };
     }

     public async getPaginatedSkippedChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedStreamerChallengesResponse<SkippedChallengeDto>> {

          const url = `${this.ChallengeBoardApiRoute}/skipped`;
          const response = await this.getPaginatedChallengesInternal<GetPaginatedStreamerChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter);

          return { ...response };
     }
     
     public async skipChallengeByChallengeId(challengeId: number) : Promise<boolean> {
          const url = `${this.ChallengeBoardApiRoute}/skip?challengeid=${challengeId}`;
          const method = MethodType.POST;
          const headers = await this.authStore.tryGetAuthorizedHeaders();

          const response = await this.httpService.send<boolean>(url, method, headers);
          
          return response.data;
     }

     public async completeChallengeByChallengeId(challengeId: number) : Promise<boolean> {
          const url = `${this.ChallengeBoardApiRoute}/complete?challengeid=${challengeId}`;
          const method = MethodType.POST;
          const headers = await this.authStore.tryGetAuthorizedHeaders();

          const response = await this.httpService.send<boolean>(url, method, headers);

          return response.data;
     }

     private async getPaginatedChallengesInternal<T>(url: string, currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number)
          : Promise<T> {

          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const method = MethodType.POST;

          const filters = this.handleFilters(minPriceFilter);
          const sorting = this.handleSorting(sortByCreatedTime);
          const request: GetPaginatedStreamerChallengesRequest = {
               currentPage: currentPage,
               streamerId: this.authStore.user!.profile.sub,
               challengesPerPage: challengesPerPage,
               filters: filters,
               sortBy: sorting
          };

          const response = await this.httpService.send<T>(url, method, headers, request);

          return { ...response.data };
     }

     private handleFilters = (minPriceFilter?: number) => {
          let filters = new Map<ChallengesBoardFilter, number>();

          if (minPriceFilter) {
               filters.set(ChallengesBoardFilter.MinPriceFilter, minPriceFilter)
          }

          return Object.fromEntries(filters);
     }

     private handleSorting = (sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean) => {
          let filters = new Map<SortChallengeBy, boolean>();

          if (sortByCreatedTime) {
               filters.set(SortChallengeBy.CreatedTime, true);
          }

          if (sortByMinDonatePrice) {
               filters.set(SortChallengeBy.MinDonatePrice, true);
          }

          return Object.fromEntries(filters);
     }
}