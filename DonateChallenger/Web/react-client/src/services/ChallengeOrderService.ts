import { injectable } from "inversify";
import { AddChallengeOrderResponse } from "../dtos/response/AddChallengeOrderResponse";
export interface ChallengeOrderService {
     buildOrder(description: string, donatePrice: number, streamerId: string, donateFrom: string, title?: string)
     : Promise<AddChallengeOrderResponse<string>>;
}

@injectable()
export default class DefaultChallengeOrderService implements ChallengeOrderService {
     public async buildOrder(description: string, donatePrice: number, streamerId: string, donateFrom: string, title?: string | undefined)
          : Promise<AddChallengeOrderResponse<string>> {
          throw new Error("Method not implemented.");
     }
}