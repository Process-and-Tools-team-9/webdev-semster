import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function LoginPage({ onLogin }) {
  const [firstname, setFirstname] = useState('');
  const [lastname, setLastname] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (firstname && lastname) {
      try {
        const response = await fetch('http://localhost:5025/api/users/login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({ firstname, lastname }),
        });

        if (response.ok) {
          const data = await response.json();
          console.log(data.message); // For debugging
          onLogin(data.user); // Use the user object from the backend response
          navigate('/home');
        } else {
          alert('Error logging in. Please try again.');
        }
      } catch (error) {
        console.error('Error:', error);
        alert('Error connecting to the server.');
      }
    } else {
      alert('Please enter both first and last names');
    }
  };
  
    return (
      <div>
        <h2>Login</h2>
        <form onSubmit={handleSubmit}>
          <div>
            <label htmlFor="firtname">Firtname:</label>
            <input
              type="text"
              id="firstname"
              value={firstname}
              onChange={(e) => setFirstname(e.target.value)}
              required
            />
          </div>
          <div>
            <label htmlFor="lastname">Lastname:</label>
            <input
              type="text"
              id="lastname"
              value={lastname}
              onChange={(e) => setLastname(e.target.value)}
              required
            />
          </div>
          <button type="submit">Submit</button>
        </form>
      </div>
    );
  }
export default LoginPage;