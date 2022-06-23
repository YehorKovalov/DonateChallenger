import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { configure } from "mobx"
import { IoCProvider } from './utilities/ioc/ioc.react';
import { container } from './utilities/ioc/iocContainer';
import 'bootstrap/dist/css/bootstrap.min.css';

configure({
     enforceActions: "never",
})

ReactDOM.render(
          <React.StrictMode>
               <IoCProvider container={container}>
                    <App/>
               </IoCProvider>
          </React.StrictMode>
, document.getElementById('root'));