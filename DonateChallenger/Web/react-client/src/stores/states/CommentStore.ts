import { injectable } from "inversify";
import { makeAutoObservable } from "mobx";

@injectable()
export default class CommentStore {
     constructor() {
          makeAutoObservable(this);
     }

     message = "";
     currentChallengeId = 0;
     userId = "";
}