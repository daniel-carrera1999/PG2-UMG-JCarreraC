const express = require('express');
const router = express.Router();
const ctrl = require('../controllers/adopcion.controller');

router.get('/mascotas-disponibles', ctrl.listDisponibles);
router.post('/adoptar', ctrl.create);
router.get('/mis_solicitudes/:idUsuario', ctrl.listSolicitudesUsuario);
router.get('/todas_solicitudes', ctrl.listTodasSolicitudes);
router.get('/detalle_solicitud/:idAdopcion', ctrl.getDetalleAdopcion);
router.get('/detalle_solicitud_management/:idAdopcion', ctrl.getDetalleAdopcionManagement);

module.exports = router;