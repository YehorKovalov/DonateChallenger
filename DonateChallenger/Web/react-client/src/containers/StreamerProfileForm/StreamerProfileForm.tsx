import { observer } from 'mobx-react';
import { useEffect, useState } from 'react';
import { Container } from 'react-bootstrap';
import ProfileFormControl from '../../components/ProfileFormControl';
import ToolTip from '../../components/ToolTip';
import StreamerProfileStore from '../../stores/containers/StreamerProfileStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const StreamerProfileForm = observer(() => {
     const profileStore = useInjection<StreamerProfileStore>(iocStores.streamerProfileStore);

     const [show, setShow] = useState(false);
     const toggleShow = () => setShow(!show);

     useEffect(() => {
          const fetchGetStreamerProfile = async () => { await profileStore.getStreamerProfile(); }
          fetchGetStreamerProfile();
     }, []);

     return (
          <Container className='pt-5'>
               <div className='d-flex flex-wrap justify-content-evenly'>
                    <div className='mb-5'>
                         <ToolTip onClick={toggleShow}
                              mainText={`Nickname: ${profileStore.profile.streamerNickname}`}
                              tipText={'Click to change'}/>
                         <ProfileFormControl
                              showControl={show}
                              onClose={toggleShow}
                              errorList={profileStore.nicknameInput.errors}
                              inputValue={profileStore.nicknameInput.state}
                              onChangeInput={(e) => profileStore.nicknameInput.state = e.target.value}
                              onSubmit={profileStore.changeNickname}
                         />
                    </div>
                    <div className='mb-5'>
                         <ToolTip onClick={toggleShow}
                              mainText={`Minimum donation price: ${profileStore.profile.minDonatePrice}`}
                              tipText={'Click to change'}/>
                         <ProfileFormControl
                              type='number'
                              showControl={show}
                              onClose={toggleShow}
                              errorList={profileStore.minDonatePriceInput.errors}
                              inputValue={profileStore.minDonatePriceInput.state}
                              onChangeInput={(e) => profileStore.minDonatePriceInput.state = Number.parseInt(e.target.value)}
                              onSubmit={profileStore.changeMinDonatePrice}
                         />
                    </div>
                    <div className='mb-5'>
                         <ToolTip onClick={toggleShow}
                              mainText={`Email: ${profileStore.profile.email}`}
                              tipText={'Click to change'}/>
                         <ProfileFormControl
                              showControl={show}
                              onClose={toggleShow}
                              errorList={profileStore.emailInput.errors}
                              inputValue={profileStore.emailInput.state}
                              onChangeInput={(e) => profileStore.emailInput.state = e.target.value}
                              onSubmit={profileStore.changeEmail}
                         />
                    </div>
               </div>
          </Container>
     );
});

export default StreamerProfileForm;