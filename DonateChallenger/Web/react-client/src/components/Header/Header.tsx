import { observer } from 'mobx-react';
import { Container, Nav, Navbar } from 'react-bootstrap';
import AuthStore from '../../oidc/AuthStore';
import StreamerProfile from '../../pages/StreamerProfilePage';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import Brand from '../Brand';
import Logout from '../Logout';
import NavBoardList from '../NavBoardList';
import NavLink from '../NavLink';
import UserLoginIcon from '../UserLoginIcon';
import './styles.css';

const Header = observer(() => {

     const authStore = useInjection<AuthStore>(iocStores.authStore);

     return (
          <Navbar expand='lg' variant='dark' className='header-bg-color text-center color-silver border-bottom'>
               <Container fluid>
                    <Navbar.Brand className='col-5'>
                              <Brand/>
                    </Navbar.Brand>
                    <div className='col-3'></div>
                    <Navbar.Toggle className='ms-auto' aria-controls="responsive-navbar-nav" />
                    <Navbar.Collapse id="navbarScroll">
                         <Nav className="col-4 fs-3" navbarScroll>
                              {authStore.user
                              ? <>
                                   <NavBoardList />
                                   <NavLink href='/profile'>Profile</NavLink>
                                   <NavLink href='/signout' ><Logout/></NavLink>
                              </>
                              :
                              <>
                                   <NavLink href='/signin'><UserLoginIcon/></NavLink>
                              </>
                              }
                          </Nav>
                    </Navbar.Collapse>
               </Container>
          </Navbar>
     );
});

export default Header;