import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeForAddingDto } from "../dtos/DTOs/ChallengeForAddingDto";
import { ChallengesTempStorageService } from "../services/ChallengesTempStorageService";
import iocServices from "../utilities/ioc/iocServices";

@injectable()
export default class ChallengesTempStorageStore {
     @inject(iocServices.challengesTempStorageService) private readonly challengesTempStorageService!: ChallengesTempStorageService;

     constructor() {
          makeAutoObservable(this);
     }

     storageChallenges: ChallengeForAddingDto[] = [];
     challengeForAdding: ChallengeForAddingDto = {
          description: '',
          donateFrom: '',
          donatePrice: 0,
     }

     public getStorage = async (): Promise<void> => {

          const response = await this.challengesTempStorageService.getStorage();
          var storageJson = response.data;
          
          if (storageJson) {
               this.storageChallenges = JSON.parse(storageJson);
          }
     }

     public addChallenge = async (): Promise<boolean> => {

          if (!this.challengeForAddingStateIsValid()) {
               console.log("Challenge for adding state is not valid");
               return false;
          }

          this.storageChallenges.unshift(this.challengeForAdding);
          return await this.updateStorage();
     }

     public updateStorage = async (): Promise<boolean> => {
          const storageChallengesJson = JSON.stringify(this.storageChallenges); 
          const response = await this.challengesTempStorageService.updateStorage(storageChallengesJson);
          if (!response) {
               console.log("Storage's not updated. Something went wrong");
               return false;
          }

          return true;
     }

     public getDonationsSumPrice = (): number => {
          let sum = 0;
          this.storageChallenges.forEach(s => sum += s.donatePrice);
          return sum;
     }

     private challengeForAddingStateIsValid = (): boolean => {
          const logicalPriceMinimum = 0.1;
          const state = this.challengeForAdding;
          return state.description.length > 0
               && state.donateFrom.length > 0
               && state.donatePrice > logicalPriceMinimum;
     }
}