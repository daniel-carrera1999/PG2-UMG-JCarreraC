const { bitacora, usuario } = require('../models');

const listarBitacora = async (req, res) => {
  try {
    const registros = await bitacora.findAll({
      include: [{
        model: usuario,
        attributes: ['id', 'username'] // solo los campos que necesitas del usuario
      }],
      order: [['fecha', 'DESC']] // opcional, para mostrar los más recientes primero
    });

    res.status(200).json(registros);
  } catch (error) {
    console.error('Error al obtener bitácora:', error);
    res.status(500).json({ message: 'Error al obtener bitácora', error });
  }
};

module.exports = { listarBitacora };
