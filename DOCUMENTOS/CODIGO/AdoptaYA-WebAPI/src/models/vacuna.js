const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Vacuna = sequelize.define('vacuna', {
    id:                { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    descripcion:       { type: DataTypes.STRING(155), allowNull: true },
    aplicada:          { type: DataTypes.TINYINT(1), allowNull: true },
    fecha_aplicacion:  { type: DataTypes.DATEONLY, allowNull: true },
    date:              { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:          { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 },
    id_mascota:        { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'vacuna',
    timestamps: false
  });

  Vacuna.associate = (models) => {
    Vacuna.belongsTo(models.mascota, { foreignKey: 'id_mascota' });
  };

  return Vacuna;
};