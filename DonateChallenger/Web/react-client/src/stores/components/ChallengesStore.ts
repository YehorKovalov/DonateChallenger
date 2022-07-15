import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeStatusEnum } from "../../models/ChallengeStatusEnum";
import { CurrentChallenge } from "../../models/CurrentChallenge";
import { PaginatedChallenges } from "../../models/PaginatedChallenges";
import { ChallengeCatalogService } from "../../services/ChallengeCatalogService";
import challengesConstants from "../../utilities/ChallengesConstants";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import { formPages } from "../../utilities/PagesProvider";
import ChallengesBoardStore from "./ChallengesBoardStore";

@injectable()
export default class ChallengesStore {
     
     @inject(iocServices.challengeCatalogService) private readonly challengeService!: ChallengeCatalogService;
     @inject(iocStores.challengesBoardStore) private readonly boardStore!: ChallengesBoardStore;
     private readonly challengesPerPage: number = challengesConstants.APP_CHALLENGES_PER_PAGE;

     constructor() {
          makeAutoObservable(this);
     }


     paginatedChallenges: PaginatedChallenges<CurrentChallenge> | null = null;
     currentChallengeStatus: ChallengeStatusEnum = ChallengeStatusEnum.Current;

     public getChallengesByStatus = async (status: ChallengeStatusEnum) => {

          if (status === ChallengeStatusEnum.Current) {
               await this.getPaginatedCurrentChallenges();
          }
          else if (status === ChallengeStatusEnum.Completed) {
               await this.getPaginatedCompletedChallenges();
          }
          else if (status === ChallengeStatusEnum.Skipped) {
               await this.getPaginatedSkippedChallenges();
          }

          this.boardStore.buttons = formPages(this.paginatedChallenges!.totalPages)
          this.currentChallengeStatus = status;

          this.boardStore.pagesAmount = this.paginatedChallenges!.totalPages;
     }

     public getChallengesByCurrentStatus = async () => {
          await this.getChallengesByStatus(this.currentChallengeStatus);
     }

     public getBoardTitle = () => {
          return `${this.currentChallengeStatus} Challenges`;
     }

     private getPaginatedCurrentChallenges = async () => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedCurrentChallenges(this.boardStore.currentPage, this.challengesPerPage, this.boardStore.sortByCreatedTime, this.boardStore.minPriceFilter);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }

     private getPaginatedSkippedChallenges = async () => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedSkippedChallenges(this.boardStore.currentPage, this.challengesPerPage, this.boardStore.sortByCreatedTime, this.boardStore.minPriceFilter);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }

     private getPaginatedCompletedChallenges = async () => {
          
          const paginatedCurrentChallenges = await this.challengeService
               .getPaginatedCompletedChallenges(this.boardStore.currentPage, this.challengesPerPage, this.boardStore.sortByCreatedTime, this.boardStore.minPriceFilter);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }
}