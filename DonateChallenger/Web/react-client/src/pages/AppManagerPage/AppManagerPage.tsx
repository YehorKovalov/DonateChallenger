import { observer } from 'mobx-react';
import { useEffect } from 'react';
import { Container, Tab, Tabs } from 'react-bootstrap';
import SelectChallengeStatusTab from '../../components/SelectChallengeStatusTab';
import StreamerSearch from '../../components/StreamerSearch';
import ChallengeOrdersList from '../../containers/ChallengeOrdersList';
import CommentsList from '../../containers/CommentsList';
import AuthStore from '../../oidc/AuthStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import StreamersStore from '../../stores/states/StreamersStore';
import iocStores from '../../utilities/ioc/iocStores';
import AppManagerPageStore from '../../stores/pages/AppManagerPageStore';

const AppManagerPage = observer(() => {

     const authStore = useInjection<AuthStore>(iocStores.authStore);
     const streamersStore = useInjection<StreamersStore>(iocStores.streamersStore);
     const appManagerPageStore = useInjection<AppManagerPageStore>(iocStores.appManagerPageStore);
     

     useEffect(() => {
          if (!authStore.user) {
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }
     }, [])

     return (
          <Container>
               <Tabs
                    className="mb-5 pt-5"
                    onSelect={appManagerPageStore.handleOnSelect}
                    activeKey={appManagerPageStore.activeKey}
               >
                    <Tab eventKey="challenges" title="Challenges">
                         <StreamerSearch/>
                         <SelectChallengeStatusTab streamerId={streamersStore.selectedStreamer.streamerId}/>
                    </Tab>
                    <Tab eventKey="challenge-orders" title="Orders">
                         <ChallengeOrdersList />
                    </Tab>
                    <Tab eventKey="comments" title="Comments" disabled={appManagerPageStore.activeKey !== "comments"}>
                         <CommentsList />
                    </Tab>
               </Tabs>
          </Container>
     );
});

export default AppManagerPage;


