import { observer } from 'mobx-react';
import { Card, Col, Row, Stack } from 'react-bootstrap';
import DateTimeStore from '../../stores/components/DateTimeStore';
import ChallengeStore from '../../stores/states/ChallengeStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import ButtonWithMovableBorder from '../ButtonWithMovableBorder';
import ButtonWithShadowAndEffectOnHover from '../ButtonWithShadowAndEffectOnHover';
import './styles.css';

export interface CurrentChallengeCardProps {
     challengeId: number,
     title?: string,
     description: string,
     donatePrice: number,
     donateFrom: number,
     createdTime: string
}

const CurrentChallengeCard = observer((props: CurrentChallengeCardProps) => {

     const challengeStore = useInjection<ChallengeStore>(iocStores.challengeStore);
     const dateTimeStore = useInjection<DateTimeStore>(iocStores.dateTimeStore);
     const className = props.challengeId === challengeStore.lastUsedChallengeId ? 'color-silver border  blur' : 'color-silver border';

     return (
          <Card bg='dark' key={props.challengeId} className={className}>
               <Card.Body>
                    <Stack direction='horizontal'>
                         <div className='donate-donater'>{props.donateFrom}</div>
                         <div className='ms-auto'>{dateTimeStore.getUserFriendlyDateTime(props.createdTime)}</div>
                    </Stack>
                    <div className='donate-price'>{props.donatePrice}$</div>
                    <div><span className='color-silver'>{props.title}</span></div>
                    <div className='border-top border-bottom pt-4 pb-5 mt-2 pe-1 ps-2'>
                         <span className='donate-description'>{props.description}</span>
                    </div>

                    <Row className='mt-3 ms-4'>
                         <Col lg={8}>
                              <ButtonWithMovableBorder title='Completed'
                                   onClick={async () => await challengeStore.completeChallenge(props.challengeId)}/>
                         </Col>
                         <Col>
                         <ButtonWithShadowAndEffectOnHover title='Skip'
                              onClick={async () => await challengeStore.skipChallenge(props.challengeId)}/>
                         </Col>
                    </Row>
               </Card.Body>
          </Card>
     );
});

export default CurrentChallengeCard;