import './styles.css';

interface ButtonWithMovableBorderProps {
     title: string,
     onClick?: React.MouseEventHandler<HTMLAnchorElement>;
     className?: string;
}

const ButtonWithMovableBorder = (props: ButtonWithMovableBorderProps) => {
     const resultClassName=`animated-button ${props.className}`;
     return (
          <a role='button' className={resultClassName} onClick={props.onClick}>
               <span></span>
               <span></span>
               <span></span>
               <span></span>
               {props.title}
          </a>
     );
};

export default ButtonWithMovableBorder;