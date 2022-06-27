import { injectable } from "inversify";
import { SigninRequest, SignoutRequest, SignoutResponse, User, UserManager } from "oidc-client";
import { oidcConfiguration } from "./OidcConfiguration";

export interface AuthenticationService {
     getAuthenticationStatus(): boolean;
     getUser(): Promise<User | null>;
     signinRedirect(): Promise<void>;
     signinRedirectCallback(): Promise<User | undefined>;
     signinSilent(): Promise<User | undefined>;
     signinSilentCallback(): Promise<User | undefined>;
     signoutRedirect(): Promise<void>;
     signoutRedirectCallback(): Promise<SignoutResponse>;
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
                         `oidc.user:${process.env.REACT_APP_IDENTITY_URL}:${process.env.REACT_APP_IDENTITY_CLIENT_ID}`
                    )
               )
               );
          return !!oidcUser && !!oidcUser.id_token;
     }

     public async getUser(): Promise<User | null> {
          return await this.userManager.getUser();
     }

     public async signinRedirect(): Promise<void> {
          await this.userManager.signinRedirect();
     }

     public async signinRedirectCallback(): Promise<User | undefined> {
          return await this.userManager.signinRedirectCallback();
     }

     public async signinSilent(): Promise<User | undefined> {
          return await this.userManager.signinSilent();
     }

     public async signinSilentCallback(): Promise<User | undefined> {
          return await this.userManager.signinSilentCallback();
     }

     public async signoutRedirect(): Promise<void> {
          await this.userManager.signoutRedirect(
               {
                    id_token_hint: localStorage.getItem('id_token'),
               });
     }

     public async signoutRedirectCallback(): Promise<SignoutResponse> {
          return await this.userManager.signoutRedirectCallback();
     }
}