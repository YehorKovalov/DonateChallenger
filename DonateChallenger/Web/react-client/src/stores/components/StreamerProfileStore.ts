import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { StreamerProfile } from "../../models/StreamerProfile";
import AuthStore from "../../oidc/AuthStore";
import { StreamerService } from "../../services/StreamerService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class StreamerProfileStore {
     @inject(iocServices.streamerService) private readonly streamerService!: StreamerService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     
     constructor() {
          makeAutoObservable(this);
     }

     profile: StreamerProfile = null!;

     changedNickname: string = '';
     changedMinDonatePrice: string = '';

     public getStreamerProfile = async () => {
          await this.getStreamerId();
          var response = await this.streamerService.getStreamerProfileById(this.profile.streamerId);
          this.profile = response.data;
     }

     public getMinDonatePrice = async () : Promise<void> => {
          await this.getStreamerId();

          if (this.authStore.user) {
               const response = await this.streamerService.getMinDonatePrice(this.profile.streamerId);
               this.profile!.minDonatePrice = response.data;
          }
     }

     public changeMinDonatePrice = async () : Promise<void> => {
          const minDonatePrice = Number.parseInt(this.changedMinDonatePrice);
          await this.getStreamerId();
          if (this.authStore.user) {
               if (minDonatePrice === this.profile.minDonatePrice) {
                    console.log("Invalid minDonatePrice")
                    return;
               }

               const response = await this.streamerService.changeMinDonatePrice(this.profile.streamerId, minDonatePrice);
               
               const emptyString = '';
               this.changedMinDonatePrice = emptyString;
               console.log(`Changing is succesful: ${response.Succeeded}`)
          }
     }

     public changeNickname = async () : Promise<void> => {
          await this.getStreamerId();

          if (this.authStore.user) {
               if (this.changedNickname === this.profile.streamerNickname) {
                    return;
               }

               const emptyString = '';
               this.changedNickname = emptyString;
          }
     }

     private getStreamerId = async () => {
          if (!this.authStore.user) {
               await this.authStore.tryGetUser()
          }

          this.profile.streamerId = this.authStore.user!.profile.sub;
     }
}
