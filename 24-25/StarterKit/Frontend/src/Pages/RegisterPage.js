import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function RegisterPage() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();

    if (username && password) {
      try {
        const response = await fetch('http://localhost:5025/api/users/register', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username, password }),
        });

        if (response.ok) {
          alert('Account created successfully! Please log in.');
          navigate('/login'); // redirect to login page after successful registration
        } else {
          alert('Error creating account. Please try again.');
        }
      } catch (error) {
        console.error('Error:', error);
        alert('Error connecting to the server.');
      }
    } else {
      alert('Please enter both username and password');
    }
  };

  return (
    <div>
      <h2>Create an Account</h2>

      <form onSubmit={handleRegister}>
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

        <button type="submit">Sign Up</button>
      </form>
    </div>
  );
}

export default RegisterPage;
