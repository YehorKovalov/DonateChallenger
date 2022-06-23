import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import moment from "moment";
import { ChallengeService } from "../../services/ChallengeService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengerBoardStore from "./ChallengerBoardStore";

@injectable()
export default class ChallengeStore {
     
     @inject(iocServices.challengeService) private readonly challengeService!: ChallengeService;
     @inject(iocStores.boardsStore) private readonly boardsStore!: ChallengerBoardStore;

     constructor() {
          makeAutoObservable(this);
     }

     lastUsedChallengeId: number = 0;

     public skipChallenge = async (challengeId: number) => {
          const isSuccess = await this.challengeService.skipChallengeByChallengeId(challengeId);
          if (isSuccess) {
               this.lastUsedChallengeId = challengeId;
               this.waitForBluring(() => {
                    this.boardsStore.getChallengesByCurrentStatus();
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
               this.waitForBluring(() => {
                    this.boardsStore.getChallengesByCurrentStatus();
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

     private waitForBluring = (action: () => void) => {
          setTimeout(() => {
               action();
          }, 500);
     }
}