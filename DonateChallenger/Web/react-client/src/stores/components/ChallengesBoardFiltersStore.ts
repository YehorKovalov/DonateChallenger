import { injectable } from "inversify";
import { makeAutoObservable } from "mobx";

@injectable()
export default class ChallengesBoardFiltersStore {

     constructor() {
          makeAutoObservable(this);
     }

     sortByCreatedTime: boolean = false;
     sortByMinDonatePrice: boolean = false;
     minPriceFilter?: number | undefined = undefined;
}