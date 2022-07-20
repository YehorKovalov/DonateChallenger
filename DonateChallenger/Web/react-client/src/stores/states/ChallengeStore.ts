import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeCatalogService } from "../../services/ChallengeCatalogService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengesBoardStore from "../containers/ChallengesBoardStore";

@injectable()
export default class ChallengeStore {
     
     @inject(iocServices.challengeCatalogService) private readonly challengeService!: ChallengeCatalogService;
     @inject(iocStores.challengesBoardStore) private readonly challengesBoardStore!: ChallengesBoardStore;

     constructor() {
          makeAutoObservable(this);
     }

     lastUsedChallengeId: number = 0;

     public skipChallenge = async (challengeId: number) => {

          const isSuccess = await this.challengeService.skipChallengeByChallengeId(challengeId);
          if (isSuccess) {
               this.lastUsedChallengeId = challengeId;
               await this.waitForBluring(async () => {
                    await this.challengesBoardStore.getChallengesByCurrentStatus();
               })
          }
          else {
               alert("sorry, something get wrong")
               console.log("skipChallenge ---> challenge's not skipped");
          }
     }

     public completeChallenge = async (challengeId: number) => {

          const isSuccess = await this.challengeService.completeChallengeByChallengeId(challengeId);
          if (isSuccess) {
               this.lastUsedChallengeId = challengeId;
               await this.waitForBluring(async () => {
                    await this.challengesBoardStore.getChallengesByCurrentStatus();
               })
          }
          else {
               alert("sorry something get wrong")
               console.log("skipChallenge ---> challenge's not skipped");
          }
     }

     private waitForBluring = async (action: () => Promise<void>) => {
          setTimeout(async () => {
               await action();
          }, 500);
     }
}