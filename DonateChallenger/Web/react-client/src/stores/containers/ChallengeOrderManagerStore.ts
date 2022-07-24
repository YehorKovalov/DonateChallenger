import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { ChallengeOrder } from "../../models/ChallengeOrder";
import { ChallengeOrderToAdd } from "../../models/ChallengeOrderToAdd";
import { PaginatedOrders } from "../../models/PaginatedOrders";
import { UserRole } from "../../models/UserRole";
import AuthStore from "../../oidc/AuthStore";
import { ChallengeOrderManagerService } from "../../services/ChallengeOrderManagerService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class ChallengeOrderManagerStore {

     @inject(iocServices.challengeOrderManagerService) private readonly managerService!: ChallengeOrderManagerService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     private orderPerPage = 20;

     constructor() {
          makeAutoObservable(this);
     }

     order: ChallengeOrder = {
          challengeOrderId: '',
          paymentId: '',
          challengesAmount: 0,
          resultDonationPrice: 0,
          date: ''
     }

     orderToAdd: ChallengeOrderToAdd = {
          paymentId: '',
          challengesAmount: 0,
          resultDonationPrice: 0,
     }

     paginatedOrders: PaginatedOrders<ChallengeOrder> = {
          totalCount: 0,
          totalPages: 0,
          challengeOrdersPerPage: this.orderPerPage,
          currentPage: 0,
          data: []
     }
     
     public add = async () => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.addChallengeOrder(this.orderToAdd.paymentId, this.orderToAdd.challengesAmount, this.orderToAdd.resultDonationPrice);
               if (!result.succeeded) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }

               alert("Successful");
          }
     }

     public getPaginated = async () => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.getPaginatedOrders(this.paginatedOrders.currentPage, this.paginatedOrders.challengeOrdersPerPage);
               this.paginatedOrders = result;
          }
     }

     public getById = async (orderId: string) => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.getOrderById(orderId);
               if (!result.data) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }

               this.order = result.data;
          }
     }

     public update = async (orderId: string) => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const order = this.paginatedOrders.data.find(f => f.challengeOrderId === orderId);
               if (!order) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }
     
               const result = await this.managerService.updateChallengeOrder(order.challengeOrderId, order.paymentId, order.challengesAmount, order.resultDonationPrice);
               if (result.data && result.data === order.challengeOrderId) {
                    alert("Successful");
                    return;
               }
     
               alert("Not updated, check logs");          
          }
     }
}