import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { CurrentChallenge } from "../../models/CurrentChallenge";
import { PaginatedChallenges } from "../../models/PaginatedChallenges";
import { ChallengeService } from "../../services/ChallengeService";
import challengesConstants from "../../utilities/ChallengesConstants";
import iocServices from "../../utilities/ioc/iocServices";

@injectable()
export default class ChallengesStore {
     
     @inject(iocServices.challengeService) private readonly challengeService!: ChallengeService;
     private readonly challengesPerPage: number = challengesConstants.APP_CHALLENGES_PER_PAGE;

     constructor() {
          makeAutoObservable(this);
     }

     currentPage = 0;
     sortByCreatedTime?: boolean | undefined = undefined;
     minPriceFilter?: number | undefined = undefined;
     paginatedChallenges: PaginatedChallenges<CurrentChallenge> | null = null;

     public getPaginatedCurrentChallenges = async () => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedCurrentChallenges(this.currentPage, this.challengesPerPage, this.sortByCreatedTime, this.minPriceFilter);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }

     public getPaginatedSkippedChallenges = async () => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedSkippedChallenges(this.currentPage, this.challengesPerPage, this.sortByCreatedTime, this.minPriceFilter);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }

     public getPaginatedCompletedChallenges = async () => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedCompletedChallenges(this.currentPage, this.challengesPerPage, this.sortByCreatedTime, this.minPriceFilter);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }
}