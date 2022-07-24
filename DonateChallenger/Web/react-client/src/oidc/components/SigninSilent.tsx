import { useEffect } from 'react';
import LoadingSpinner from '../../components/LoadingSpinner';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import AuthStore from '../AuthStore';

const SigninSilent = () => {
     const authStore = useInjection<AuthStore>(iocStores.authStore);
     useEffect(() => {
          const signinSilent = async () => { await authStore.signinSilent(); };
          signinSilent();
     }, [authStore, authStore.user]);
     return (
          <LoadingSpinner/>
     );
};

export default SigninSilent;