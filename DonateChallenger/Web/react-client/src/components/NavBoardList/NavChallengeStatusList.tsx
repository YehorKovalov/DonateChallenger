import { observer } from "mobx-react";
import { ChallengeStatusEnum } from "../../models/ChallengeStatusEnum";
import ChallengesStore from "../../stores/components/ChallengesStore";
import { useInjection } from "../../utilities/ioc/ioc.react";
import iocStores from "../../utilities/ioc/iocStores";
import NavLink from "../NavLink";
import ChallengeStatus from "./ChallengeStatus";
import './styles.css';

const NavChallengeStatusList = observer(() => {

     const store = useInjection<ChallengesStore>(iocStores.challengesStore);

     return (
          <li>
               <span className="menu">
                    <NavLink href='/' ><span className="menu-title menu-title">Challenges</span></NavLink>
                    <ul className="menu-dropdown">
                         <li>
                              <ChallengeStatus title={ChallengeStatusEnum.Current}
                                   onClick={async () => await store.getChallengesByStatus(ChallengeStatusEnum.Current)}/>
                         </li>
                         <li>
                              <ChallengeStatus title={ChallengeStatusEnum.Completed}
                                   onClick={async () => await store.getChallengesByStatus(ChallengeStatusEnum.Completed)}/>
                         </li>
                         <li>
                              <ChallengeStatus title={ChallengeStatusEnum.Skipped}
                                   onClick={async () => await store.getChallengesByStatus(ChallengeStatusEnum.Skipped)}/>
                         </li>
                    </ul>
               </span>
          </li>
     );
});

export default NavChallengeStatusList;