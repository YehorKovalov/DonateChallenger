import { injectable } from "inversify";
import { SignoutResponse, User, UserManager } from "oidc-client";
import { oidcConfiguration } from "./OidcConfiguration";

export interface AuthenticationService {
     getUser(): Promise<User | null>;
     login(): Promise<void>;
     logout(): Promise<void>;
     signinRedirectCallback: () => Promise<User | undefined>;
     signoutRedirectCallback: () => Promise<SignoutResponse>;
}

@injectable()
export default class DefaultAuthenticationService implements AuthenticationService {
     private readonly userManager!: UserManager

     constructor() {
          this.userManager = new UserManager(oidcConfiguration);
     }

     public async getUser(): Promise<User | null> {
          return await this.userManager.getUser();
     };

     public async login(): Promise<void> {
          await this.userManager.signinRedirect();
     };

     public async logout(): Promise<void> {
          await this.userManager.signoutRedirect();
     }

     public signinRedirectCallback = async (): Promise<User | undefined> => {
          return await this.userManager.signinRedirectCallback();
     };     

     public signoutRedirectCallback = async (): Promise<SignoutResponse> => {
          return await this.userManager.signoutRedirectCallback();
     };
}