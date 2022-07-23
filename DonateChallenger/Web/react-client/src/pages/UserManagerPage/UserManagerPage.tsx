import { observer } from 'mobx-react';
import { useEffect } from 'react';
import { Container, Tab, Tabs } from 'react-bootstrap';
import StreamersList from '../../containers/StreamersList';
import UsersList from '../../containers/UsersList';
import AuthStore from '../../oidc/AuthStore';
import UserManagerStore from '../../stores/containers/UserManagerStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const UserManagerPage = observer(() => {

     const userManager = useInjection<UserManagerStore>(iocStores.userManagerStore);
     const authStore = useInjection<AuthStore>(iocStores.authStore);

     const handleOnSelect = async (tabName: string | null) => {
          switch (tabName) {
               case "all":
                    await userManager.getAll();
                    break;
               case "streamers":
                    await userManager.getStreamers();
                    break;
          }
     }

     useEffect(() => {
          if (!authStore.user) {
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }

          const fetch = async () => { await userManager.getAll(); }
          fetch();
     }, []);

     return (
          <Container>
               <Tabs
                    defaultActiveKey="all"
                    className="mb-5 pt-5"
                    onSelect={handleOnSelect}
               >
                    <Tab eventKey="all" title="All">
                         <UsersList/>
                    </Tab>
                    <Tab eventKey="streamers" title="Streamers">
                         <StreamersList/>
                    </Tab>
               </Tabs>
          </Container>
     );
});

export default UserManagerPage;