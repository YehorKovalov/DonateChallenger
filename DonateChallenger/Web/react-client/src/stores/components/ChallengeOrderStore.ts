import { injectable } from "inversify";
import { makeAutoObservable } from "mobx";

@injectable()
export default class ChallengeOrderStore {

     constructor() {
          makeAutoObservable(this);
     }

     streamerId: string = '';
     title?: string = undefined;
     description: string = '';
     donatePrice: number = 0;
     donateFrom: string = '';
}