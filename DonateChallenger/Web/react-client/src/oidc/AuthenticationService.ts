import { injectable } from "inversify";
import { User, UserManager } from "oidc-client";
import { oidcConfiguration } from "./OidcConfiguration";

export interface AuthenticationService {
     getAuthenticationStatus(): boolean;
     getUser(): Promise<User | null>;
     login(): Promise<void>;
     renewToken(): Promise<User | null>;
     logout(): Promise<void>;
     clearStaleState(): Promise<void>;
     startSilentRenew: () => void;
     stopSilentRenew: () => void;
}

@injectable()
export default class DefaultAuthenticationService implements AuthenticationService {
     private readonly userManager!: UserManager

     constructor() {
          this.userManager = new UserManager(oidcConfiguration);
     }

     public getAuthenticationStatus(): boolean {
          const oidcUser: User = JSON.parse(
               String(
                    sessionStorage.getItem(
                         `oidc.user:${process.env.REACT_APP_IDENTITY_URL}:${process.env.REACT_APP_CLIENT_ID}`
                    )
               )
               );
          return !!oidcUser && !!oidcUser.id_token;
     };

     public async getUser(): Promise<User | null> {
          return await this.userManager.getUser();
     };

     public async login(): Promise<void> {
          await this.userManager.signinRedirect();
     };

     public async renewToken(): Promise<User | null> {
          return await this.userManager.signinSilent();
     };

     public async logout(): Promise<void> {
          await this.userManager.signoutRedirect(
               {
                    id_token_hint: localStorage.getItem('id_token'),
               });
     };

     public clearStaleState = async (): Promise<void> => {
          await this.userManager.clearStaleState();
     };

     public startSilentRenew = (): void => {
          this.userManager.startSilentRenew();
     };
     
     public stopSilentRenew = (): void => {
          this.userManager.stopSilentRenew();
     };
}