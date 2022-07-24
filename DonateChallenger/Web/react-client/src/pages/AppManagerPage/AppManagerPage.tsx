import { observer } from 'mobx-react';
import { useEffect } from 'react';
import { Container, Tab, Tabs } from 'react-bootstrap';
import ChallengeOrdersList from '../../containers/ChallengeOrdersList';
import ChallengesList from '../../containers/ChallengesList';
import CommentsList from '../../containers/CommentsList';
import AuthStore from '../../oidc/AuthStore';
import CatalogChallengeManagerStore from '../../stores/containers/CatalogChallengeManagerStore';
import ChallengeOrderManagerStore from '../../stores/containers/ChallengeOrderManagerStore';
import CommentManagerStore from '../../stores/containers/CommentManagerStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const AppManagerPage = observer(() => {

     const catalogChallengesManager = useInjection<CatalogChallengeManagerStore>(iocStores.catalogChallengeManagerStore);
     const commentManager = useInjection<CommentManagerStore>(iocStores.commentManagerStore);
     const challengeOrderManager = useInjection<ChallengeOrderManagerStore>(iocStores.challengeOrderManagerStore);

     const authStore = useInjection<AuthStore>(iocStores.authStore);
     
     const handleOnSelect = async (tabName: string | null) => {
          switch (tabName) {
               case "challenges":
                    await catalogChallengesManager.getPaginatedCurrentChallenges();
                    break;
               case "challenge-orders":
                    await challengeOrderManager.getPaginated();
                    break;
               case "comments":
                    await commentManager.getPaginated(0);
                    break;
          }
     }

     useEffect(() => {
          if (!authStore.user) {
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }
     }, [])


     return (
          <Container>
               <Tabs
                    defaultActiveKey="all"
                    className="mb-5 pt-5"
                    onSelect={handleOnSelect}
               >
                    <Tab eventKey="challenges" title="Challenges">
                         <ChallengesList />
                    </Tab>
                    <Tab eventKey="challenge-orders" title="Orders">
                         <ChallengeOrdersList />
                    </Tab>
                    <Tab eventKey="comments" title="Comments">
                         <CommentsList />
                    </Tab>
               </Tabs>
          </Container>
     );
});

export default AppManagerPage;