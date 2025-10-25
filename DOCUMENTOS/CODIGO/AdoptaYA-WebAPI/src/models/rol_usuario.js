const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const rol_usuario = sequelize.define('rol_usuario', {
    id_rol:     { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true },
    id_usuario: { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true }
  }, {
    tableName: 'rol_usuario'
  });

  return rol_usuario;
};
