const DishService = {
    fetchAllDishes: async () => {
      const response = await fetch('/api/dishes');
      return await response.json();
    },
  
    fetchDishById: async (id) => {
      const response = await fetch(`/api/dishes/${id}`);
      return await response.json();
    },
  
    createDish: async (dish) => {
      const response = await fetch('/api/dishes', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(dish),
      });
      return await response.json();
    },
  
    updateDish: async (id, updatedDish) => {
      const response = await fetch(`/api/dishes/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedDish),
      });
      return await response.json();
    },
  
    deleteDish: async (id) => {
      await fetch(`/api/dishes/${id}`, {
        method: 'DELETE',
      });
    },
  };
  
  export default DishService;
  