import { injectable, inject } from "inversify";
import { SkippedChallengeDto } from "../dtos/DTOs/SkippedChallengeDto";
import { CurrentChallengeDto } from "../dtos/DTOs/CurrentChallengeDto";
import { CompletedChallengeDto } from "../dtos/DTOs/CompletedChallengeDto";
import { AddChallengeForStreamerRequest } from "../dtos/requests/AddChallengeForStreamerRequest";
import { GetPaginatedStreamerChallengesRequest } from "../dtos/requests/GetPaginatedStreamerChallengesRequest";
import { AddChallengeForStreamerResponse } from "../dtos/response/AddChallengeForStreamerResponse";
import { GetPaginatedStreamerChallengesResponse as GetPaginatedChallengesResponse } from "../dtos/response/GetPaginatedStreamerChallengesResponse";
import { ChallengesBoardFilter } from "../models/ChallengesBoardFilter";
import iocServices from "../utilities/ioc/iocServices";
import { ApiHeader, ContentType, HttpService, MethodType } from "./HttpService";

export interface ChallengeService {
     addChallenge(description: string, donatePrice: number, donateFrom: string, title?: string)
          : Promise<AddChallengeForStreamerResponse>;
     getPaginatedCurrentChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedChallengesResponse<CurrentChallengeDto>>;
     getPaginatedCompletedChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, minPriceFilter?: number)
     : Promise<GetPaginatedChallengesResponse<CompletedChallengeDto>>;
     getPaginatedSkippedChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedChallengesResponse<SkippedChallengeDto>>;
     skipChallengeByChallengeId(challengeId: number) : Promise<boolean>;
     completeChallengeByChallengeId(challengeId: number) : Promise<boolean>;
}

@injectable()
export default class DefaultChallengeService implements ChallengeService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;
     private readonly CHALLENGE_BOARD_ROUTE = process.env.REACT_APP_CHALLENGES_BOARD_CONTROLLER_ROUTE;
     
     public async addChallenge(description: string, donatePrice: number, donateFrom: string, title?: string): Promise<AddChallengeForStreamerResponse> {
          
          const request: AddChallengeForStreamerRequest<number> = {
               title: title,
               description: description,
               donateFrom: donateFrom,
               donatePrice: donatePrice,
          }
          
          const url = `${this.CHALLENGE_BOARD_ROUTE}/add`;
          const headers: ApiHeader = {
               contentType: ContentType.Json
          }
          const response = await this.httpService.send<AddChallengeForStreamerResponse>(url, MethodType.POST, headers, request);

          return { ...response.data };
     }

     public async getPaginatedCurrentChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedChallengesResponse<CurrentChallengeDto>> {

          
          const url = `${this.CHALLENGE_BOARD_ROUTE}/current`;
          const response = await this.getPaginatedChallengesInternal<GetPaginatedChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, sortByCreatedTime, minPriceFilter);

          return { ...response };
     }


     public async getPaginatedCompletedChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedChallengesResponse<CompletedChallengeDto>> {

          const url = `${this.CHALLENGE_BOARD_ROUTE}/completed`;
          const response = await this.getPaginatedChallengesInternal<GetPaginatedChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, sortByCreatedTime, minPriceFilter);

          return { ...response };
     }

     public async getPaginatedSkippedChallenges(currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, minPriceFilter?: number)
          : Promise<GetPaginatedChallengesResponse<SkippedChallengeDto>> {

          const url = `${this.CHALLENGE_BOARD_ROUTE}/skipped`;
          const response = await this.getPaginatedChallengesInternal<GetPaginatedChallengesResponse<CurrentChallengeDto>>(url, currentPage, challengesPerPage, sortByCreatedTime, minPriceFilter);

          return { ...response };
     }
     
     public async skipChallengeByChallengeId(challengeId: number) : Promise<boolean> {
          const url = `${this.CHALLENGE_BOARD_ROUTE}/skip?challengeid=${challengeId}`;
          const method = MethodType.POST;
          const response = await this.httpService.send<boolean>(url, method);

          return response.data;
     }

     public async completeChallengeByChallengeId(challengeId: number) : Promise<boolean> {
          const url = `${this.CHALLENGE_BOARD_ROUTE}/complete?challengeid=${challengeId}`;
          const method = MethodType.POST;

          const response = await this.httpService.send<boolean>(url, method);

          return response.data;
     }

     private async getPaginatedChallengesInternal<T>(url: string, currentPage: number, challengesPerPage: number, sortByCreatedTime?: boolean, minPriceFilter?: number)
          : Promise<T> {

          const headers = { contentType: ContentType.Json };
          const method = MethodType.POST;

          const filters = this.handleFilters(sortByCreatedTime, minPriceFilter);
          const request: GetPaginatedStreamerChallengesRequest = {
               currentPage: currentPage,
               challengesPerPage: challengesPerPage,
               filters: filters
          };

          const response = await this.httpService.send<T>(url, method, headers, request);

          return { ...response.data };
     }

     private handleFilters = (sortByCreatedTime?: boolean, minPriceFilter?: number) => {
          let filters = new Map<ChallengesBoardFilter, number>();

          if (sortByCreatedTime) {
               filters.set(ChallengesBoardFilter.SortByCreatedTime, 1);
          }

          if (minPriceFilter) {
               filters.set(ChallengesBoardFilter.MinPriceFilter, minPriceFilter)
          }

          return Object.fromEntries(filters);
     }

     private decreaseCurrentPageValue = (currentPage: number): number => {
          return currentPage === 0 ? currentPage : --currentPage;
     }
}