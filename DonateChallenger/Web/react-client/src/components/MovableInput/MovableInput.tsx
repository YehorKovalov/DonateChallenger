import React from 'react';
import './styles.css';
export interface MovableInputProps {
     onChange: React.ChangeEventHandler<HTMLInputElement>;
     className?: string;
     value?: string | number;
     placeholder?: string;
     onFocus?: React.FocusEventHandler<HTMLInputElement>;
     onBlur?: React.ReactEventHandler<HTMLInputElement>;
}

const MovableInput = (props: MovableInputProps) => {
     const className = `movable_input ${props.className}`
     return (
          <input onBlur={props.onBlur} onFocus={props.onFocus} placeholder={props.placeholder} value={props.value} spellCheck={false} className={className} onChange={props.onChange}/>
     );
};

export default MovableInput;