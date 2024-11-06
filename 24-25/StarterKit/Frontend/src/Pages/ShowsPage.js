import React, { useState, useEffect } from 'react';

const ShowPage = () => {
  const [movies, setMovies] = useState([]);
  const [selectedMovie, setSelectedMovie] = useState('');

  // Fetch movies from the ASP.NET Core API
  useEffect(() => {
    fetch('http://localhost:5025/api/Movies')
      .then((response) => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        return response.json();
      })
      .then((data) => {
        console.log('Fetched movies:', data); // Check if data is being logged correctly
        setMovies(data);
      })
      .catch((error) => {
        console.error('Error fetching movies:', error); // Log any errors
      });
  }, []);

  const handleMovieSelect = (event) => {
    const movieId = event.target.value;
    const movie = movies.find((m) => m.id === parseInt(movieId)); // Find the selected movie by ID
    setSelectedMovie(movie);
  };

  return (
    <div>
      <h1>Select a Movie</h1>
      <p><a href="home">Go to home page</a></p>
      
      {/* Dropdown for movie selection */}
      <select value={selectedMovie?.id || ''} onChange={handleMovieSelect}>
        <option value="">Select a movie</option>
        {movies.map((movie) => (
          <option key={movie.id} value={movie.id}>
            {movie.title}
          </option>
        ))}
      </select>

      {/* Displaying selected movie */}
      {selectedMovie && (
        <div style={{ marginTop: '20px' }}>
          <h2>Selected Movie: {selectedMovie.title}</h2>
          <p>Release year: {selectedMovie.year}</p>
          <p>Director: {selectedMovie.director}</p>
        </div>
      )}
    </div>
  );
};

export default ShowPage;
