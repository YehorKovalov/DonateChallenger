import { inject, injectable } from "inversify";
import { ChangeProfileDataResponse } from "../../dtos/response/ChangeProfileDataResponse";
import { InputWithValidation } from "../../models/InputWithValidation";
import AuthStore from "../../oidc/AuthStore";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class InputValidationStore {

     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     public changeProfileField = async <T>(input: InputWithValidation<T>, inputFor: T, method: (id: string, changeOn: T) => Promise<ChangeProfileDataResponse<T>>)
          : Promise<[succeeded: boolean, errors: string[], changedData: T]> => {

          if (this.authStore.user) {
               if (input.state === inputFor) {
                    return [false, [], input.state];
               }    

               const response = await method(this.authStore.user.profile.sub, input.state);
               
               if (response.succeeded) {
                    return [true, [], response.changedData];
               }

               return [false, [...response.validationErrors], response.changedData]
          }

          return [false, [ "Unauthorized" ], input.state];
     }
}