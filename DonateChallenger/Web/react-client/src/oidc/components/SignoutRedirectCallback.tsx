import { useEffect } from 'react';
import LoadingSpinner from '../../components/LoadingSpinner';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import AuthStore from '../AuthStore';

const SignoutRedirectCallback = () => {
     const authStore = useInjection<AuthStore>(iocStores.authStore);
     useEffect(() => {
          const signinRedirectCallback = async (): Promise<void> => {
               await authStore.signinRedirectCallback();
          };
          signinRedirectCallback().catch((error) => console.log(error));
     }, [authStore, authStore.user]);

  return (
     <LoadingSpinner/>
  );
};

export default SignoutRedirectCallback;