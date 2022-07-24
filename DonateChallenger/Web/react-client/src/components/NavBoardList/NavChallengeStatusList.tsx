import { observer } from "mobx-react";
import { ChallengeStatusEnum } from "../../models/ChallengeStatusEnum";
import ChallengesBoardStore from "../../stores/containers/ChallengesBoardStore";
import { useInjection } from "../../utilities/ioc/ioc.react";
import iocStores from "../../utilities/ioc/iocStores";
import NavLink from "../NavLink";
import ChallengeStatus from "./ChallengeStatus";
import './styles.css';

const NavChallengeStatusList = observer(() => {

     const store = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);

     return (
          <li>
               <span className="menu">
                    <NavLink href='/challenges' className="menu-title">Challenges</NavLink>
                    <ul className="menu-dropdown">
                         <li>
                              <ChallengeStatus title={ChallengeStatusEnum.Current}
                                   onClick={() => store.currentChallengeStatus = ChallengeStatusEnum.Current}/>
                         </li>
                         <li>
                              <ChallengeStatus title={ChallengeStatusEnum.Completed}
                                   onClick={() => store.currentChallengeStatus = ChallengeStatusEnum.Completed}/>
                         </li>
                         <li>
                              <ChallengeStatus title={ChallengeStatusEnum.Skipped}
                                   onClick={() => store.currentChallengeStatus = ChallengeStatusEnum.Skipped}/>
                         </li>
                    </ul>
               </span>
          </li>
     );
});

export default NavChallengeStatusList;