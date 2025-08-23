const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const modulo = sequelize.define('modulo', {
    id:          { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true },
    nombre:      { type: DataTypes.STRING(100) },
    path:        { type: DataTypes.STRING(255) },
    descripcion: { type: DataTypes.STRING(255) }
  }, {
    tableName: 'modulo'
  });

  modulo.associate = (models) => {
    // modulo -> permiso (1:N)
    modulo.hasMany(models.permiso, { foreignKey: 'id_modulo' });
  };

  return modulo;
};
