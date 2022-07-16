import { observer } from 'mobx-react';
import { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import StreamerProfileFormControl from '../../components/StreamerProfileFormControl';
import ToolTip from '../../components/ToolTip';
import StreamerProfileStore from '../../stores/components/StreamerProfileStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const ProfileForm = observer(() => {
     const streamerProfileStore = useInjection<StreamerProfileStore>(iocStores.streamerProfileStore);

     const [show, setShow] = useState(false);
     const toggleShow = () => setShow(!show);

     useEffect(() => {
          const fetchGetStreamerProfile = async () => { await streamerProfileStore.getStreamerProfile(); }
          fetchGetStreamerProfile();
     }, []);

     return (
          <div className="w-75 position-absolute top-50 start-50 translate-middle">
               <Row>
                    <Col>
                         <ToolTip onClick={toggleShow}
                              mainText={`Minimum donation price: ${streamerProfileStore.profile.minDonatePrice}`}
                              tipText={'Click to change'}/>
                         <StreamerProfileFormControl
                              showControl={show}
                              onClose={toggleShow}
                              errorList={streamerProfileStore.minDonatePriceInput.errors}
                              inputValue={streamerProfileStore.minDonatePriceInput.state}
                              onChangeInput={(e) => streamerProfileStore.minDonatePriceInput.state = e.target.value}
                              onSubmit={streamerProfileStore.changeMinDonatePrice}
                         />
                    </Col>
                    <Col>
                         <ToolTip onClick={toggleShow}
                              mainText={`Nickname: ${streamerProfileStore.profile.streamerNickname}`}
                              tipText={'Click to change'}/>
                         <StreamerProfileFormControl
                              showControl={show}
                              onClose={toggleShow}
                              errorList={streamerProfileStore.nicknameInput.errors}
                              inputValue={streamerProfileStore.nicknameInput.state}
                              onChangeInput={(e) => streamerProfileStore.nicknameInput.state = e.target.value}
                              onSubmit={streamerProfileStore.changeNickname}
                         />
                    </Col>
               </Row>
          </div>
     );
});

export default ProfileForm;