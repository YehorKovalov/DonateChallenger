import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import moment from "moment";
import { ChallengeCatalogService } from "../../services/ChallengeService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengesStore from "./ChallengesStore";

@injectable()
export default class ChallengeStore {
     
     @inject(iocServices.challengeCatalogService) private readonly challengeService!: ChallengeCatalogService;
     @inject(iocStores.challengesStore) private readonly challengesStore!: ChallengesStore;

     constructor() {
          makeAutoObservable(this);
     }

     lastUsedChallengeId: number = 0;

     public skipChallenge = async (challengeId: number) => {
          const isSuccess = await this.challengeService.skipChallengeByChallengeId(challengeId);
          if (isSuccess) {
               this.lastUsedChallengeId = challengeId;
               await this.waitForBluring(async () => {
                    await this.challengesStore.getChallengesByCurrentStatus();
               })
          }
          else {
               alert("sorry something get wrong")
               console.log("skipChallenge ---> challenge's not skipped");
          }
     }

     public completeChallenge = async (challengeId: number) => {
          const isSuccess = await this.challengeService.completeChallengeByChallengeId(challengeId);
          if (isSuccess) {
               this.lastUsedChallengeId = challengeId;
               await this.waitForBluring(async () => {
                    await this.challengesStore.getChallengesByCurrentStatus();
               })
          }
          else {
               alert("sorry something get wrong")
               console.log("skipChallenge ---> challenge's not skipped");
          }
     }

     public getUserFriendlyDateTime = (time: string) => {
          return moment(new Date(time), "YYYYMMDD").fromNow();
     }

     private waitForBluring = async (action: () => Promise<void>) => {
          setTimeout(async () => {
               await action();
          }, 500);
     }
}