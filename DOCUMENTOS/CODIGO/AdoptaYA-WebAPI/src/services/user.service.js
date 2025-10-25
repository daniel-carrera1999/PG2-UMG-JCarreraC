// src/services/user.service.js
const { usuario, rol, permiso, modulo } = require('../models');

exports.loadUserFull = async (userId) => {
  const u = await usuario.findByPk(userId, {
    attributes: ['id','username','correo','nombre','apellido','inactive'],
    include: [
      {
        model: rol,
        through: { attributes: [] },
        include: [
          {
            model: permiso,
            separate: true,
            order: [['id_modulo', 'ASC']],
            include: [modulo]
          }
        ]
      }
    ]
  });
  return u;
};
