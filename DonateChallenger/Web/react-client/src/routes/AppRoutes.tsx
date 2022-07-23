import { observer } from 'mobx-react';
import { Suspense } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LoadingSpinner from '../components/LoadingSpinner';
import Layout from '../containers/Layout';
import ChallengesPage from '../pages/ChallengesBoardPage';
import SelectUserRolePage from '../pages/SelectUserRolePage';
import AuthStore from '../oidc/AuthStore';
import { useInjection } from '../utilities/ioc/ioc.react';
import iocStores from '../utilities/ioc/iocStores';
import { SignInRedirect, SignOutRedirect } from '../oidc/components';
import SigninRedirectCallback from '../oidc/components/SigninRedirectCallback';
import SignoutRedirectCallback from '../oidc/components/SignoutRedirectCallback';
import SigninSilent from '../oidc/components/SigninSilent';
import OrderChallengePage from '../pages/ChallengeOrderPage';
import { UserRole } from '../models/UserRole';
import UserManagerPage from '../pages/UserManagerPage';
import ProfilePage from '../pages/ProfilePage';
import CompletedChallengesForumPage from '../pages/CompletedChallengesForumPage';

const AppRoutes = observer(() => {

     const authStore = useInjection<AuthStore>(iocStores.authStore);
     const getIndexJSX = (): JSX.Element => {
          switch (authStore.userRole) {
               case UserRole.Anonymous:
                    return <SelectUserRolePage/>;
               case UserRole.Donater:
                    return <ProfilePage />;
               case UserRole.Streamer:
                    return <ProfilePage />;
               case UserRole.Manager:
                    return <SelectUserRolePage/>;//will be added
               case UserRole.Admin:
                    return <UserManagerPage />;
          }
     }

     return (
          <Suspense fallback={ <LoadingSpinner /> }>
               <Router>
                    <Routes>
                         <Route path='/' element={ <Layout /> }>
                              <Route index element={getIndexJSX()}/>
                              <Route path="/signin" element={ <SignInRedirect /> } />
                              <Route path="/signin-oidc" element={ <SigninRedirectCallback /> } />
                              <Route path="/silentrenew" element={ <SigninSilent/> } />
                              <Route path="/signout" element={ <SignOutRedirect /> } />
                              <Route path="/signout-oidc" element={ <SignoutRedirectCallback /> } />
                              <Route path="/challenges" element={ <ChallengesPage /> } />
                              <Route path="/order" element={ <OrderChallengePage /> } />
                              <Route path="/admin" element={ <UserManagerPage /> } />
                              <Route path="/completed/forum" element={ <CompletedChallengesForumPage /> }/>
                         </Route>
                    </Routes>
               </Router>
          </Suspense>
     );
});

export default AppRoutes;