import { observer } from 'mobx-react';
import { Card } from 'react-bootstrap';
import DateTimeStore from '../../stores/components/DateTimeStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import './styles.css';

interface CommentProps {
     message: string;
     date: string;
     username: string;
     userId: string;
     commentId: number;
     onClick?: React.MouseEventHandler<HTMLElement>;
}

const Comment = observer((props: CommentProps) => {
     const dateTimeStore = useInjection<DateTimeStore>(iocStores.dateTimeStore);

     return (
          <Card key={props.commentId} className="bg-transparent color-silver">
               <Card.Header className='border-top'>
                    <span className='m-5'>{props.username}</span>
               </Card.Header>
               <Card.Body>
                    <span>{props.message}</span>
               </Card.Body>
               <Card.Footer className='text-end card-body'>
                    <span>{dateTimeStore.getUserFriendlyDateTime(props.date)}</span>
               </Card.Footer>
          </Card>
     );
});

export default Comment;