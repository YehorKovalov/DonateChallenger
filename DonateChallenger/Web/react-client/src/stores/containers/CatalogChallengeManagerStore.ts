import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeForAddingDto } from "../../dtos/DTOs/ChallengeForAddingDto";
import { CatalogChallenge } from "../../models/CatalogChallenge";
import { PaginatedChallenges } from "../../models/PaginatedChallenges";
import { UserRole } from "../../models/UserRole";
import AuthStore from "../../oidc/AuthStore";
import { CatalogChallengeManagerService } from "../../services/CatalogChallengeManagerService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengesStore from "../states/ChallengesStore";

@injectable()
export default class CatalogChallengeManagerStore {

     @inject(iocServices.challengeCatalogManagerService) private readonly managerService!: CatalogChallengeManagerService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     @inject(iocStores.challengesStore) private readonly challengesStore!: ChallengesStore;
     private challengesPerPage = 20;

     constructor() {
          makeAutoObservable(this);
     }

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

     public update = async (challengeId: number) => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const challenge = this.challengesStore.paginatedChallenges!.data.find(f => f.challengeId === challengeId);
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
     }

     public getPaginatedCurrentChallenges = async (streamerId?: string) => {
          
          const paginatedCurrentChallenges = await this.managerService
               .getPaginatedCurrentChallenges(this.paginatedChallenges.currentPage, this.challengesPerPage, undefined, undefined, undefined, streamerId);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }

     public getPaginatedSkippedChallenges = async (streamerId?: string) => {
          
          const paginatedCurrentChallenges = await this.managerService
               .getPaginatedSkippedChallenges(this.paginatedChallenges.currentPage, this.challengesPerPage, undefined, undefined, undefined, streamerId);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }

     public getPaginatedCompletedChallenges = async (streamerId?: string) => {
          
          const paginatedCurrentChallenges = await this.managerService
               .getPaginatedCompletedChallenges(this.paginatedChallenges.currentPage, this.challengesPerPage, undefined, undefined, undefined, streamerId);
          
          this.paginatedChallenges = paginatedCurrentChallenges;
     }
}