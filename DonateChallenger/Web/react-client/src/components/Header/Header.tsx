import { observer } from 'mobx-react';
import { Container, Nav, Navbar } from 'react-bootstrap';
import { UserRole } from '../../models/UserRole';
import AuthStore from '../../oidc/AuthStore';
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

                              {authStore.userRole === UserRole.Anonymous &&
                              <>
                                   <NavLink href='/order'>Order</NavLink>
                              </>}

                              {authStore.userRole === UserRole.Donater &&
                              <>
                                   <NavLink href='/'>Profile</NavLink>
                                   <NavLink href='/order'>Order</NavLink>
                                   <NavLink href='/completed/forum'>Forum</NavLink>
                              </>}

                              {authStore.userRole === UserRole.Streamer &&
                              <>
                                   <NavLink href='/'>Profile</NavLink>
                                   <NavLink href='/order'>Order</NavLink>
                                   <NavBoardList />
                                   <NavLink href='/completed/forum'>Forum</NavLink>
                              </>}

                              {authStore.userRole === UserRole.Manager &&
                              <>
                                   <NavLink href='/'>Manage</NavLink>
                                   <NavLink href='/completed/forum'>Forum</NavLink>
                              </>}

                              {authStore.userRole === UserRole.Admin &&
                              <>
                                   <NavLink href='/admin'>Profiles</NavLink>
                              </>}

                              {authStore.user
                              ? <>
                                   <NavLink href='/signout' ><Logout/></NavLink>
                              </>
                              : <NavLink href='/signin'><UserLoginIcon/></NavLink> }
                          </Nav>
                    </Navbar.Collapse>
               </Container>
          </Navbar>
     );
});

export default Header;