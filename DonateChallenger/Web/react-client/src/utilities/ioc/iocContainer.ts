import { Container } from "inversify";
import DefaultChallengeService, { ChallengeService } from "../../services/ChallengeService";
import DefaultHttpService, { HttpService } from "../../services/HttpService";
import AuthStore from "../../oidc/AuthStore";
import ChallengesStore from "../../stores/components/ChallengesStore";
import ChallengeStore from "../../stores/components/ChallengeStore";
import iocServices from "./iocServices";
import iocStores from "./iocStores";
import DefaultAuthenticationService, { AuthenticationService } from "../../oidc/AuthenticationService";
import DefaultLocalStorageService, { LocalStorageService } from "../../services/LocalStorageService";
import ChallengesBoardStore from "../../stores/components/ChallengesBoardStore";
import UserRoleStore from "../../stores/UserRoleStore";
import DefaultStreamerService, { StreamerService } from "../../services/StreamerService";
import StreamerProfileStore from "../../stores/components/StreamerProfileStore";

export const container = new Container();
container.bind<HttpService>(iocServices.httpService).to(DefaultHttpService).inSingletonScope();
container.bind<ChallengeService>(iocServices.challengeService).to(DefaultChallengeService).inTransientScope();
container.bind<AuthenticationService>(iocServices.authenticationService).to(DefaultAuthenticationService).inSingletonScope();
container.bind<LocalStorageService>(iocServices.localStorageService).to(DefaultLocalStorageService).inTransientScope();
container.bind<StreamerService>(iocServices.streamerService).to(DefaultStreamerService).inTransientScope();

container.bind<ChallengesStore>(iocStores.challengesStore).to(ChallengesStore).inSingletonScope();
container.bind<ChallengeStore>(iocStores.challengeStore).to(ChallengeStore).inSingletonScope();
container.bind<ChallengesBoardStore>(iocStores.challengesBoardStore).to(ChallengesBoardStore).inSingletonScope();
container.bind<AuthStore>(iocStores.authStore).to(AuthStore).inSingletonScope();
container.bind<UserRoleStore>(iocStores.userRoleStore).to(UserRoleStore).inSingletonScope();
container.bind<StreamerProfileStore>(iocStores.streamerProfileStore).to(StreamerProfileStore).inSingletonScope();