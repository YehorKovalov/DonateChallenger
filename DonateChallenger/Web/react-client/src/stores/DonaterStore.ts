import { injectable } from "inversify";
import { makeAutoObservable } from "mobx";

@injectable()
export default class DonaterStore {

     constructor() {
          makeAutoObservable(this);          
     }

     public redirectToDonatingPage = () => {
          location.replace("/order")
     }
}