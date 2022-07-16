interface ErrorMessageLabelProps {
     message: string
}

const ErrorMessageLabel = (props: ErrorMessageLabelProps) => {
     return (
          <label className='text-danger fs-4'>{props.message}</label>
     );
};

export default ErrorMessageLabel;