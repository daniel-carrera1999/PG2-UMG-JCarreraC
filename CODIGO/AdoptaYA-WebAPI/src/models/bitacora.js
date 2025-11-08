const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Bitacora = sequelize.define('bitacora', {
    id:         { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    tabla:      { type: DataTypes.STRING(50), allowNull: true },
    accion:     { type: DataTypes.STRING(10), allowNull: true },
    fecha:      { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    datos:      { type: DataTypes.TEXT, allowNull: true },
    id_usuario: { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'bitacora',
    timestamps: false
  });

  Bitacora.associate = (models) => {
    Bitacora.belongsTo(models.usuario, { foreignKey: 'id_usuario' });
  };

  return Bitacora;
};