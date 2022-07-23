import { observer } from 'mobx-react';
import { useEffect, useState } from 'react';
import { Container } from 'react-bootstrap';
import ProfileFormControl from '../../components/ProfileFormControl';
import ToolTip from '../../components/ToolTip';
import UserProfileStore from '../../stores/containers/UserProfileStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const UserProfileForm = observer(() => {

     const profileStore = useInjection<UserProfileStore>(iocStores.userProfileStore);

     useEffect(() => {
          const fetchGetStreamerProfile = async () => { await profileStore.getUserProfile(); }
          fetchGetStreamerProfile();
     }, []);

     const [show, setShow] = useState(false);
     const toggleShow = () => setShow(!show);

     return (
          <Container className='pt-5'>
               <div className='d-flex flex-wrap justify-content-evenly'>
                    <div className='mb-5'>
                         <ToolTip onClick={toggleShow}
                              mainText={`Nickname: ${profileStore.profile.userNickname}`}
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

export default UserProfileForm;