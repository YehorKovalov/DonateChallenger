import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import AuthStore from "../../oidc/AuthStore";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class SelectUserRoleStore {

     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     constructor() {
          makeAutoObservable(this);
     }

     public continueAsStreamer = async () => {
          await this.authStore.signinRedirect();
     }

     public continueAsDonater = () => {
          location.replace("/order")
     }
}
