import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import ChallengesBoardStore from '../../stores/components/ChallengesBoardStore';
import ChallengesStore from '../../stores/components/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import Arrow from '../Arrow';
import { ArrowDirection } from '../Arrow/Arrow';
import PaginationButton from '../PaginationButton';

const Pagination = observer(() => {

     const challengesStore = useInjection<ChallengesStore>(iocStores.challengesStore);
     const boardStore = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);

     useEffect(() => {
          const fetch = async () => {
               await challengesStore.getChallengesByCurrentStatus();
          }

          fetch();
     }, [boardStore.currentPage]);

          return (
          <Row className='justify-content-md-center text-center pb-5 pt-5'>
               <Col lg={1}>
                    <Arrow direction={ArrowDirection.left} onClick={async () => await boardStore.changePageOnPrevious()} size={3} />
               </Col>
               <Col lg={2}>
                    {boardStore.buttons.map(b =>
                              <PaginationButton key={b} content={b} onClick={async () => await boardStore.changePageOn(--b)}
                                   className={b === (boardStore.currentPage + 1) ? 'fs-1 color-white me-5 ms-5' : 'fs-3 color-silver me-3 ms-3'} />)
                    }
               </Col>
               <Col lg={1}>
                    <Arrow direction={ArrowDirection.right} onClick={async () => await boardStore.changePageOnNext()} size={3}/>
               </Col>
          </Row>
     );
});

export default Pagination;