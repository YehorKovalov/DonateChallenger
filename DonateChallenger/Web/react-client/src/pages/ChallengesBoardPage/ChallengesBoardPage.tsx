import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import ChallengePagination from '../../components/ChallengePagination';
import ChallengesBoardFilters from '../../components/ChallengesBoardFilters';
import PageTitle from '../../components/PageTitle';
import CompletedChallengesBoard from '../../containers/CompletedChallengesBoard';
import CurrentChallengesBoard from '../../containers/CurrentChallengesBoard';
import SkippedChallengesBoard from '../../containers/SkippedChallengesBoard';
import { ChallengeStatusEnum } from '../../models/ChallengeStatusEnum';
import AuthStore from '../../oidc/AuthStore';
import ChallengesBoardFiltersStore from '../../stores/components/ChallengesBoardFiltersStore';
import ChallengesBoardPaginationStore from '../../stores/components/ChallengesBoardPaginationStore';
import ChallengesBoardStore from '../../stores/containers/ChallengesBoardStore';
import ChallengesStore from '../../stores/states/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const ChallengesBoardPage = observer(() => {

     const boardStore = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);
     const filters = useInjection<ChallengesBoardFiltersStore>(iocStores.challengesBoardFiltersStore);
     const pagination = useInjection<ChallengesBoardPaginationStore>(iocStores.challengesBoardPaginationStore);
     const challenges = useInjection<ChallengesStore>(iocStores.challengesStore);
     const authStore = useInjection<AuthStore>(iocStores.authStore);

     useEffect(() => {
          if (!authStore.user) {
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }
     }, []);

     useEffect(() => {
          const fetch = async () => { await boardStore.getChallengesByCurrentStatus(); }
          fetch();
     }, [filters.minPriceFilter, filters.sortByCreatedTime, filters.sortByMinDonatePrice, pagination.currentPage, boardStore.currentChallengeStatus]);

     return (
          <div>
               <PageTitle title={boardStore.getBoardTitle()}/>
               <ChallengesBoardFilters />
               {challenges.paginatedChallenges?.data?.length
               ? <>
                    {boardStore.currentChallengeStatus === ChallengeStatusEnum.Current && <CurrentChallengesBoard/>}
                    {boardStore.currentChallengeStatus === ChallengeStatusEnum.Skipped && <SkippedChallengesBoard/>}
                    {boardStore.currentChallengeStatus === ChallengeStatusEnum.Completed && <CompletedChallengesBoard/>}
                    <ChallengePagination/>
               </>
               :
               <div className='center-block'>
                    <span className='fs-1 color-silver'>You don't have any {boardStore.currentChallengeStatus.toLowerCase()} challenge...</span>
               </div>}
          </div>
     );
});

export default ChallengesBoardPage;