import { observer } from 'mobx-react';
import CommentPaginationStore from '../../stores/components/CommentPaginationStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import Arrow from '../Arrow';
import { ArrowDirection } from '../Arrow/Arrow';

const CommentPagination = observer(() => {

     const pagination = useInjection<CommentPaginationStore>(iocStores.commentPaginationStore);

     return (
          <div>
               {pagination.currentPage - 1 >= 0 && <span className='me-4'>
                    <Arrow direction={ArrowDirection.left} size={2} onClick={() => pagination.changePageOnPrevious()}/>
               </span>}
               {pagination.currentPage + 1 < pagination.pagesAmount && <span className='ms-4'>
                    <Arrow direction={ArrowDirection.right} size={2} onClick={() => pagination.changePageOnNext()}/>
               </span>}
          </div>
     );
});

export default CommentPagination;