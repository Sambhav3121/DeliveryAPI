import express from 'express';
import DishController from './DishController';

const router = express.Router();

router.get('/dishes', DishController.getAllDishes);
router.get('/dishes/:id', DishController.getDishById);
router.post('/dishes', DishController.createDish);
router.put('/dishes/:id', DishController.updateDish);
router.delete('/dishes/:id', DishController.deleteDish);

export default router;
