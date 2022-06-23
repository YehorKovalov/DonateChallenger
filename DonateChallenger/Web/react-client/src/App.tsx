import './App.css';
import AppRoutes from './routes/AppRoutes';
import { observer } from 'mobx-react';
const App = observer(() => {
  return (
    <div className="main-font-family">
          <AppRoutes />
    </div>
  );
});

export default App;
