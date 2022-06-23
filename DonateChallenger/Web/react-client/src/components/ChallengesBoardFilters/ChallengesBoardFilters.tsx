import { observer } from 'mobx-react';
import { useEffect } from 'react';
import { Col, Form, Row } from 'react-bootstrap';
import ChallengerBoardStore from '../../stores/components/ChallengerBoardStore';
import ChallengesStore from '../../stores/components/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const ChallengesBoardFilters = observer(() => {
     const challengesStore = useInjection<ChallengesStore>(iocStores.challengesStore);
     const board = useInjection<ChallengerBoardStore>(iocStores.boardsStore);

     useEffect(() => {
          const fetch = async () => {
               await board.getChallengesByCurrentStatus();
          }

          fetch();

     }, [challengesStore.minPriceFilter, challengesStore.sortByCreatedTime]);

     return (
          <Row className='mt-3 mb-3 fs-5 color-silver justify-content-md-center'>
               <Col lg={2}>
                    <Form.Check type="switch" label="Sort by time" onChange={(e) => challengesStore.sortByCreatedTime = e.target.checked}/>
               </Col>
               <Col lg={2}>
                    <div>Min donate price : 100$</div>
               </Col>
          </Row>
     );
});

export default ChallengesBoardFilters;