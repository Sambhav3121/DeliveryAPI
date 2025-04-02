import Dish from './DishModel';

const dishes = [];

const DishController = {
  getAllDishes: (req, res) => {
    res.json(dishes);
  },

  getDishById: (req, res) => {
    const dish = dishes.find((d) => d.id === req.params.id);
    if (dish) {
      res.json(dish);
    } else {
      res.status(404).send('Dish not found');
    }
  },

  createDish: (req, res) => {
    const { name, description, price, category } = req.body;
    const id = dishes.length + 1;
    const newDish = new Dish(id, name, description, price, category);
    dishes.push(newDish);
    res.status(201).json(newDish);
  },

  updateDish: (req, res) => {
    const dish = dishes.find((d) => d.id === req.params.id);
    if (dish) {
      const { name, description, price, category } = req.body;
      dish.name = name || dish.name;
      dish.description = description || dish.description;
      dish.price = price || dish.price;
      dish.category = category || dish.category;
      res.json(dish);
    } else {
      res.status(404).send('Dish not found');
    }
  },

  deleteDish: (req, res) => {
    const index = dishes.findIndex((d) => d.id === req.params.id);
    if (index !== -1) {
      dishes.splice(index, 1);
      res.status(204).send();
    } else {
      res.status(404).send('Dish not found');
    }
  },
};

export default DishController;
