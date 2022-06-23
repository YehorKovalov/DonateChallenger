import { injectable } from "inversify";
import { makeAutoObservable } from "mobx";

@injectable()
export default class AuthStore {

     constructor() {
          makeAutoObservable(this);
     }

     streamerAuthenticated = true;
}