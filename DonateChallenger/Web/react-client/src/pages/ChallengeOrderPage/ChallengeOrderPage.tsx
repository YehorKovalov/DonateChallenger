import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { Container } from "react-bootstrap";
import OrderForm from "../../containers/OrderForm";
import './styles.css';
const ChallengeOrderPage = observer(() => {

     return (
          <Container>
               <Container className="center">
                    <OrderForm/>
               </Container>
          </Container>
     );
});

export default ChallengeOrderPage;