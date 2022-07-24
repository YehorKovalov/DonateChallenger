import { observer } from 'mobx-react';
import { useEffect } from 'react';
import AuthStore from '../../oidc/AuthStore';
import SelectUserRoleStore from '../../stores/global/SelectUserRoleStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const SelectUserRolePage = observer(() => {
     const roleStore = useInjection<SelectUserRoleStore>(iocStores.selectUserRoleStore);
     const authStore = useInjection<AuthStore>(iocStores.authStore);

     useEffect(() => {
          if (!authStore.user) {
               const fetch = async () => { await authStore.tryGetUser(); }
               fetch();
          }
     }, []);

     return (
          <div className='position-absolute top-50 start-50 translate-middle color-silver '>
               <span role={'button'} className='me-5 fs-1 hover_white' onClick={roleStore.continueAsStreamer}>I am streamer</span>
               <span role='button' className='fs-1 hover_white' onClick={roleStore.continueAsDonater}>I am donater</span>
          </div>
     );
});

export default SelectUserRolePage;