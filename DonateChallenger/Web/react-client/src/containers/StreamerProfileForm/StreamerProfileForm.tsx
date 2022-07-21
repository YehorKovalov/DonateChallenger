import { observer } from 'mobx-react';
import { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
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
          <div className="w-75 position-absolute top-50 start-50 translate-middle">
               <Row >
                    <Col>
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
                    </Col>
                    <Col>
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
                    </Col>
               </Row>
          </div>
     );
});

export default StreamerProfileForm;