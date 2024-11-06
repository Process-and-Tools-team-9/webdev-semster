// Button.js
import React, { useState } from 'react';

function MyButton() {
  // Declare a state variable for the count, initialize it to 0
  const [count, setCount] = useState(0);


  // Function to handle button clicks and increment the count
  const handleClick = () => {
    setCount(count + 1);
  };

  return (
    <div>
      <button onClick={handleClick}>
        Don't Click Me!
      </button>
      <p>You've clicked the button {count} times.</p>
    </div>
  );
}
export default MyButton;
