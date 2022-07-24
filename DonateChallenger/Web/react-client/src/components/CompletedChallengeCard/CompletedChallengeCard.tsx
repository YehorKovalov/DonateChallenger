import { observer } from 'mobx-react';
import { Card, Col, OverlayTrigger, Row, Tooltip } from 'react-bootstrap';
import DateTimeStore from '../../stores/components/DateTimeStore';
import CommentsBlockStore from '../../stores/containers/CommentsBlockStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

export interface CompletedChallengeCardProps {
     challengeId: number,
     title?: string,
     description: string,
     donatePrice: number,
     donateFrom: string,
     createdTime: string
}

const CompletedChallengeCard = observer((props: CompletedChallengeCardProps) => {

     const commentBlockStore = useInjection<CommentsBlockStore>(iocStores.commentsBlockStore);
     const dateTimeStore = useInjection<DateTimeStore>(iocStores.dateTimeStore);

     const handleClick = async () => {
          commentBlockStore.showBlock = true;
          await commentBlockStore.getComments(props.challengeId);
     }

     return (
          <OverlayTrigger
               placement="top"
               overlay={<Tooltip id="button-tooltip-2" className='color-silver fs-4'>Show comments</Tooltip>}
          >
               <Card bg='dark' className='color-silver hover_white border' role={"button"}
                         key={props.challengeId} onClick={async () => await handleClick()}>
                    <Card.Body>
                         <Row className='mb-2'>
                              <Col lg={8} className='donate-donater text-center'>{props.donateFrom}</Col>
                              <Col lg={4}>{dateTimeStore.getUserFriendlyDateTime(props.createdTime)}</Col>
                         </Row>
                         <div className='donate-price'>{props.donatePrice}$</div>
                         <div><span className='color-silver'>{props.title}</span></div>
                         <div className='border-top pt-4 pb-5 mt-2 pe-1 ps-2'>
                              <span className='donate-description'>{props.description}</span>
                         </div>
                    </Card.Body>
               </Card>
        </OverlayTrigger>
     );
});

export default CompletedChallengeCard;