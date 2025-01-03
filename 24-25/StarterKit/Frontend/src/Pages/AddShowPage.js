import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function AddShowPage() {
  const [title, setTitle] = useState('');
  const [year, setYear] = useState('');
  const [director, setDirector] = useState('');
  const [message, setMessage] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');

    const newMovie = {
      title,
      year: parseInt(year, 10),
      director
    };

    try {
      // Update the URL to match your .NET API port
      const response = await fetch('http://localhost:5025/api/Movies', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(newMovie)
      });

      if (!response.ok) {
        throw new Error('Failed to add movie');
      }

      setMessage('Movie added successfully!');

      // Optionally reset the form
      setTitle('');
      setYear('');
      setDirector('');
    } catch (error) {
      console.error(error);
      setMessage('Error adding movie.');
    }
  };

  return (
    <div style={{ margin: '20px' }}>
      <h2>Add a New Movie</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Title:</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>

        <div>
          <label>Year:</label>
          <input
            type="number"
            value={year}
            onChange={(e) => setYear(e.target.value)}
          />
        </div>

        <div>
          <label>Director:</label>
          <input
            type="text"
            value={director}
            onChange={(e) => setDirector(e.target.value)}
          />
        </div>

        <button type="submit">Add Movie</button>
      </form>

      {message && <p>{message}</p>}
    </div>
  );
}

export default AddShowPage;
