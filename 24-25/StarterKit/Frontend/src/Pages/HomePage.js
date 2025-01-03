import React from 'react';

function HomePage({ user, onLogout }) {
  return (
    <div>
      <h1>Welcome, {user.Username}!</h1>
      <a href="shows" className="custom-link">Go to shows page</a>
      <button onClick={onLogout}>Logout</button> {/* Logout button */}
    </div>
  );
}

export default HomePage;
