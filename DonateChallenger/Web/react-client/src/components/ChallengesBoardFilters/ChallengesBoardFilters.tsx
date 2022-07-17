import { observer } from 'mobx-react';
import { Col, Form, Row } from 'react-bootstrap';
import ChallengesBoardFiltersStore from '../../stores/components/ChallengesBoardFiltersStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const ChallengesBoardFilters = observer(() => {

     const filters = useInjection<ChallengesBoardFiltersStore>(iocStores.challengesBoardFiltersStore);

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