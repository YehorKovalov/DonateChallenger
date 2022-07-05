import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { InputWithValidation } from "../../models/InputWithValidation";
import { StreamerProfile } from "../../models/StreamerProfile";
import AuthStore from "../../oidc/AuthStore";
import { StreamerService } from "../../services/StreamerService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class StreamerProfileStore {
     @inject(iocServices.streamerService) private readonly streamerService!: StreamerService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     private emptyString = '';
     constructor() {
          this.profile = {
               streamerId: '',
               streamerNickname: '',
               minDonatePrice: 0
          };

          this.nicknameInput = {
               errors: [],
               state: ''
          }

          this.minDonatePriceInput = {
               errors: [],
               state: ''
          }

          makeAutoObservable(this);
     }

     profile: StreamerProfile = null!;

     nicknameInput: InputWithValidation<string> = null!;

     minDonatePriceInput: InputWithValidation<string> = null!;

     public getStreamerProfile = async () => {
          await this.getStreamerId();
          var response = await this.streamerService.getStreamerProfileById(this.profile.streamerId);
          this.profile = response.data;
     }

     public changeMinDonatePrice = async () : Promise<boolean> => {
          const minDonatePrice = Number.parseInt(this.minDonatePriceInput.state);
          await this.getStreamerId();
          if (this.authStore.user) {
               if (minDonatePrice === this.profile.minDonatePrice) {
                    return false;
               }

               const response = await this.streamerService.changeMinDonatePrice(this.profile.streamerId, minDonatePrice);
               this.minDonatePriceInput.errors = response.validationErrors

               if (response.succeeded) {
                    this.profile.minDonatePrice = response.changedData;
                    this.minDonatePriceInput.state = this.emptyString;
                    return true;
               }

               this.minDonatePriceInput.state = response.changedData.toString();
          }
          return false;
     }

     public changeNickname = async () : Promise<boolean> => {
          await this.getStreamerId();

          if (this.authStore.user) {
               if (this.nicknameInput.state === this.profile.streamerNickname) {
                    return false;
               }

               const response = await this.streamerService.changeStreamerNickname(this.profile.streamerId, this.nicknameInput.state);
               this.nicknameInput.errors = response.validationErrors;
               if (response.succeeded) {
                    this.profile.streamerNickname = response.changedData;
                    this.nicknameInput.state = this.emptyString;
                    return true;
               }
               
               this.nicknameInput.state = response.changedData;
          }

          return false;
     }

     private getStreamerId = async () => {
          if (!this.authStore.user) {
               await this.authStore.tryGetUser()
          }

          this.profile.streamerId = this.authStore.user!.profile.sub;
     }
}
