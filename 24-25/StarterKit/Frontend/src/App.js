import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import './App.css';
import MyButton from './Button';
import LoginPage from './Pages/LoginPage';
import HomePage from './Pages/HomePage';  // Import your Home page component
import ShowsPage from './Pages/ShowsPage'; // Import your Shows page component
import backgroundImage from './Assets/Background.jpeg';

function App() {
  const [user, setUser] = useState(null); // State to track the logged-in user

  useEffect(() => {
    const savedUser = sessionStorage.getItem('user');
    if (savedUser) {
      setUser(JSON.parse(savedUser)); // Parse and set the user from localStorage
    }
  }, []);

  // Handle login and store the user details (firstname, lastname)
  const handleLogin = (userDetails) => {
    setUser(userDetails);
    sessionStorage.setItem('user', JSON.stringify(userDetails)); // Save user to localStorage
  };

  // Handle logout (clear localStorage and state)
  const handleLogout = () => {
    setUser(null);
    sessionStorage.removeItem('user'); // Clear user from localStorage
  };


  return (
    <Router>
      <div className="App" style={{ backgroundImage: `url(${backgroundImage})` }}>
        <header className="App-header">
          {/* If user is not logged in, show login page */}
          {user === null ? (
            <LoginPage onLogin={handleLogin} />
          ) : (

            // Define routes for the application
            <Routes>
              <Route path="/home" element={<HomePage user={user} onLogout={handleLogout}/>} />
              <Route path="/login" element={<LoginPage user={user} />} />
              <Route path="/shows" element={<ShowsPage user={user} />} />
            </Routes>
          )}
        </header>
      </div>
    </Router>
  );
}

export default App;