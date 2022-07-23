import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { InputWithValidation } from "../../models/InputWithValidation";
import { StreamerProfile } from "../../models/StreamerProfile";
import AuthStore from "../../oidc/AuthStore";
import { StreamerService } from "../../services/StreamerService";
import { UserService } from "../../services/UserService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import InputValidationStore from "../components/InputValidationStore";

@injectable()
export default class StreamerProfileStore {

     @inject(iocServices.streamerService) private readonly streamerService!: StreamerService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     @inject(iocServices.userService) private readonly userService!: UserService;
     @inject(iocStores.inputValidationStore) private readonly inputValidation!: InputValidationStore;

     private emptyString = '';
     constructor() {
          this.profile = {
               streamerId: '',
               streamerNickname: '',
               minDonatePrice: 0,
               merchantId: '',
               email: ''
          };

          this.nicknameInput = {
               errors: [],
               state: ''
          }

          this.minDonatePriceInput = {
               errors: [],
               state: 0
          }

          this.emailInput = {
               errors: [],
               state: ''
          }

          makeAutoObservable(this);
     }

     profile: StreamerProfile = null!;
     emailInput: InputWithValidation<string> = null!;
     nicknameInput: InputWithValidation<string> = null!;
     minDonatePriceInput: InputWithValidation<number> = null!;

     public getStreamerProfile = async () => {

          if (this.authStore.user) {
               const response = await this.streamerService.getStreamerProfileById(this.authStore.user.profile.sub);
               this.profile = response.data;
          }
     }

     public changeMinDonatePrice = async () : Promise<boolean> => {

          const [succeeded, errors, changedData] = await this.inputValidation.changeProfileField(this.minDonatePriceInput, this.profile.minDonatePrice,
               async () => await this.streamerService.changeMinDonatePrice(this.authStore.user!.profile.sub, this.minDonatePriceInput.state));

          if (succeeded) {
               this.profile.minDonatePrice = changedData;
               this.minDonatePriceInput.state = 0;
               this.minDonatePriceInput.errors = [];
               return true;
          }

          this.minDonatePriceInput.errors = errors;

          return false;
     }

     public changeNickname = async () : Promise<boolean> => {

          const [succeeded, errors, changedData] = await this.inputValidation.changeProfileField(this.nicknameInput, this.profile.streamerNickname,
               async () => await this.userService.changeNickname(this.authStore.user!.profile.sub, this.nicknameInput.state));

          if (succeeded) {
               this.profile.streamerNickname = changedData;
               this.nicknameInput.state = this.emptyString;
               this.nicknameInput.errors = [];
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