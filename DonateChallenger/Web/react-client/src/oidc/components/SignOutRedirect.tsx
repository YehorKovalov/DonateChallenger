import { useEffect } from 'react';
import LoadingSpinner from '../../components/LoadingSpinner';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import AuthStore from '../AuthStore';

const SignOutRedirect = () => {
     const authStore = useInjection<AuthStore>(iocStores.authStore);
     useEffect(() => {
          const signoutRedirect = async (): Promise<void> => { await authStore.signoutRedirect(); };
          signoutRedirect();
     }, [authStore, authStore.user]);

  return (
     <LoadingSpinner/>
  );
};

export default SignOutRedirect;