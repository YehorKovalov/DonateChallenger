import { observer } from 'mobx-react';
import { Suspense, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LoadingSpinner from '../components/LoadingSpinner';
import Layout from '../containers/Layout';
import ChallengesPage from '../pages/ChallengesPage';
import SelectUserRolePage from '../pages/SelectUserRolePage';
import AuthStore from '../oidc/AuthStore';
import { useInjection } from '../utilities/ioc/ioc.react';
import iocStores from '../utilities/ioc/iocStores';
import { SignInRedirect, SignOutRedirect } from '../oidc/components';
import SigninRedirectCallback from '../oidc/components/SigninRedirectCallback';
import SignoutRedirectCallback from '../oidc/components/SignoutRedirectCallback';

const AppRoutes = observer(() => {

     const authStore = useInjection<AuthStore>(iocStores.authStore);

     return (
          <Suspense fallback={ <LoadingSpinner /> }>
               <Router>
                    <Routes>
                         <Route path='/' element={ <Layout /> }>
                              <Route index element={authStore.user ? <ChallengesPage /> : <SelectUserRolePage/> }/>
                              <Route path="/signin" element={ <SignInRedirect /> } />
                              <Route path="/signout" element={ <SignOutRedirect /> } />
                              <Route path="/signin/callback" element={ <SigninRedirectCallback /> } />
                              <Route path="/logout/callback" element={ <SignoutRedirectCallback /> } />
                         </Route>
                    </Routes>
               </Router>
          </Suspense>
     );
});

export default AppRoutes;