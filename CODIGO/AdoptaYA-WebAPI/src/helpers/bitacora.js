const { bitacora } = require('../models');

/**
 * Registra una acción en la bitácora
 * @param {Object} params - Parámetros de la bitácora
 * @param {string} params.tabla - Nombre de la tabla afectada
 * @param {string} params.accion - Tipo de acción (INSERT, UPDATE, DELETE)
 * @param {Object|string|number} params.datos - Datos a registrar (objeto, string o ID)
 * @param {number} params.id_usuario - ID del usuario que realiza la acción
 * @param {Object} params.transaction - Transacción de Sequelize (opcional)
 */
exports.registrarBitacora = async ({ tabla, accion, datos, id_usuario, transaction }) => {
  try {
    let datosString;
    
    if (typeof datos === 'object') {
      datosString = JSON.stringify(datos);
    } else {
      datosString = String(datos);
    }

    await bitacora.create({
      tabla,
      accion,
      fecha: new Date(),
      datos: datosString,
      id_usuario
    }, { transaction });

  } catch (error) {
    console.error('Error al registrar en bitácora:', error);
  }
};