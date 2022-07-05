import { observer } from 'mobx-react';
import { Button, CloseButton, ListGroup, Toast } from 'react-bootstrap';
import './styles.css';

interface StreamerProfileFormControlProps {
     showControl: boolean;
     onClose: () => void;
     inputValue: string | number;
     onChangeInput: React.ChangeEventHandler<HTMLInputElement>;
     onSubmit: React.MouseEventHandler<HTMLSpanElement>;
     errorList?: string[];
}

const StreamerProfileForm = observer((props: StreamerProfileFormControlProps) => {

     return (
          <>
               <Toast show={props.showControl} onClose={props.onClose} className="bg-transparent">
                    <Toast.Body>
                         {props.errorList && props.errorList.length > 0 &&
                         <ListGroup variant="flush" className='pb-2 mb-2'>
                              {props.errorList.map(e => <ListGroup className='color-red fs-5'>{e}</ListGroup>)}
                         </ListGroup>
                         }
                         <input value={props.inputValue} className="ps-2 w-50 border border-success fs-5 me-5" onChange={props.onChangeInput}/>
                         <Button variant="outline-light" onClick={props.onSubmit}>Apply</Button>
                    </Toast.Body>
               </Toast>
          </>
     );
});

export default StreamerProfileForm;