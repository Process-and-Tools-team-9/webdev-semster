import React from 'react';

function HomePage({ user, onLogout }) {
  return (
    <div>
      <h1>Welcome, {user.firstname} {user.lastname}!</h1>
      <a href="shows">Go to shows page</a>
      <button onClick={onLogout}>Logout</button> {/* Logout button */}
    </div>
  );
}

export default HomePage;
