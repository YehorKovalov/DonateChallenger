import { Container } from "inversify";
import DefaultChallengeService, { ChallengeService } from "../../services/ChallengeService";
import DefaultHttpService, { HttpService } from "../../services/HttpService";
import AuthStore from "../../stores/AuthStore";
import ChallengerBoardStore from "../../stores/components/ChallengerBoardStore";
import ChallengesStore from "../../stores/components/ChallengesStore";
import ChallengeStore from "../../stores/components/ChallengeStore";
import iocServices from "./iocServices";
import iocStores from "./iocStores";

export const container = new Container();
container.bind<HttpService>(iocServices.httpService).to(DefaultHttpService).inSingletonScope();
container.bind<ChallengeService>(iocServices.challengeService).to(DefaultChallengeService).inTransientScope();

container.bind<ChallengesStore>(iocStores.challengesStore).to(ChallengesStore).inSingletonScope();
container.bind<ChallengeStore>(iocStores.challengeStore).to(ChallengeStore).inSingletonScope();
container.bind<ChallengerBoardStore>(iocStores.boardsStore).to(ChallengerBoardStore).inSingletonScope();
container.bind<AuthStore>(iocStores.authStore).to(AuthStore).inSingletonScope();