import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { InputWithValidation } from "../../models/InputWithValidation";
import { UserProfile } from "../../models/UserProfile";
import AuthStore from "../../oidc/AuthStore";
import { UserService } from "../../services/UserService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import InputValidationStore from "../components/InputValidationStore";

@injectable()
export default class UserProfileStore {

     @inject(iocServices.userService) private readonly userService!: UserService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     @inject(iocStores.inputValidationStore) private readonly inputValidation!: InputValidationStore;

     private emptyString = '';

     constructor() {
          this.profile = {
               email: "",
               userId: "",
               userNickname: ""
          };

          this.nicknameInput = {
               errors: [],
               state: ''
          }

          makeAutoObservable(this);
     }

     profile: UserProfile = null!;
     emailInput: InputWithValidation<string> = null!;
     nicknameInput: InputWithValidation<string> = null!;

     public getUserProfile = async () => {

          if (this.authStore.user) {
               const response = await this.userService.getUserProfileById(this.authStore.user.profile.sub);
               this.profile = response.data;
          }
     }

     public changeNickname = async () : Promise<boolean> => {
          const [succeeded, errors, changedData] = await this.inputValidation.changeProfileField(this.nicknameInput, this.profile.userNickname,
               async () => await this.userService.changeNickname(this.authStore.user!.profile.sub, this.nicknameInput.state));

          if (succeeded) {
               this.profile.userNickname = changedData;
               this.nicknameInput.state = this.emptyString;
               return true;
          }

          this.nicknameInput.errors = errors;

          return false;
     }

      
     public changeEmail = async () : Promise<boolean> => {

          const [succeeded, errors, changedData] = await this.inputValidation.changeProfileField(this.emailInput, this.profile.email,
               async () => await this.userService.changeEmail(this.authStore.user!.profile.sub, this.emailInput.state));

          if (succeeded) {
               this.profile.email = changedData;
               this.emailInput.state = this.emptyString;
               this.emailInput.errors = [];
               return true;
          }

          this.emailInput.errors = errors;

          return false;
     }
}