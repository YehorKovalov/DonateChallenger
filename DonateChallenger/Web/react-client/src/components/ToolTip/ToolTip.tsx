import { observer } from 'mobx-react-lite';
import { OverlayTrigger, Tooltip } from 'react-bootstrap';

interface ToolTipProps {
     tipText: string;
     onClick: React.MouseEventHandler<HTMLSpanElement>;
     mainText: string;
}

const ToolTip = observer((props: ToolTipProps) => {
     const overlayTriggerPlacement = 'right';
     return (
          <OverlayTrigger key={overlayTriggerPlacement} placement={overlayTriggerPlacement} overlay={
               <Tooltip id={`tooltip-${overlayTriggerPlacement}`}>
                    <span className='fs-5'>{props.tipText}</span>
               </Tooltip>}
          >
               <span role='button' onClick={props.onClick} className="color-silver fs-2">
                    {props.mainText}
               </span>
          </OverlayTrigger>
          );
});

export default ToolTip;