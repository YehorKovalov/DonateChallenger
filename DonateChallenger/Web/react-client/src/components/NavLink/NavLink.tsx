import { Nav, NavItem } from 'react-bootstrap';

interface NavLinkProps {
     children: JSX.Element | string,
     href: string,
     className?: string
}

const NavLink = (props: NavLinkProps) => {
     var resultClassName = `color-silver hover_white ${props.className}`;
     return (
          <NavItem>
               <Nav.Link href={props.href}><span className={resultClassName}>{props.children}</span></Nav.Link>
          </NavItem>
     );
};

export default NavLink;