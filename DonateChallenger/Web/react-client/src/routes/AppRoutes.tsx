import { observer } from 'mobx-react';
import { Suspense } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LoadingSpinner from '../components/LoadingSpinner';
import Layout from '../containers/Layout';
import ChallengesPage from '../pages/ChallengesPage';
import SelectUserRolePage from '../pages/SelectUserRolePage';
import AuthStore from '../oidc/AuthStore';
import { useInjection } from '../utilities/ioc/ioc.react';
import iocStores from '../utilities/ioc/iocStores';
import { SignInRedirect, SignInSilent, SignOutRedirect } from '../oidc/components';

const AppRoutes = observer(() => {

     const authStore = useInjection<AuthStore>(iocStores.authStore);
     
     return (
          <Suspense fallback={ <LoadingSpinner /> }>
               <Router>
                    <Routes>
                         <Route path='/' element={ <Layout /> }>
                              <Route index element={authStore.userIsAuthenticated ? <ChallengesPage /> : <SelectUserRolePage/> }/>
                              <Route path="/signin" element={ <SignInRedirect />} />
                              <Route path="/signout" element={ <SignOutRedirect />} />
                              <Route path="/signinsilent" element={ <SignInSilent />} />
                         </Route>
                    </Routes>
               </Router>
          </Suspense>
     );
});

export default AppRoutes;