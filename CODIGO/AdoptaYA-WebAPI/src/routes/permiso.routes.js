const router = require('express').Router();
const ctrl = require('../controllers/permiso.controller');

// llave compuesta en URL
router.get('/', ctrl.list);
router.get('/id_rol/:id_rol/id_modulo/:id_modulo', ctrl.get);
router.post('/', ctrl.create);
router.put('/id_rol/:id_rol/id_modulo/:id_modulo', ctrl.update);
router.delete('/id_rol/:id_rol/id_modulo/:id_modulo', ctrl.remove);

module.exports = router;
