const router = require('express').Router();

router.use('/usuario', require('./usuario.routes'));
router.use('/rol', require('./rol.routes'));
router.use('/modulo', require('./modulo.routes'));
router.use('/permiso', require('./permiso.routes'));
router.use('/rol_usuario', require('./rol_usuario.routes'));
router.use('/auth', require('./auth.routes'));

module.exports = router;
