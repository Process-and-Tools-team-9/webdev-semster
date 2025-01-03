import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';

function LoginPage({ onLogin }) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState(''); // 1) State to store errors
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    setErrorMessage(''); // Clear any previous errors

    if (username && password) {
      try {
        const response = await fetch('http://localhost:5025/api/users/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username, password }),
        });

        if (response.ok) {
          const data = await response.json();
          console.log(data.message); // For debugging
          onLogin(data.user); // Use the user object from the backend response
          navigate('/home');
        } else {
          // 2) Handle non-OK responses (e.g., 401 Unauthorized)
          // Try to get error text from server; otherwise use default
          let errorText = 'Incorrect username or password.';
          try {
            // Attempt to parse the error body (if the server sends a message)
            errorText = await response.text();
          } catch (err) {
            console.error('Error parsing error response:', err);
          }
          setErrorMessage(errorText);
        }
      } catch (error) {
        console.error('Error:', error);
        setErrorMessage('Error connecting to the server.');
      }
    } else {
      setErrorMessage('Please enter both username and password');
    }
  };

  return (
    <div>
      <h2>Login</h2>

      {/* 3) Show error message if it exists */}
      {errorMessage && <p style={{ color: 'red' }}>{errorMessage}</p>}

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="username">Username:</label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>

        <div>
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        <button type="submit">Login</button>
      </form>

      <p>
        <Link to="/register" className="custom-link">Register here</Link>
      </p>
    </div>
  );
}

export default LoginPage;
