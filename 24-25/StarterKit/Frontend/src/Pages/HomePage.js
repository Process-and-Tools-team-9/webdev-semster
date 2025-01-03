import React from 'react';
import { Link } from 'react-router-dom';

function HomePage({ user, onLogout }) {
  return (
    <div>
      <h1>Welcome, {user.Username}!</h1>
      <Link to="/shows" className="custom-link">View Shows</Link>
      <button onClick={onLogout}>Logout</button> {/* Logout button */}
    </div>
  );
}

export default HomePage;
