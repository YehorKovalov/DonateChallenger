import { observer } from 'mobx-react';
import { useEffect, useState } from 'react';
import { Form } from 'react-bootstrap';
import AuthStore from '../../oidc/AuthStore';
import ChallengeForAddingStore from '../../stores/states/ChallengeForAddingStore';
import StreamersStore from '../../stores/states/StreamersStore';
import { ChallengeFormValidation } from '../../utilities/ChallengeFormValidation';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const ChallengeForm = observer(() => {

     const streamersStore = useInjection<StreamersStore>(iocStores.streamersStore);
     const authStore = useInjection<AuthStore>(iocStores.authStore);
     const challengeForAddingStore = useInjection<ChallengeForAddingStore>(iocStores.challengeForAddingStore);
     const [formIsFocused, setFormIsFocused] = useState(false);

     useEffect(() => {
          if (!authStore.user){
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }
     }, []);

     return (
          <Form validated={formIsFocused} onBlur={() => setFormIsFocused(false)} onFocus={() => setFormIsFocused(true)}>
               <Form.Group>
                    <Form.Control onChange={e => challengeForAddingStore.title.state = e.target.value}
                         placeholder="Title..." className="order_challenge__title"
                         spellCheck={false}
                         maxLength={ChallengeFormValidation.Title.maxLength}/>
               </Form.Group>
               <Form.Group>
                    <Form.Control onChange={e => challengeForAddingStore.description.state = e.target.value}
                         placeholder="Description..." as="textarea"
                         spellCheck={false} required
                         maxLength={ChallengeFormValidation.Description.maxLength}
                         minLength={ChallengeFormValidation.Description.minLength}/>
               </Form.Group>
               <Form.Group>
                    <Form.Control type='number' onChange={e => challengeForAddingStore.donatePrice.state = Number.parseFloat(e.target.value)}
                         placeholder="Donation Price..." className="order_challenge__price"
                         spellCheck={false} required
                         min={streamersStore.selectedStreamer.minDonatePrice}/>
               </Form.Group>
          </Form>
     );
});

export default ChallengeForm;