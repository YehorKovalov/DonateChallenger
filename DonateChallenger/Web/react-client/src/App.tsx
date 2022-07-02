import './App.css';
import AppRoutes from './routes/AppRoutes';
import { observer } from 'mobx-react';
import { useInjection } from './utilities/ioc/ioc.react';
import AuthStore from './oidc/AuthStore';
import iocStores from './utilities/ioc/iocStores';
import { useEffect } from 'react';
const App = observer(() => {
  
  const authStore = useInjection<AuthStore>(iocStores.authStore);

  useEffect(() => {
    const fetchSigninSilent = async () => {
      await authStore.signinSilent();
      if (!authStore.user) {
        await authStore.tryGetUser();
      }
    }

    fetchSigninSilent();
  }, [authStore]);

  return (
    <div className="main-font-family">
          <AppRoutes />
    </div>
  );
});

export default App;
