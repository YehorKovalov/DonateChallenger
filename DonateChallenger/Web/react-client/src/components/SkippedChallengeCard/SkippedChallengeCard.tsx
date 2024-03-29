import { Card, Col, Row } from 'react-bootstrap';
import DateTimeStore from '../../stores/components/DateTimeStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

export interface SkippedChallengeCardProps {
     challengeId: number,
     title?: string,
     description: string,
     donatePrice: number,
     donateFrom: string,
     createdTime: string
}

const SkippedChallengeCard = (props: SkippedChallengeCardProps) => {

     const dateTimeStore = useInjection<DateTimeStore>(iocStores.dateTimeStore);

     return (
          <Card bg='dark' key={props.challengeId} className='color-silver border'>
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
     );
};

export default SkippedChallengeCard;