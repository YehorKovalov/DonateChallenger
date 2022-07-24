import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeForAddingDto } from "../../dtos/DTOs/ChallengeForAddingDto";
import { CatalogChallenge } from "../../models/CatalogChallenge";
import { ChallengeStatusEnum } from "../../models/ChallengeStatusEnum";
import { PaginatedChallenges } from "../../models/PaginatedChallenges";
import { UserRole } from "../../models/UserRole";
import AuthStore from "../../oidc/AuthStore";
import { CatalogChallengeManagerService } from "../../services/CatalogChallengeManagerService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class CatalogChallengeManagerStore {

     @inject(iocServices.challengeCatalogManagerService) private readonly managerService!: CatalogChallengeManagerService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     private challengesPerPage = 20;

     constructor() {
          makeAutoObservable(this);
     }

     selectedStatus: ChallengeStatusEnum = ChallengeStatusEnum.Current;

     challengeToAdd: ChallengeForAddingDto = {
          description: '',
          donatePrice: 0,
          donateFrom: '',
          streamerId: '',
          title: ''
     }

     challenge: CatalogChallenge = {
          challengeId: 0,
          description: '',
          donatePrice: 0,
          donateFrom: '',
          title: '',
          createdTime: ''
     }

     paginatedChallenges: PaginatedChallenges<CatalogChallenge> = {
          totalCount: 0,
          totalPages: 0,
          challengesPerPage: this.challengesPerPage,
          currentPage: 0,
          data: []
     };

     public add = async () => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.addChallenge(this.challengeToAdd.description, this.challengeToAdd.donatePrice, this.challengeToAdd.donateFrom, this.challengeToAdd.streamerId, this.challengeToAdd.title);
               if (!result.challengeId) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }

               alert("Successful");
          }
     }

     public getById = async (challengeId: number) => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.getChallengeById(challengeId);
               if (!result.data) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }

               this.challenge = result.data;
          }
     }

     public update = async (challengeId: number, streamerId: string) => {

          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const challenge = this.paginatedChallenges.data!.find(f => f.challengeId === challengeId);
               if (!challenge) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }
     
               const result = await this.managerService.updateChallenge(challenge.challengeId, challenge.description, challenge.donatePrice, challenge.donateFrom, challenge.title)
               if (result.data && result.data === challenge.challengeId) {
                    alert("Successful");
                    return;
               }
     
               alert("Not updated, check logs");          
          }
          else {
               alert("You don't have an access or unauthorized");
          }
     }

     public getPaginatedCurrentChallenges = async (streamerId: string) => {

          await this.handlPaginating(async () => {
               const paginatedCurrentChallenges = await this.managerService.getPaginatedCurrentChallenges(this.paginatedChallenges.currentPage, this.challengesPerPage, streamerId);
               this.paginatedChallenges = paginatedCurrentChallenges;
          }, streamerId);
     }

     public getPaginatedSkippedChallenges = async (streamerId: string) => {

          await this.handlPaginating(async () => {
               const paginatedCurrentChallenges = await this.managerService.getPaginatedSkippedChallenges(this.paginatedChallenges.currentPage, this.challengesPerPage, streamerId);
               this.paginatedChallenges = paginatedCurrentChallenges;
          }, streamerId);
     }

     public getPaginatedCompletedChallenges = async (streamerId: string) => {
          
          await this.handlPaginating(async () => {
               const paginatedCurrentChallenges = await this.managerService.getPaginatedCompletedChallenges(this.paginatedChallenges.currentPage, this.challengesPerPage, streamerId);
               this.paginatedChallenges = paginatedCurrentChallenges;
          }, streamerId);
     }

     private getPaginatedChallengesByCurrentStatus = async (streamerId: string) => {
          switch (this.selectedStatus) {
               case ChallengeStatusEnum.Current:
                    await this.getPaginatedCurrentChallenges(streamerId);
                    break;
               case ChallengeStatusEnum.Completed:
                    await this.getPaginatedCompletedChallenges(streamerId);
                    break;
               case ChallengeStatusEnum.Skipped:
                    await this.getPaginatedSkippedChallenges(streamerId);
                    break;
          }
     }

     private handlPaginating = async (method: () => Promise<void>, streamerId: string) => {
          if (streamerId) {
               await method();
          }
          else {
               alert("Choose streamer");
          }
     }
}