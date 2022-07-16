import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { PaymentService } from "../../services/PaymentService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengesTempStorageStore from "../global/ChallengesTempStorageStore";
import StreamersStore from "../states/StreamersStore";

@injectable()
export default class ChallengeOrderStore {

     @inject(iocStores.challengesTempStorageStore) private readonly challengesTempStorageStore!: ChallengesTempStorageStore;
     @inject(iocStores.streamersStore) private readonly streamersStore!: StreamersStore;
     @inject(iocServices.paymentService) private readonly paymentService!: PaymentService;

     constructor() {
          makeAutoObservable(this);
     }

     public makeOrder = async () => {

          const streamer = this.streamersStore.selectedStreamer;
          const storageUpdatingResult = await this.challengesTempStorageStore.addChallengeToStorage(streamer.streamerId);
          if (!storageUpdatingResult) {
               return;
          }

          const sum = this.challengesTempStorageStore.getDonationsSumPrice();
          const response = await this.paymentService.getPayPalPaymentUrl(sum, "USD", streamer.merchantId);
          window.location.replace(response.url);
     }
}