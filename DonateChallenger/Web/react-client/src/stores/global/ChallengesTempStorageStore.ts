import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeForAddingDto } from "../../dtos/DTOs/ChallengeForAddingDto";
import { ChallengesTempStorageService } from "../../services/ChallengesTempStorageService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengeForAddingStore from "../states/ChallengeForAddingStore";

@injectable()
export default class ChallengesTempStorageStore {

     @inject(iocServices.challengesTempStorageService) private readonly challengesTempStorageService!: ChallengesTempStorageService;
     @inject(iocStores.challengeForAddingStore) private readonly challengeForAddingStore!: ChallengeForAddingStore;

     constructor() {
          makeAutoObservable(this);
     }

     storageChallenges: ChallengeForAddingDto[] = [];

     public getStorage = async (): Promise<void> => {

          const response = await this.challengesTempStorageService.getStorage();
          var storageJson = response.data;
          
          if (storageJson) {
               this.storageChallenges = JSON.parse(storageJson);
          }
     }

     public addChallengeToStorage = async (streamerId: string): Promise<boolean> => {

          if (!this.challengeForAddingStateIsValid(streamerId)) {
               return false;
          }
          
          const challenge: ChallengeForAddingDto = {
               streamerId: streamerId,
               title: this.challengeForAddingStore.title.state,
               description: this.challengeForAddingStore.description.state,
               donateFrom: this.challengeForAddingStore.donateFrom.state,
               donatePrice: this.challengeForAddingStore.donatePrice.state
          }
          this.storageChallenges.unshift(challenge);

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

     private challengeForAddingStateIsValid = (streamerId: string): boolean => {

          const logicalPriceMinimum = 0.1;
          const descriptionStateIsValid = this.challengeForAddingStore.description.state.length > 0;
          if (!descriptionStateIsValid) {
               console.log("description");
               console.log(this.challengeForAddingStore.description.state);
               return false;
          }
          const streamerIdStateIsValid = streamerId.length > 0;


          if (!streamerIdStateIsValid) {
               console.log("streamerId");
               return false;
          }

          const donateFromStateIsValid = this.challengeForAddingStore.donateFrom.state.length > 0;

          if (!donateFromStateIsValid) {
               console.log("donate");
               return false;
          }

          const donatePriceStateIsValid = this.challengeForAddingStore.donatePrice.state > logicalPriceMinimum;

          if (!donatePriceStateIsValid) {
               console.log("donatePrice");
               return false;
          }

          return true;
     }
}