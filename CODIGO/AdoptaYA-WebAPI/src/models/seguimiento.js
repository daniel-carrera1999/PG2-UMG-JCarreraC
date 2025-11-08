const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Seguimiento = sequelize.define('seguimiento', {
    id:                 { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    fecha_seguimiento:  { type: DataTypes.DATEONLY, allowNull: true },
    observaciones:      { type: DataTypes.STRING(500), allowNull: true },
    date:               { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:           { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 },
    id_adopcion:        { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'seguimiento',
    timestamps: false
  });

  Seguimiento.associate = (models) => {
    Seguimiento.belongsTo(models.adopcion, { foreignKey: 'id_adopcion' });
  };

  return Seguimiento;
};