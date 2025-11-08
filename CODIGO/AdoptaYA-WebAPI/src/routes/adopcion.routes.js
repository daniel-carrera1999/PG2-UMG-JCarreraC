const express = require('express');
const router = express.Router();
const ctrl = require('../controllers/adopcion.controller');

router.get('/mascotas-disponibles', ctrl.listDisponibles);
router.post('/adoptar', ctrl.create);
router.get('/mis_solicitudes/:idUsuario', ctrl.listSolicitudesUsuario);
router.get('/detalle_solicitud/:idAdopcion', ctrl.getDetalleAdopcion);

module.exports = router;