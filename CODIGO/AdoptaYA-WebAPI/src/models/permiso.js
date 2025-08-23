const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const permiso = sequelize.define('permiso', {
    id_rol:    { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true },
    id_modulo: { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true },
    create:    { type: DataTypes.TINYINT(1), allowNull: true },
    read:      { type: DataTypes.TINYINT(1), allowNull: true },
    update:    { type: DataTypes.TINYINT(1), allowNull: true },
    delete:    { type: DataTypes.TINYINT(1), allowNull: true }
  }, {
    tableName: 'permiso'
  });

  permiso.associate = (models) => {
    permiso.belongsTo(models.rol,    { foreignKey: 'id_rol' });
    permiso.belongsTo(models.modulo, { foreignKey: 'id_modulo' });
  };

  return permiso;
};
