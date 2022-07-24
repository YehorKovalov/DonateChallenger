import { observer } from 'mobx-react-lite';
import { Col, Row } from 'react-bootstrap';
import ChallengesBoardPaginationStore from '../../stores/components/ChallengesBoardPaginationStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import Arrow from '../Arrow';
import { ArrowDirection } from '../Arrow/Arrow';
import PaginationButton from '../PaginationButton';

const ChallengePagination = observer(() => {

     const pagination = useInjection<ChallengesBoardPaginationStore>(iocStores.challengesBoardPaginationStore);

          return (
          <Row className='justify-content-md-center text-center pb-5 pt-5'>
               <Col lg={1}>
                    {pagination.currentPage - 1 >= 0 &&
                    <Arrow onClick={() => pagination.changePageOnPrevious()}
                         size={3} direction={ArrowDirection.left}  />
                    }
               </Col>
               <Col lg={2}>
                    {pagination.buttons.length > 1 && pagination.buttons.map(b =>
                              <PaginationButton key={b} content={b} onClick={() => pagination.changePageOn(--b)}
                                   className={b === (pagination.currentPage + 1) ? 'fs-1 color-white me-5 ms-5' : 'fs-3 color-silver me-3 ms-3'} />)
                    }
               </Col>
               <Col lg={1}>
                    {pagination.currentPage + 1 < pagination.pagesAmount &&
                    <Arrow onClick={() => pagination.changePageOnNext()}
                         size={3} direction={ArrowDirection.right} />
                         }
               </Col>
          </Row>
     );
});

export default ChallengePagination;