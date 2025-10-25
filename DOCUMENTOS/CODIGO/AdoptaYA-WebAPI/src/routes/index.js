const router = require('express').Router();

router.use('/usuario', require('./usuario.routes'));
router.use('/rol', require('./rol.routes'));
router.use('/modulo', require('./modulo.routes'));
router.use('/permiso', require('./permiso.routes'));
router.use('/rol_usuario', require('./rol_usuario.routes'));
router.use('/auth', require('./auth.routes'));
router.use('/animal', require('./animal.routes'));
router.use('/mascota', require('./mascota.routes'));
router.use('/solicitante', require('./solicitante.routes'));
router.use('/referencia_personal', require('./referencia_personal.routes'));

module.exports = router;
