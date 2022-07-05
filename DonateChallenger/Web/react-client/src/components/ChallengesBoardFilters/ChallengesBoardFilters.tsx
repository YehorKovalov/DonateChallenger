import { observer } from 'mobx-react';
import { useEffect } from 'react';
import { Col, Form, Row } from 'react-bootstrap';
import ChallengesBoardStore from '../../stores/components/ChallengesBoardStore';
import ChallengesStore from '../../stores/components/ChallengesStore';
import { ChallengesFiltersConstants } from '../../utilities/ChallengesFiltersConstants';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const ChallengesBoardFilters = observer(() => {
     const challengesStore = useInjection<ChallengesStore>(iocStores.challengesStore);
     const boardStore = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);

     useEffect(() => {
          const fetch = async () => {
               await challengesStore.getChallengesByCurrentStatus();
          }

          fetch();

     }, [boardStore.minPriceFilter, boardStore.sortByCreatedTime]);

     return (
          <Row className='mt-3 mb-3 fs-5 color-silver justify-content-md-center'>
               <Col lg={2}>
                    <Form.Check type="switch" label="Sort by time" onChange={(e) => boardStore.sortByCreatedTime = e.target.checked}/>
               </Col>
               <Col lg={2}>
               </Col>
          </Row>
     );
});

export default ChallengesBoardFilters;