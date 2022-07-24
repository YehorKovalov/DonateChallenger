import { observer } from "mobx-react";
import { useEffect } from "react";
import { Container } from "react-bootstrap";
import StreamerProfileForm from "../../containers/StreamerProfileForm";
import UserProfileForm from "../../containers/UserProfileForm";
import { UserRole } from "../../models/UserRole";
import AuthStore from "../../oidc/AuthStore";
import { useInjection } from "../../utilities/ioc/ioc.react";
import iocStores from "../../utilities/ioc/iocStores";

const ProfilePage = observer(() => {

     const authStore = useInjection<AuthStore>(iocStores.authStore);

     useEffect(() => {
          if (!authStore.user) {
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }
     }, []);

     return (
          <Container>
               {authStore.userRole === UserRole.Streamer && <StreamerProfileForm/>}
               {authStore.userRole === UserRole.Donater && <UserProfileForm/>}
          </Container>
     );
});

export default ProfilePage;