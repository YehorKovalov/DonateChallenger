import { Container, Nav, Navbar, NavDropdown } from 'react-bootstrap';
import Brand from '../Brand';
import NavBoardList from '../NavBoardList';
import NavLink from '../NavLink';
import UserLoginIcon from '../UserLoginIcon';
import './styles.css';

const Header = () => {
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
                              <NavBoardList />
                              <NavLink href='/signin' className='pt-5'><UserLoginIcon/></NavLink>
                         </Nav>
                    </Navbar.Collapse>
               </Container>
          </Navbar>
     );
};

export default Header;