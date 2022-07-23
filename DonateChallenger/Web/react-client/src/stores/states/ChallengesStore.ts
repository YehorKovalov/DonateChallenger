import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeStatusEnum } from "../../models/ChallengeStatusEnum";
import { CurrentChallenge } from "../../models/CurrentChallenge";
import { PaginatedChallenges } from "../../models/PaginatedChallenges";
import { ChallengeCatalogService } from "../../services/ChallengeCatalogService";
import challengesConstants from "../../utilities/ChallengesConstants";
import iocServices from "../../utilities/ioc/iocServices";

@injectable()
export default class ChallengesStore {
     
     @inject(iocServices.challengeCatalogService) private readonly challengeService!: ChallengeCatalogService;
     private readonly challengesPerPage: number = challengesConstants.APP_CHALLENGES_PER_PAGE;

     constructor() {
          makeAutoObservable(this);
     }

     paginatedChallenges: PaginatedChallenges<CurrentChallenge> | null = null;

     public getChallenges = async (status: ChallengeStatusEnum, currentPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number, streamerId?: string) => {
          switch (status) {
               case ChallengeStatusEnum.Current:
                    await this.getPaginatedCurrentChallenges(currentPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter, streamerId);
                    break;
               case ChallengeStatusEnum.Completed:
                    await this.getPaginatedCompletedChallenges(currentPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter, streamerId);
                    break;
               case ChallengeStatusEnum.Skipped:
                    await this.getPaginatedSkippedChallenges(currentPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter, streamerId);
                    break;
          }
     }

     private getPaginatedCurrentChallenges = async (currentPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number, streamerId?: string) => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedCurrentChallenges(currentPage, this.challengesPerPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter, streamerId);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }

     private getPaginatedSkippedChallenges = async (currentPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number, streamerId?: string) => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedSkippedChallenges(currentPage, this.challengesPerPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter, streamerId);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }

     private getPaginatedCompletedChallenges = async (currentPage: number, sortByCreatedTime?: boolean, sortByMinDonatePrice?: boolean, minPriceFilter?: number, streamerId?: string) => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedCompletedChallenges(currentPage, this.challengesPerPage, sortByCreatedTime, sortByMinDonatePrice, minPriceFilter, streamerId);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }
}