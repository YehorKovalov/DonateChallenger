import { observer } from 'mobx-react-lite';
import { Container, Dropdown, Table } from 'react-bootstrap';
import MovableInput from '../../components/MovableInput';
import TableInput from '../../components/TableInput';
import ToolTip from '../../components/ToolTip';
import UserManagerStore from '../../stores/containers/UserManagerStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const StreamersList = observer(() => {

     const userManager = useInjection<UserManagerStore>(iocStores.userManagerStore);

     return (
          <Container>
               <Table striped bordered hover variant="black">
                    <thead className='color-silver'>
                         <tr>
                              <th>Email</th>
                              <th>Nickname</th>
                              <th>minDonatePrice</th>
                              <th>MerchantId</th>
                              <th>Action</th>
                         </tr>
                    </thead>
                    <tbody>
                         {userManager.streamers.users.map(s => 
                         <tr key={s.streamerId}>
                              <td>
                                   <TableInput value={s.email} onChange={(e) => s.email = e.target.value} />
                              </td>
                              <td>
                                   <TableInput value={s.streamerNickname} onChange={(e) => s.streamerNickname = e.target.value} />
                              </td>
                              <td>
                                   <TableInput value={s.minDonatePrice} onChange={(e) => s.minDonatePrice = Number.parseInt(e.target.value)} />
                              </td>
                              <td>
                                   <TableInput value={s.merchantId} onChange={(e) => s.merchantId = e.target.value} />
                              </td>
                              <td>
                                   <Dropdown>
                                        <Dropdown.Toggle variant="outline-light" id="dropdown-basic">
                                             Action
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu variant='dark'>
                                             <Dropdown.Item onClick={async () => await userManager.updateStreamerProfile(s.streamerId)}>Update</Dropdown.Item>
                                             <Dropdown.Item onClick={async () => await userManager.delete(s.streamerId)}>Delete</Dropdown.Item>
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

export default StreamersList;