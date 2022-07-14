import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { OperationCanceledException } from "typescript";
import { PaymentService } from "../../services/PaymentService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengesTempStorageStore from "../ChallengesTempStorageStore";
import StreamersStore from "./StreamersStore";

@injectable()
export default class ChallengeOrderStore {

     @inject(iocStores.challengesTempStorageStore) private readonly challengesTempStorageStore!: ChallengesTempStorageStore;
     @inject(iocStores.streamersStore) private readonly streamersStore!: StreamersStore;
     @inject(iocServices.paymentService) private readonly paymentService!: PaymentService;

     constructor() {
          makeAutoObservable(this);
     }


     public makeOrder = async () => {
          const storageUpdatingResult = await this.challengesTempStorageStore.addChallenge();
          if (!storageUpdatingResult) {
               throw OperationCanceledException;
          }

          const streamer = this.streamersStore.selectedStreamer;
          const sum = this.challengesTempStorageStore.getDonationsSumPrice();
          this.paymentService.goToPayPalPayment(sum, "USD", streamer.merchantId);
     }
}