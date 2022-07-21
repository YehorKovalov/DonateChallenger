import { injectable } from "inversify";
import { makeAutoObservable } from "mobx";

@injectable()
export default class ChallengesBoardPaginationStore {

     constructor() {
          makeAutoObservable(this);
     }

     currentPage = 0;
     pagesAmount = 0;
     buttons: number[] = [];

     public changePageOnNext = () => {
          this.changePageOn(this.currentPage + 1);
     }

     public changePageOnPrevious = () => {
          this.changePageOn(this.currentPage - 1);
     }
     
     public changePageOn = (pageNumber: number) => {
          if ((pageNumber >= this.pagesAmount) || pageNumber < 0) {
               return;
          }

          this.currentPage = pageNumber;
     }
}