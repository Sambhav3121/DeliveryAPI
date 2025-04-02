import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import UserProfile from './views/UserProfile';
import DishView from './components/Dish/DishView'; // Import DishView

const App = () => {
    return (
        <Router>
            <div className="App">
                <h1>Welcome to the User Profile App</h1>
                <Routes>
                    <Route path="/" element={<UserProfile />} />
                    <Route path="/dishes" element={<DishView />} /> {/* Add DishView route */}
                </Routes>
            </div>
        </Router>
    );
};

export default App;
