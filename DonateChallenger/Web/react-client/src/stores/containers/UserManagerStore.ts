import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { PortionedProfiles } from "../../models/PortionedProfiles";
import { StreamerProfile } from "../../models/StreamerProfile";
import { UserProfile } from "../../models/UserProfile";
import { UserRole } from "../../models/UserRole";
import AuthStore from "../../oidc/AuthStore";
import { UserManagerService } from "../../services/UserManagerService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class UserManagerStore {

     @inject(iocServices.userManagerService) private readonly userManagerService!: UserManagerService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     private profilesPerPortion = 12;

     constructor() { makeAutoObservable(this); }

     users: PortionedProfiles<UserProfile> = {
          currentPortion: 1,
          usersPerPortion: this.profilesPerPortion,
          morePortionsExist: true,
          users: []
     }

     streamers: PortionedProfiles<StreamerProfile> = {
          currentPortion: 1,
          usersPerPortion: this.profilesPerPortion,
          morePortionsExist: true,
          users: []
     }

     public updateStreamerProfile = async (userId: string) => {
          if (this.userIsNotAdmin()) {
               return;
          }

          const streamer = this.streamers.users.find(s => s.streamerId === userId);
          if (!streamer) {
               return;
          }

          const streamerUpdated = await this.userManagerService.updateStreamerProfile(streamer.streamerId, streamer.streamerNickname, streamer.email, streamer.merchantId, streamer.minDonatePrice);
          if (streamerUpdated.succeeded) {
               alert("Succeeded");
               await this.getStreamers();
          }
     }

     public updateUserProfile = async (userId: string) => {
          if (this.userIsNotAdmin()) {
               return;
          }
          
          const user = this.users.users.find(s => s.userId === userId);
          if (!user) {
               return;
          }

          const userUpdated = await this.userManagerService.updateUserProfile(user.userId, user.userNickname, user.email);
          if (userUpdated.succeeded) {
               alert("Succeeded");
               await this.getAll();
          }
     }

     public delete = async (userId: string) => {
          if (this.userIsNotAdmin()) {
               return;
          }

          const userDeleted = await this.userManagerService.delete(userId);
          if (userDeleted.data) {
               alert("Succeeded");
               await this.getAll();
          }
     }

     public getAll = async () => {
          if (this.userIsNotAdmin()) {
               return;
          }

          const users = await this.userManagerService.getAllUsers(this.users.currentPortion, this.users.usersPerPortion);
          this.users = users;
     }

     public getStreamers = async () => {
          if (this.userIsNotAdmin()) {
               return;
          }

          const streamers = await this.userManagerService.getStreamers(this.streamers.currentPortion, this.streamers.usersPerPortion);
          this.streamers = streamers;
     }

     private userIsNotAdmin(): boolean { return !this.authStore.user || this.authStore.userRole !== UserRole.Admin; }
} 