const router = require('express').Router();
const ctrl = require('../controllers/mascota.controller');

router.post('/', ctrl.create);
router.get('/', ctrl.list);
router.get('/:id', ctrl.get);
router.get('/:id/photos/:only_principal', ctrl.get_photos);

module.exports = router;
