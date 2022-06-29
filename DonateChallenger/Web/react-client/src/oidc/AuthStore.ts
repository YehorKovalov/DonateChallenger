import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { User } from "oidc-client";
import { LocalStorageService } from "../services/LocalStorageService";
import iocServices from "../utilities/ioc/iocServices";
import { AuthenticationService } from "./AuthenticationService";

@injectable()
export default class AuthStore {
     @inject(iocServices.authenticationService) private readonly authenticationService!: AuthenticationService;
     @inject(iocServices.localStorageService) private readonly localStorageService!: LocalStorageService;
     private readonly redirectUri_id = 'redirectUri';

     constructor() {
          makeAutoObservable(this);
     }

     userIsAuthenticated = true;
     user: User | null = null;

     public getAuthenticationStatus = (): void => {
          this.userIsAuthenticated =  this.authenticationService.getAuthenticationStatus();
     };

     public tryGetUser = async (): Promise<void> => {
          const userResponse = await this.authenticationService.getUser();
          this.setUser(userResponse);
     };

     public removeRedirectLocation = (): void => {
          this.localStorageService.remove(this.redirectUri_id);
     };

     public replaceLocation = (): void => {
          window.location.replace(this.localStorageService.get(this.redirectUri_id) || '/');
     };

     public saveLocation = (location?: string): void => {
          if (location) {
               this.localStorageService.set(this.redirectUri_id, location);
          }
          else if (window.location.pathname !== '/signin' && window.location.pathname !== '/signout') {
               this.localStorageService.set(this.redirectUri_id, window.location.pathname);
          }
          else {
               localStorage.setItem(this.redirectUri_id, '/');
          }
     };

     public setUser = (user: User | null): void => {
          this.user = user;
     };

     public signinRedirect = async (location?: string): Promise<void> => {
          this.saveLocation(location);
          this.authenticationService.stopSilentRenew();
          await this.authenticationService.clearStaleState();
          await this.authenticationService.login();
          this.authenticationService.startSilentRenew();
     };

     public signinSilent = async (): Promise<void> => {
          this.user = await this.authenticationService.renewToken();
     };

     public signoutRedirect = async (location?: string): Promise<void> => {
          this.saveLocation(location);
          await this.authenticationService.logout();
          await this.authenticationService.clearStaleState();
     };
}