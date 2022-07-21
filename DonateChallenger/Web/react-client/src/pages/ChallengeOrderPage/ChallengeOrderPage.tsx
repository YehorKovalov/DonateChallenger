import { observer } from "mobx-react";
import { useEffect } from "react";
import { Container } from "react-bootstrap";
import OrderForm from "../../containers/OrderForm";
import AuthStore from "../../oidc/AuthStore";
import ChallengesTempStorageStore from "../../stores/global/ChallengesTempStorageStore";
import { useInjection } from "../../utilities/ioc/ioc.react";
import iocStores from "../../utilities/ioc/iocStores";
import './styles.css';
const ChallengeOrderPage = observer(() => {

     const challengesTempStorageStore = useInjection<ChallengesTempStorageStore>(iocStores.challengesTempStorageStore);
     const authStore = useInjection<AuthStore>(iocStores.authStore);

     useEffect(() => {
          if (!authStore.user) {
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }
     }, []);

     useEffect(() => {
          const handleGettingChallengesFromTempStorage = async () => { await challengesTempStorageStore.getStorage() }
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