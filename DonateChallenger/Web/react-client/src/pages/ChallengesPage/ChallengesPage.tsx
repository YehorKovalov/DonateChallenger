import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import ChallengesBoardFilters from '../../components/ChallengesBoardFilters';
import PageTitle from '../../components/PageTitle';
import Pagination from '../../components/Pagination';
import CompletedChallengesBoard from '../../containers/CompletedChallengesBoard';
import CurrentChallengesBoard from '../../containers/CurrentChallengesBoard';
import SkippedChallengesBoard from '../../containers/SkippedChallengesBoard';
import { ChallengeStatusEnum } from '../../models/ChallengeStatusEnum';
import ChallengesBoardFiltersStore from '../../stores/components/ChallengesBoardFiltersStore';
import ChallengesBoardPaginationStore from '../../stores/components/ChallengesBoardPaginationStore';
import ChallengesBoardStore from '../../stores/containers/ChallengesBoardStore';
import ChallengesStore from '../../stores/states/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const ChallengesPage = observer(() => {

     const boardStore = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);
     const filters = useInjection<ChallengesBoardFiltersStore>(iocStores.challengesBoardFiltersStore);
     const pagination = useInjection<ChallengesBoardPaginationStore>(iocStores.challengesBoardPaginationStore);
     const challenges = useInjection<ChallengesStore>(iocStores.challengesStore);

     useEffect(() => {
          const fetch = async () => { await boardStore.getChallengesByCurrentStatus(); }
          fetch();
     }, [filters.minPriceFilter, filters.sortByCreatedTime, pagination.currentPage, boardStore.currentChallengeStatus]);

     return (
          <div>
               <PageTitle title={boardStore.getBoardTitle()}/>
               {challenges.paginatedChallenges?.data?.length
               ? <>
                    <ChallengesBoardFilters />
                    {boardStore.currentChallengeStatus === ChallengeStatusEnum.Current && <CurrentChallengesBoard/>}
                    {boardStore.currentChallengeStatus === ChallengeStatusEnum.Skipped && <SkippedChallengesBoard/>}
                    {boardStore.currentChallengeStatus === ChallengeStatusEnum.Completed && <CompletedChallengesBoard/>}
                    <Pagination/>
               </>
               :
               <div className='center-block'>
                    <span className='fs-1 color-silver'>You don't have any {boardStore.currentChallengeStatus.toLowerCase()} challenge...</span>
               </div>}
          </div>
     );
});

export default ChallengesPage;