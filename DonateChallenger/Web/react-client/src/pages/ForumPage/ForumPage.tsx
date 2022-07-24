import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Col, Container, Row } from 'react-bootstrap';
import ChallengePagination from '../../components/ChallengePagination';
import ChallengesBoardFilters from '../../components/ChallengesBoardFilters';
import StreamerSearch from '../../components/StreamerSearch';
import CompletedChallengesBoard from '../../containers/CompletedChallengesBoard';
import { ChallengeStatusEnum } from '../../models/ChallengeStatusEnum';
import AuthStore from '../../oidc/AuthStore';
import ChallengesBoardFiltersStore from '../../stores/components/ChallengesBoardFiltersStore';
import ChallengesBoardPaginationStore from '../../stores/components/ChallengesBoardPaginationStore';
import ChallengesBoardStore from '../../stores/containers/ChallengesBoardStore';
import StreamersStore from '../../stores/states/StreamersStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const ForumPage = observer(() => {

     const streamersStore = useInjection<StreamersStore>(iocStores.streamersStore);
     const challengesBoardStore = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);
     const authStore = useInjection<AuthStore>(iocStores.authStore);
     const filters = useInjection<ChallengesBoardFiltersStore>(iocStores.challengesBoardFiltersStore);
     const pagination = useInjection<ChallengesBoardPaginationStore>(iocStores.challengesBoardPaginationStore);

     useEffect(() => {
          challengesBoardStore.currentChallengeStatus = ChallengeStatusEnum.Completed
          if (!authStore.user) {
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }
     }, []);

     useEffect(() => {
          const fetch = async () => { await challengesBoardStore.getChallengesByCurrentStatus(streamersStore.selectedStreamer.streamerId); }
          fetch();
     }, [streamersStore.selectedStreamer, filters.minPriceFilter, filters.sortByCreatedTime, filters.sortByMinDonatePrice, pagination.currentPage]);


     return (
          <Container>
               <Row className='pt-5'>
                    <Col lg={4}>
                         <div className='pt-5 sticky-top'>
                              <StreamerSearch/>
                         </div>
                    </Col>
                    <Col lg={8}>
                         <ChallengesBoardFilters />
                         <CompletedChallengesBoard/>
                         <ChallengePagination/>
                    </Col>
               </Row>
          </Container>
     );
});

export default ForumPage;