import { observer } from 'mobx-react';
import { useEffect } from 'react';
import { Col, Form, Row } from 'react-bootstrap';
import ChallengesBoardFiltersStore from '../../stores/components/ChallengesBoardFiltersStore';
import ChallengesBoardStore from '../../stores/containers/ChallengesBoardStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const ChallengesBoardFilters = observer(() => {

     const challengesBoardStore = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);
     const filters = useInjection<ChallengesBoardFiltersStore>(iocStores.challengesBoardFiltersStore);

     useEffect(() => {
          const fetchGetChallengesByCurrentStatus = async () => { await challengesBoardStore.getChallengesByCurrentStatus(); }
          fetchGetChallengesByCurrentStatus();

     }, [filters.minPriceFilter, filters.sortByCreatedTime]);

     return (
          <Row className='mt-3 mb-3 fs-5 color-silver justify-content-md-center'>
               <Col lg={2}>
                    <Form.Check type="switch" label="Sort by time" onChange={(e) => filters.sortByCreatedTime = e.target.checked}/>
               </Col>
               <Col lg={2}>
               </Col>
               <Col lg={2}>
               </Col>
          </Row>
     );
});

export default ChallengesBoardFilters;