import { observer } from "mobx-react";
import { ChallengeStatusEnum } from "../../models/ChallengeStatusEnum";
import ChallengerBoardStore from "../../stores/components/ChallengerBoardStore";
import { useInjection } from "../../utilities/ioc/ioc.react";
import iocStores from "../../utilities/ioc/iocStores";
import ChallengeStatus from "./ChallengeCategory";
import './styles.css';

const NavChallengeStatusList = observer(() => {

     const store = useInjection<ChallengerBoardStore>(iocStores.boardsStore);

     return (
          <li>
               <span className="menu">
                    <h2 className="menu-title menu-title">Challenges</h2>
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