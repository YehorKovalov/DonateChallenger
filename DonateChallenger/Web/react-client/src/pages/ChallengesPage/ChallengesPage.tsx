import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import ChallengesBoardFilters from '../../components/ChallengesBoardFilters';
import PageTitle from '../../components/PageTitle';
import CompletedChallengesBoard from '../../containers/CompletedChallengesBoard';
import CurrentChallengesBoard from '../../containers/CurrentChallengesBoard';
import SkippedChallengesBoard from '../../containers/SkippedChallengesBoard';
import { ChallengeStatusEnum } from '../../models/ChallengeStatusEnum';
import BoardsStore from '../../stores/components/ChallengerBoardStore';
import ChallengesStore from '../../stores/components/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const ChallengesPage = observer(() => {

     const challengesStore = useInjection<ChallengesStore>(iocStores.challengesStore);
     const boardsStore = useInjection<BoardsStore>(iocStores.boardsStore);
     useEffect(() => {
          const fetch = async () => {
               challengesStore.getPaginatedCurrentChallenges();
          }
          fetch();
     },[]);

     return (
          <div>
               <PageTitle title={boardsStore.getBoardTitle()}/>
               <ChallengesBoardFilters />
               {boardsStore.currentChallengeStatus === ChallengeStatusEnum.Current && <CurrentChallengesBoard/>}
               {boardsStore.currentChallengeStatus === ChallengeStatusEnum.Skipped && <SkippedChallengesBoard/>}
               {boardsStore.currentChallengeStatus === ChallengeStatusEnum.Completed && <CompletedChallengesBoard/>}
          </div>
     );
});

export default ChallengesPage;