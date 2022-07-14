import { observer } from 'mobx-react';
import { useState } from 'react';
import { Form } from 'react-bootstrap';
import ChallengesTempStorageStore from '../../stores/ChallengesTempStorageStore';
import StreamersStore from '../../stores/components/StreamersStore';
import { ChallengeFormValidation } from '../../utilities/ChallengeFormValidation';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const ChallengeForm = observer(() => {

     const streamersStore = useInjection<StreamersStore>(iocStores.streamersStore);
     const challengesTempStorage = useInjection<ChallengesTempStorageStore>(iocStores.challengesTempStorageStore);
     const [formIsFocused, setFormIsFocused] = useState(false);

     return (
          <Form validated={formIsFocused} onBlur={() => setFormIsFocused(false)} onFocus={() => setFormIsFocused(true)}>
               <Form.Group>
                    <Form.Control onChange={e => challengesTempStorage.challengeForAdding.title = e.target.value}
                         placeholder="Title..." className="order_challenge__title"
                         spellCheck={false}
                         maxLength={ChallengeFormValidation.Title.maxLength}/>
               </Form.Group>
               <Form.Group>
                    <Form.Control onChange={e => challengesTempStorage.challengeForAdding.description = e.target.value}
                         placeholder="Description..." as="textarea"
                         spellCheck={false} required
                         maxLength={ChallengeFormValidation.Description.maxLength}
                         minLength={ChallengeFormValidation.Description.minLength}/>
               </Form.Group>
               <Form.Group>
                    <Form.Control type='number' onChange={e => challengesTempStorage.challengeForAdding.donatePrice = Number.parseFloat(e.target.value)}
                         placeholder="Donation Price..." className="order_challenge__price"
                         spellCheck={false} required
                         min={streamersStore.selectedStreamer.minDonatePrice}/>
               </Form.Group>
               <Form.Group>
                    <Form.Control onChange={e => challengesTempStorage.challengeForAdding.donateFrom = e.target.value}
                         placeholder="Your nickname..." className="order_challenge__nickname"
                         spellCheck={false} required
                         maxLength={ChallengeFormValidation.DonaterNickname.maxLength} />
               </Form.Group>
          </Form>
     );
});

export default ChallengeForm;