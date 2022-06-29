import React from 'react';
interface PaginationButtonProps {
     content: number | string;
     className?: string;
     onClick?: React.MouseEventHandler<HTMLSpanElement>;
}

const PaginationButton = (props: PaginationButtonProps) => {
     return (
          <span role="button" className={props.className} onClick={props.onClick}>{props.content}</span>
     );
};

export default PaginationButton;