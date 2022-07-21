import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { User } from "oidc-client";
import { UserRole } from "../models/UserRole";
import { ApiHeader, ContentType } from "../services/HttpService";
import { LocalStorageService } from "../services/LocalStorageService";
import iocServices from "../utilities/ioc/iocServices";
import { AuthenticationService } from "./AuthenticationService";

@injectable()
export default class AuthStore {
     @inject(iocServices.authenticationService) private readonly authenticationService!: AuthenticationService;
     @inject(iocServices.localStorageService) private readonly localStorageService!: LocalStorageService;
     constructor() {
          makeAutoObservable(this);
     }

     user: User | null = null;
     userRole: UserRole = UserRole.Anonymous;

     public tryGetUser = async (): Promise<void> => {
          this.user = await this.authenticationService.getUser();
          this.userRole = await this.authenticationService.getUserRole();
     };

     public signinRedirect = async (): Promise<void> => {
          await this.authenticationService.clearStaleState();
          await this.authenticationService.login();
     };

     public signoutRedirect = async (): Promise<void> => {
          await this.authenticationService.logout();
     };

     public signinRedirectCallback = async (): Promise<void> => {
          await this.authenticationService.signinRedirectCallback();
          this.userRole = await this.authenticationService.getUserRole();
          location.replace("/")
     };

     public signinSilent = async (): Promise<void> => {
          this.user = await this.authenticationService.signinSilent();
     };
      

     public signoutRedirectCallback = async (): Promise<void> => {
          await this.authenticationService.signoutRedirectCallback();
          window.localStorage.clear();
          this.userRole = await this.authenticationService.getUserRole();
          location.replace("/")
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