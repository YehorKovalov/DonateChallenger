import { observer } from 'mobx-react';
import { Suspense } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LoadingSpinner from '../components/LoadingSpinner';
import Layout from '../containers/Layout';
import ChallengesPage from '../pages/ChallengesPage';
import SelectUserRolePage from '../pages/SelectUserRolePage';
import AuthStore from '../stores/AuthStore';
import { useInjection } from '../utilities/ioc/ioc.react';
import iocStores from '../utilities/ioc/iocStores';

const AppRoutes = observer(() => {

     const authStore = useInjection<AuthStore>(iocStores.authStore);
     
     return (
          <Suspense fallback={ <LoadingSpinner /> }>
               <Router>
                    <Routes>
                         <Route path='/' element={ <Layout /> }>
                              {authStore.streamerAuthenticated
                                   ? <Route index element={ <ChallengesPage /> } />
                                   : <Route index element={ <SelectUserRolePage/> } />}
                         </Route>
                    </Routes>
               </Router>
          </Suspense>
     );
});

export default AppRoutes;