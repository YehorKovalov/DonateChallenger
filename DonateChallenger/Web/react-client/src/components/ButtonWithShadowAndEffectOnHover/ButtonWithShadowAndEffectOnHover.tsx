import React from 'react';
import './styles.css';

interface ButtonWithShadowAndEffectOnHoverProps {
     title: string,
     onClick?: React.MouseEventHandler<HTMLAnchorElement>;
     className?: string;
}

const ButtonWithShadowAndEffectOnHover = (props: ButtonWithShadowAndEffectOnHoverProps) => {

     const resultClassName = `btn ${props.className}`;

     return (
          <span role='button' onClick={props.onClick} className={resultClassName}>
               <span className='btn-text'>{props.title}</span>
          </span>
     );
};

export default ButtonWithShadowAndEffectOnHover;