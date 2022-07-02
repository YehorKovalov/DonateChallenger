import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { UserRoleEnum } from "../models/UserRoleEnum";
import AuthStore from "../oidc/AuthStore";
import iocStores from "../utilities/ioc/iocStores";

@injectable()
export default class UserRoleStore {

     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     constructor() {
          makeAutoObservable(this);
     }

     userRole: UserRoleEnum | null = null;

     public continueAsStreamer = () => {
          this.userRole = UserRoleEnum.Streamer;
          this.authStore.signinRedirect();
     }

     public continueAsDonater = () => {
          this.userRole = UserRoleEnum.Donater;
     }
}
