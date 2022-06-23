import { injectable, inject } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeStatusEnum } from "../../models/ChallengeStatusEnum";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengesStore from "./ChallengesStore";

@injectable()
export default class ChallengerBoardStore {

     @inject(iocStores.challengesStore) private readonly challengesStore!: ChallengesStore;
     constructor() {
          makeAutoObservable(this);
     }

     currentChallengeStatus: ChallengeStatusEnum = ChallengeStatusEnum.Current;

     public getChallengesByStatus = async (status: ChallengeStatusEnum) => {
               this.currentChallengeStatus = status;
               if (status === ChallengeStatusEnum.Current) {
                    await this.challengesStore.getPaginatedCurrentChallenges();
               }
               else if (status === ChallengeStatusEnum.Completed) {
                    await this.challengesStore.getPaginatedCompletedChallenges();
               }
               else if (status === ChallengeStatusEnum.Skipped) {
                    await this.challengesStore.getPaginatedSkippedChallenges();
               }
     }

     public getChallengesByCurrentStatus = async () => {
          await this.getChallengesByStatus(this.currentChallengeStatus);
     }

     public getBoardTitle = () => {
          return `${this.currentChallengeStatus} Challenges`;
     }
}