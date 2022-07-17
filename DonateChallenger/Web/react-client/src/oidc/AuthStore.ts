import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { User } from "oidc-client";
import { ApiHeader, ContentType } from "../services/HttpService";
import { LocalStorageService } from "../services/LocalStorageService";
import iocServices from "../utilities/ioc/iocServices";
import { AuthenticationService } from "./AuthenticationService";

@injectable()
export default class AuthStore {
     @inject(iocServices.authenticationService) private readonly authenticationService!: AuthenticationService;
     @inject(iocServices.localStorageService) private readonly localStorageService!: LocalStorageService;
     private readonly redirectUri = 'redirectUri';
     constructor() {
          makeAutoObservable(this);
     }

     user: User | null = null;

     public tryGetUser = async (): Promise<void> => {
          const userResponse = await this.authenticationService.getUser();
          this.user = userResponse;
     };

     public removeRedirectLocation = (): void => {
          this.localStorageService.remove(this.redirectUri);
     };
        
     public replaceLocation = (): void => {
          window.location.replace(this.localStorageService.get(this.redirectUri) || '/');
     };

     public signinRedirect = async (): Promise<void> => {
          this.saveLocation();
          this.authenticationService.stopSilentRenew();
          await this.authenticationService.clearStaleState();
          await this.authenticationService.login();
          this.authenticationService.startSilentRenew();
     };

     public saveLocation = (location?: string): void => {
          if (location) {
            this.localStorageService.set(this.redirectUri, location);
          } else if (window.location.pathname !== '/signin' && window.location.pathname !== '/signout') {
            this.localStorageService.set(this.redirectUri, window.location.pathname);
          } else {
            localStorage.setItem(this.redirectUri, '/');
          }
     };
      

     public signoutRedirect = async (): Promise<void> => {
          this.saveLocation();
          await this.authenticationService.logout();

     };

     public signinRedirectCallback = async (): Promise<void> => {
          await this.authenticationService.signinRedirectCallback();
          this.replaceLocation();
          this.removeRedirectLocation();
     };

     public signinSilent = async (): Promise<void> => {
          this.user = await this.authenticationService.signinSilent();
     };
      

     public signoutRedirectCallback = async (): Promise<void> => {
          await this.authenticationService.signoutRedirectCallback();
          window.localStorage.clear();
          this.replaceLocation();
          this.removeRedirectLocation();
     };

     public tryGetAuthorizedHeaders = async (): Promise<ApiHeader> => {
          await this.tryGetUser();
          const headers: ApiHeader = {
               contentType: ContentType.Json,
               authorization: this.user?.access_token
          }

          return headers;
     }
}