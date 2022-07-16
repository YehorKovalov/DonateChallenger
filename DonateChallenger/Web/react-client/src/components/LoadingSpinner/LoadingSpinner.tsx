import { Spinner } from 'react-bootstrap';

const LoadingSpinner = () => {

     return (
          <div className="position-absolute top-50 start-50 translate-middle">
               <Spinner animation='border'/>
          </div>
     );
};

export default LoadingSpinner;