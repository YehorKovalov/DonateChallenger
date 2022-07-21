import { observer } from 'mobx-react';
import { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
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
          <div className="w-75 position-absolute top-50 start-50 translate-middle">
               <Row>
                    <Col>
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
                    </Col>
               </Row>
          </div>
     );
});

export default UserProfileForm;