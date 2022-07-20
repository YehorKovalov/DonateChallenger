import { observer } from 'mobx-react';
import { Form, Offcanvas } from 'react-bootstrap';
import ButtonWithMovableBorder from '../../components/ButtonWithMovableBorder';
import Comment from '../../components/Comment';
import AuthStore from '../../oidc/AuthStore';
import CommentsBlockStore from '../../stores/containers/CommentsBlockStore';
import CommentsStore from '../../stores/states/CommentsStore';
import CommentStore from '../../stores/states/CommentStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

const CommentsBlock = observer(() => {
     const commentsStore = useInjection<CommentsStore>(iocStores.commentsStore);
     const commentsBlockStore = useInjection<CommentsBlockStore>(iocStores.commentsBlockStore);
     const authStore = useInjection<AuthStore>(iocStores.authStore);
     const commentStore = useInjection<CommentStore>(iocStores.commentStore);

     const handleClose = () => commentsBlockStore.showBlock = false;
     
     return (
          <Offcanvas show={commentsBlockStore.showBlock} onHide={handleClose}>
               <Offcanvas.Header closeButton className='border-bottom color-silver fs-4'>
                    <span>Comments</span>
               </Offcanvas.Header>
               <Offcanvas.Body>
                    {commentsStore.paginatedComments?.data.map(c =>
                    <Comment key={c.commentId}
                         commentId={c.commentId}
                         userId={c.userId}
                         username={c.username}
                         date={c.date}
                         message={c.message}/>
                    )}
               </Offcanvas.Body>
               {authStore.user && <div className='add-comment_container'>
                    <Form.Group>
                         <Form.Control as="textarea"
                              placeholder="Add comment..."
                              spellCheck={false}
                              value={commentStore.message}
                              onChange={(e) => commentStore.message = e.target.value}/>
                    </Form.Group>
                    <ButtonWithMovableBorder className='w-100' title='Add' onClick={() => commentsBlockStore.addComment()}/>
               </div>}
          </Offcanvas>
     );
});

export default CommentsBlock;