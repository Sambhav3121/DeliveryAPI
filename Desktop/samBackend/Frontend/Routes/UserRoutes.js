const express = require('express');
const router = express.Router();
const UserController = require('../controllers/UserController');

router.post('/register', UserController.register);

router.post('/login', UserController.login);

router.get('/profile', UserController.getProfile);

router.put('/profile', UserController.editProfile);

module.exports = router;
