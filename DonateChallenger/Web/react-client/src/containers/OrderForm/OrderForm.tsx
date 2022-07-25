import { observer } from 'mobx-react';
import React from 'react';
import ButtonWithMovableBorder from '../../components/ButtonWithMovableBorder';
import ChallengeForm from '../../components/ChallengeForm';
import LoadingSpinner from '../../components/LoadingSpinner';
import StreamerSearch from '../../components/StreamerSearch';
import ChallengeOrderStore from '../../stores/containers/ChallengeOrderStore';
import StreamersStore from '../../stores/states/StreamersStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const OrderForm = observer(() => {

     const challengeOrderStore = useInjection<ChallengeOrderStore>(iocStores.challengeOrderStore);
     const streamersStore = useInjection<StreamersStore>(iocStores.streamersStore);

     const handleFormMouseMovingEffect = (e: React.MouseEvent<HTMLDivElement>) => {
          e.currentTarget.style.setProperty('--x', `${ e.pageX - e.currentTarget.offsetLeft }px`)
          e.currentTarget.style.setProperty('--y', `${ e.pageY - e.currentTarget.offsetTop }px`)
     }

     return (
          <div className="order_form" onMouseMove={handleFormMouseMovingEffect}>
               {!challengeOrderStore.storageIsUpdating
               ?
               <>
                    <div className='order_form__wrapper'>
                         <div className='pb-5'>
                              <StreamerSearch/>
                         </div>
                         <div className="order_challenge">
                              <div className="color-white fs-1 text-center border-bottom mb-4">Challenge</div>
                              <ChallengeForm formDisabled={!streamersStore.selectedStreamer.streamerId}/>
                         </div>
                    </div>
                    <ButtonWithMovableBorder onClick={challengeOrderStore.makeOrder}
                         title='Send' className='d-block m-auto p-auto'/>
               </>
               : 
               <LoadingSpinner/>}
          </div>
     );
});

export default OrderForm;