import React from 'react';
import './styles.css'

export enum ArrowDirection{
     right = 'right',
     left = 'left',
     down = 'down',
     up = 'up'
}

interface ArrowProps {
     direction: ArrowDirection,
     size:number,
     onClick: React.MouseEventHandler<HTMLButtonElement>
}

const Arrow = (props:ArrowProps) => {
     const arrowResultClassName = `arrow ${props.direction} p-${props.size}`;
     return (
          <i role='button' className={arrowResultClassName} onClick={props.onClick}></i>
     );
};

export default Arrow;