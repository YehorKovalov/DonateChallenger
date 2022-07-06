import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import ChallengesBoardFilters from '../../components/ChallengesBoardFilters';
import PageTitle from '../../components/PageTitle';
import Pagination from '../../components/Pagination';
import CompletedChallengesBoard from '../../containers/CompletedChallengesBoard';
import CurrentChallengesBoard from '../../containers/CurrentChallengesBoard';
import SkippedChallengesBoard from '../../containers/SkippedChallengesBoard';
import { ChallengeStatusEnum } from '../../models/ChallengeStatusEnum';
import ChallengesBoardStore from '../../stores/components/ChallengesBoardStore';
import ChallengesStore from '../../stores/components/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import { formPages } from '../../utilities/PagesProvider';

const ChallengesPage = observer(() => {

     const challengesStore = useInjection<ChallengesStore>(iocStores.challengesStore);
     const boardStore = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);

     useEffect(() => {
          console.log("i am page")
          const fetch = async () => {
              await challengesStore.getChallengesByCurrentStatus();
          }
          fetch();
     }, []);

     return (
          <div>
               <PageTitle title={challengesStore.getBoardTitle()}/>
               <ChallengesBoardFilters />
               {challengesStore.currentChallengeStatus === ChallengeStatusEnum.Current && <CurrentChallengesBoard/>}
               {challengesStore.currentChallengeStatus === ChallengeStatusEnum.Skipped && <SkippedChallengesBoard/>}
               {challengesStore.currentChallengeStatus === ChallengeStatusEnum.Completed && <CompletedChallengesBoard/>}
               <Pagination/>
          </div>
     );
});

export default ChallengesPage;