import React, { useState, useEffect } from 'react';
import DishService from './DishService';

const DishView = () => {
  const [dishes, setDishes] = useState([]);

  useEffect(() => {
    async function fetchDishes() {
      const data = await DishService.fetchAllDishes();
      setDishes(data);
    }
    fetchDishes();
  }, []);

  return (
    <div>
      <h2>Dishes</h2>
      <ul>
        {dishes.map((dish) => (
          <li key={dish.id}>
            <h3>{dish.name}</h3>
            <p>{dish.description}</p>
            <p>Price: ${dish.price}</p>
            <p>Category: {dish.category}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default DishView;
