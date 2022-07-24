import { observer } from 'mobx-react';
import { Container, Dropdown, Table } from 'react-bootstrap';
import TableInput from '../../components/TableInput';
import CatalogChallengeManagerStore from '../../stores/containers/CatalogChallengeManagerStore';
import CommentManagerStore from '../../stores/containers/CommentManagerStore';
import AppManagerPageStore from '../../stores/pages/AppManagerPageStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

interface CompletedChallengesListProps {
     streamerId: string;
}

const CompletedChallengesList = observer((props: CompletedChallengesListProps) => {

     const catalogChallengesManager = useInjection<CatalogChallengeManagerStore>(iocStores.catalogChallengeManagerStore);
     const appManagerPageStore = useInjection<AppManagerPageStore>(iocStores.appManagerPageStore);
     const commentManager = useInjection<CommentManagerStore>(iocStores.commentManagerStore);

     return (
          <Container>
               <Table striped bordered hover variant="black fs-5">
                    <thead className='color-silver'>
                         <tr>
                              <th>Description</th>
                              <th>Donation From</th>
                              <th>Donate price</th>
                              <th>Title</th>
                              <th>Action</th>
                         </tr>
                    </thead>
                    <tbody>
                         {catalogChallengesManager.paginatedChallenges.data.map(s =>
                         <tr key={s.challengeId}>
                              <td>
                                   <TableInput value={s.description} onChange={(e) => s.description = e.target.value} />
                              </td>
                              <td>
                                   <TableInput value={s.donateFrom} onChange={(e) => s.donateFrom = e.target.value} />
                              </td>
                              <td>
                                   <TableInput value={s.donatePrice} onChange={(e) => s.donatePrice = Number.parseInt(e.target.value)} />
                              </td>
                              <td>
                                   <TableInput value={!s.title ? "" : s.title} onChange={(e) => s.title = e.target.value} />
                              </td>
                              <td>
                                   <Dropdown>
                                        <Dropdown.Toggle variant="outline-light" id="dropdown-basic">
                                             Action
                                        </Dropdown.Toggle>
                                        <Dropdown.Menu variant='dark'>
                                             <Dropdown.Item onClick={async () => await catalogChallengesManager.update(s.challengeId, props.streamerId)}>Update</Dropdown.Item>
                                             <Dropdown.Item onClick={async () => {
                                                  appManagerPageStore.activeKey = "comments";
                                                  await commentManager.getPaginated(s.challengeId);
                                             }}>
                                                  To comments</Dropdown.Item>
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

export default CompletedChallengesList;