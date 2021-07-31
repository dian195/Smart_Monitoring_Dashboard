import React from 'react';
import { Route, Switch } from 'react-router-dom';

// FREE
import HomePage from './pages/HomePage';
import HistoryPage from './pages/History';



class Routes extends React.Component {
  render() {
    return (
      <Switch>
        <Route exact path='/' component={HomePage} />
        <Route exact path='/History' component={HistoryPage} />
        
        <Route
          render={function() {
            return <h1>Not Found</h1>;
          }}
        />
      </Switch>
    );
  }
}

export default Routes;
