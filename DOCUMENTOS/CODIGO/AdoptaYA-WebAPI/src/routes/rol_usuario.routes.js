const router = require('express').Router();
const ctrl = require('../controllers/rol_usuario.controller');

router.get('/', ctrl.list);
router.get('/id_rol/:id_rol/id_usuario/:id_usuario', ctrl.get);
router.post('/', ctrl.create);
router.delete('/id_rol/:id_rol/id_usuario/:id_usuario', ctrl.remove);

module.exports = router;
