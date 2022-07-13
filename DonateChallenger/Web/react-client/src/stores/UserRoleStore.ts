import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { UserRoleEnum } from "../models/UserRoleEnum";
import AuthStore from "../oidc/AuthStore";
import iocStores from "../utilities/ioc/iocStores";
import DonaterStore from "./DonaterStore";

@injectable()
export default class UserRoleStore {

     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     @inject(iocStores.donaterStore) private readonly donaterStore!: DonaterStore;

     constructor() {
          makeAutoObservable(this);
     }

     userRole: UserRoleEnum | null = null;

     public continueAsStreamer = async () => {
          this.userRole = UserRoleEnum.Streamer;
          await this.authStore.signinRedirect();
     }

     public continueAsDonater = () => {
          this.userRole = UserRoleEnum.Donater;
          this.donaterStore.redirectToDonatingPage();
     }
}
