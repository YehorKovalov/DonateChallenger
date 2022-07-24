import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import AuthStore from "../../oidc/AuthStore";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengeOrderManagerStore from "../containers/ChallengeOrderManagerStore";

@injectable()
export default class AppManagerPageStore {

     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     @inject(iocStores.challengeOrderManagerStore) private readonly challengeOrderManager!: ChallengeOrderManagerStore;

     constructor() {
          makeAutoObservable(this);
     }

     activeKey = '';
     public handleOnSelect = async (tabName: string | null) => {
          switch (tabName) {
               case "challenges":
                    this.activeKey = "challenges";
                    break;
               case "challenge-orders":
                    this.activeKey = "challenge-orders";
                    await this.challengeOrderManager.getPaginated();
                    break;
          }
     }
}
