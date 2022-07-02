import { useEffect } from 'react';
import LoadingSpinner from '../../components/LoadingSpinner';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import AuthStore from '../AuthStore';

const SigninRedirectCallback = () => {
     const authStore = useInjection<AuthStore>(iocStores.authStore);
     useEffect(() => {
          const signinRedirectCallback = async (): Promise<void> => {
               await authStore.signinRedirectCallback();
          };
          signinRedirectCallback();
     }, [authStore, authStore.user]);

  return (
     <LoadingSpinner/>
  );
};

export default SigninRedirectCallback;