import { observer } from 'mobx-react';
import { useEffect } from 'react';
import AuthStore from '../../oidc/AuthStore';
import UserRoleStore from '../../stores/global/UserRoleStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const SelectUserRolePage = observer(() => {
     const roleStore = useInjection<UserRoleStore>(iocStores.userRoleStore);
     const authStore = useInjection<AuthStore>(iocStores.authStore);

     useEffect(() => {
          const getAuthentication = async (): Promise<void> => { await authStore.tryGetUser(); };
          getAuthentication();
        }, [authStore]);

     return (
          <div className='position-absolute top-50 start-50 translate-middle color-silver '>
               <span role={'button'} className='me-5 fs-1 hover_white' onClick={roleStore.continueAsStreamer}>I am streamer</span>
               <span role='button' className='fs-1 hover_white' onClick={roleStore.continueAsDonater}>I am donater</span>
          </div>
     );
});

export default SelectUserRolePage;