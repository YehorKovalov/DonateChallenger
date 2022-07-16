import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeStatusEnum } from "../../models/ChallengeStatusEnum";
import iocStores from "../../utilities/ioc/iocStores";
import { formPages } from "../../utilities/PagesProvider";
import ChallengesBoardFiltersStore from "../components/ChallengesBoardFiltersStore";
import ChallengesBoardPaginationStore from "../components/ChallengesBoardPaginationStore";
import ChallengesStore from "../states/ChallengesStore";

@injectable()
export default class ChallengesBoardStore {
     @inject(iocStores.challengesStore) private readonly challengesStore!: ChallengesStore;
     @inject(iocStores.challengesBoardFiltersStore) private readonly filters!: ChallengesBoardFiltersStore;
     @inject(iocStores.challengesBoardPaginationStore) private readonly pagination!: ChallengesBoardPaginationStore;

     constructor() {
          makeAutoObservable(this);
     }

     currentChallengeStatus: ChallengeStatusEnum = ChallengeStatusEnum.Current;

     public getBoardTitle = () => `${this.currentChallengeStatus} Challenges`;

     public getChallengesByCurrentStatus = async () => {

          const pagination = this.pagination;
          const filters = this.filters;

          await this.challengesStore.getChallenges(this.currentChallengeStatus, pagination.currentPage, filters.sortByCreatedTime, filters.sortByMinDonatePrice, filters.minPriceFilter);
          
          pagination.pagesAmount = this.challengesStore.paginatedChallenges!.totalPages;
          this.pagination.buttons = formPages(this.challengesStore.paginatedChallenges!.totalPages)
     }
}