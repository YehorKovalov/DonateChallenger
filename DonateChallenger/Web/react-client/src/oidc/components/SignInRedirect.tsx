import { useEffect } from 'react';
import LoadingSpinner from '../../components/LoadingSpinner';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import AuthStore from '../AuthStore';

const SignInRedirect = () => {
     const authStore = useInjection<AuthStore>(iocStores.authStore);
     useEffect(() => {
          const signinRedirect = async () => { await authStore.signinRedirect(); };
          signinRedirect();
     }, [authStore, authStore.user]);
     return (
          <LoadingSpinner/>
     );
};

export default SignInRedirect;