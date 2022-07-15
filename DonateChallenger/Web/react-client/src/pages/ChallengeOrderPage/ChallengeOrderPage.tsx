import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { Container } from "react-bootstrap";
import OrderForm from "../../containers/OrderForm";
import ChallengesTempStorageStore from "../../stores/ChallengesTempStorageStore";
import { useInjection } from "../../utilities/ioc/ioc.react";
import iocStores from "../../utilities/ioc/iocStores";
import './styles.css';
const ChallengeOrderPage = observer(() => {
     const challengesTempStorageStore = useInjection<ChallengesTempStorageStore>(iocStores.challengesTempStorageStore);
     useEffect(() => {
          const handleGettingChallengesFromTempStorage = async () => {
               await challengesTempStorageStore.getStorage()
          }
          handleGettingChallengesFromTempStorage();
     }, []);
     return (
          <Container>
               <Container className="center-block">
                    <OrderForm/>
               </Container>
          </Container>
     );
});

export default ChallengeOrderPage;