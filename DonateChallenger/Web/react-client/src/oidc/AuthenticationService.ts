import { injectable } from "inversify";
import { SignoutResponse, User, UserManager } from "oidc-client";
import { oidcConfiguration } from "./OidcConfiguration";

export interface AuthenticationService {
     getUser(): Promise<User | null>;
     login(): Promise<void>;
     logout(): Promise<void>;
     signinRedirectCallback: () => Promise<User | undefined>;
     signoutRedirectCallback: () => Promise<SignoutResponse>;
     clearStaleState: () => Promise<void>;
     signinPopup: () => Promise<User>;
     signinPopupCallback: () => Promise<User | undefined>;
     signinSilent: () => Promise<User>;
     signinSilentCallback: () => Promise<User | undefined>;
     signoutPopup: () => Promise<void>;
     signoutPopupCallback: () => Promise<void>;
     startSilentRenew: () => void;
     stopSilentRenew: () => void;
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

     public clearStaleState = async (): Promise<void> => {
          await this.userManager.clearStaleState();
     };
     
     public signinPopup = async (): Promise<User> => {
          return await this.userManager.signinPopup();
     };
      
     public signinPopupCallback = async (): Promise<User | undefined> => {
          return (await this.userManager.signinPopupCallback()) || undefined;
     };
     
     public signinSilent = async (): Promise<User> => {
          return await this.userManager.signinSilent();
     };
      
     public signinSilentCallback = async (): Promise<User | undefined> => {
          return (await this.userManager.signinSilentCallback()) || undefined;
     };
     public signoutPopup = async (): Promise<void> => {
          await this.userManager.signoutPopup();
     };
          
     public signoutPopupCallback = async (): Promise<void> => {
          await this.userManager.signoutPopupCallback();
     };
     public startSilentRenew = (): void => {
          this.userManager.startSilentRenew();
     };
          
     public stopSilentRenew = (): void => {
          this.userManager.stopSilentRenew();
     };
}