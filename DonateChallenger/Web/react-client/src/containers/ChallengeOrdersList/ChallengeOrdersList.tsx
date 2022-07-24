import { observer } from 'mobx-react';
import { Container, Dropdown, Table } from 'react-bootstrap';
import TableInput from '../../components/TableInput';
import ChallengeOrderManagerStore from '../../stores/containers/ChallengeOrderManagerStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const ChallengeOrdersList = observer(() => {

     const challengeOrderManager = useInjection<ChallengeOrderManagerStore>(iocStores.challengeOrderManagerStore);

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
                         {challengeOrderManager.paginatedOrders.data.map(s => 
                         <tr key={s.challengeOrderId}>
                              <td>
                                   <TableInput value={s.challengesAmount} onChange={(e) => s.challengesAmount = Number.parseInt(e.target.value)} />
                              </td>
                              <td>
                                   <TableInput value={s.paymentId} onChange={(e) => s.paymentId = e.target.value} />
                              </td>
                              <td>
                                   <TableInput value={s.resultDonationPrice} onChange={(e) => s.resultDonationPrice = Number.parseInt(e.target.value)} />
                              </td>
                              <td>
                                   <Dropdown>
                                        <Dropdown.Toggle variant="outline-light" id="dropdown-basic">
                                             Action
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu variant='dark'>
                                             <Dropdown.Item onClick={async () => await challengeOrderManager.update(s.challengeOrderId)}>Update</Dropdown.Item>
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

export default ChallengeOrdersList;