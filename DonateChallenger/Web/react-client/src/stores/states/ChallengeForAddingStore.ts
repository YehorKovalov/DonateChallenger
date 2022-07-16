import { injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeForAddingDto } from "../../dtos/DTOs/ChallengeForAddingDto";
import { InputWithValidation } from "../../models/InputWithValidation";

@injectable()
export default class ChallengeForAddingStore {

     constructor() {
          makeAutoObservable(this);
     }

     challengeForAdding: ChallengeForAddingDto = {
          description: '',
          donateFrom: '',
          streamerId: '',
          donatePrice: 0
     }

     title: InputWithValidation<string> = {
          errors: [],
          state: ''
     };
     description: InputWithValidation<string> = {
          errors: [],
          state: ''
     };
     donateFrom: InputWithValidation<string> = {
          errors: [],
          state: ''
     };
     donatePrice: InputWithValidation<number> = {
          errors: [],
          state: 0
     };
}