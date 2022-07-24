import { observer } from 'mobx-react';
import { Container, Dropdown, Table } from 'react-bootstrap';
import TableInput from '../../components/TableInput';
import CommentManagerStore from '../../stores/containers/CommentManagerStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const CommentsList = observer(() => {

     const commentManager = useInjection<CommentManagerStore>(iocStores.commentManagerStore);

     return (
          <Container>
               <Table striped bordered hover variant="black fs-5">
                    <thead className='color-silver'>
                         <tr>
                              <th>Message</th>
                              <th>User</th>
                              <th>Challenge</th>
                              <th>Created</th>
                              <th>Action</th>
                         </tr>
                    </thead>
                    <tbody>
                         {commentManager.paginatedComments.data.map(s => 
                         <tr key={s.commentId}>
                              <td>
                                   <TableInput value={s.message} onChange={(e) => s.message = e.target.value} />
                              </td>
                              <td>
                                   <TableInput value={s.userId} onChange={(e) => s.userId = e.target.value} />
                              </td>
                              <td>
                                   <TableInput value={s.challengeId} onChange={(e) => s.challengeId = Number.parseInt(e.target.value)} />
                              </td>
                              <td>
                                   <div>{s.date}</div>
                              </td>
                              <td>
                                   <Dropdown>
                                        <Dropdown.Toggle variant="outline-light" id="dropdown-basic">
                                             Action
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu variant='dark'>
                                             <Dropdown.Item onClick={async () => await commentManager.update(s.commentId)}>Update</Dropdown.Item>
                                             <Dropdown.Item onClick={async () => await commentManager.delete(s.commentId)}>Delete</Dropdown.Item>
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

export default CommentsList;