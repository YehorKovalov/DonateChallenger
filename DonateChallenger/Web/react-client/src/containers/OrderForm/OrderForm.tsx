import { observer } from 'mobx-react';
import React, { useEffect } from 'react';
import { Form } from 'react-bootstrap';
import OrderSearch from '../../components/StreamerSearch';
import ChallengeOrderStore from '../../stores/components/ChallengeOrderStore';
import StreamersStore from '../../stores/components/StreamersStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const OrderForm = observer(() => {
          
     const streamersStore = useInjection<StreamersStore>(iocStores.streamersStore);
     const challengeOrderStore = useInjection<ChallengeOrderStore>(iocStores.challengeOrderStore);

     useEffect(() => {
          const handleSearchingStreamersByNickname = async () => {
               await streamersStore.searchStreamersByNickname();
          }
          handleSearchingStreamersByNickname();
     }, [streamersStore.nicknameForSearching]);

     const handleFormMouseMovingEffect = (e: React.MouseEvent<HTMLDivElement>) => {
          e.currentTarget.style.setProperty('--x', `${ e.pageX - e.currentTarget.offsetLeft }px`)
          e.currentTarget.style.setProperty('--y', `${ e.pageY - e.currentTarget.offsetTop }px`)
     }

     return (
          <div className="order_form" onMouseMove={handleFormMouseMovingEffect}>
               <div className='pb-5'>
                    <div className='color-white fs-1 text-center mb-4'>Streamer nickname</div>
                    <OrderSearch/>
               </div>
               <div className="order_challenge">
                    <div>
                         <div className="color-white fs-1 text-center border-bottom mb-4">Challenge</div>
                    </div>
                    <div>
                         <Form.Control spellCheck={false} placeholder="Title..." className="order_challenge__title"
                              onChange={e => challengeOrderStore.title = e.target.value}/>
                         <Form.Control spellCheck={false} as="textarea" placeholder="Description..."
                                   onChange={e => challengeOrderStore.description = e.target.value}/>
                         <Form.Control spellCheck={false} placeholder="Donation Price..." className="order_challenge__price"
                                   onChange={e => challengeOrderStore.donatePrice = Number.parseFloat(e.target.value)}/>
                         <Form.Control spellCheck={false} placeholder="Your nickname..." className="order_challenge__nickname"
                                   onChange={e => challengeOrderStore.donateFrom = e.target.value}/>
                    </div>
               </div>
          </div>
     );
});

export default OrderForm;