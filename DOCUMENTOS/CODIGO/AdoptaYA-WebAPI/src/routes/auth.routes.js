const router = require('express').Router();
const ctrl = require('../controllers/auth.controller');
const { verifyAccessToken } = require('../middlewares/auth');

router.post('/login', ctrl.login);
router.post('/refresh-token', ctrl.refresh);
router.post('/logout', ctrl.logout);
router.get('/me', verifyAccessToken, ctrl.me);

module.exports = router;
