const router = require('express').Router();
const ctrl = require('../controllers/bitacora.controller');

router.get('/', ctrl.listarBitacora);

module.exports = router;
