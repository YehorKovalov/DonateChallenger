import { observer } from 'mobx-react';
import { Container, Dropdown, Table } from 'react-bootstrap';
import TableInput from '../../components/TableInput';
import UserManagerStore from '../../stores/containers/UserManagerStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const UsersList = observer(() => {

     const userManager = useInjection<UserManagerStore>(iocStores.userManagerStore);

     return (
          <Container>
               <Table striped bordered hover variant="black fs-5">
                    <thead className='color-silver'>
                         <tr>
                              <th>Email</th>
                              <th>Username</th>
                              <th>Action</th>
                         </tr>
                    </thead>
                    <tbody>
                         {userManager.users.users.map(s => 
                         <tr key={s.userId}>
                              <td>
                                   <TableInput value={s.email} onChange={(e) => s.email = e.target.value} />
                              </td>
                              <td>
                                   <TableInput value={s.userNickname} onChange={(e) => s.userNickname = e.target.value} />
                              </td>
                              <td>
                                   <Dropdown>
                                        <Dropdown.Toggle variant="outline-light" id="dropdown-basic">
                                             Action
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu variant='dark'>
                                             <Dropdown.Item onClick={async () => await userManager.updateUserProfile(s.userId)}>Update</Dropdown.Item>
                                             <Dropdown.Item onClick={async () => await userManager.delete(s.userId)}>Delete</Dropdown.Item>
                                        </Dropdown.Menu>
                                   </Dropdown>
                              </td>
                         </tr>
                         )}
                    </tbody>
               </Table>
          </Container>
     );
});

export default UsersList;