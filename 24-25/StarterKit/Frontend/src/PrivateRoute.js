import React from 'react';
import { Navigate } from 'react-router-dom';

function PrivateRoute({ user, requiredRole, children }) {
  // Check if user is logged in
  if (!user) {
    return <Navigate to="/login" />;
  }

  // If a requiredRole is specified, check if user.role matches
  if (requiredRole && user.role !== requiredRole) {
    // If user is not an admin, for example, redirect somewhere
    return <Navigate to="/shows" />;
  }

  // If all checks pass, render the protected component
  return children;
}

export default PrivateRoute;
