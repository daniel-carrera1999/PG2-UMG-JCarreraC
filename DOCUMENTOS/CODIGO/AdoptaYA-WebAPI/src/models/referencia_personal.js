const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const ReferenciaPersonal = sequelize.define('referencia_personal', {
    id:             { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    nombre:         { type: DataTypes.STRING(200), allowNull: true },
    telefono:       { type: DataTypes.STRING(75), allowNull: true },
    vinculo:        { type: DataTypes.STRING(45), allowNull: true },
    date:           { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:       { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 },
    id_solicitante: { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'referencia_personal',
    timestamps: false
  });

  ReferenciaPersonal.associate = (models) => {
    ReferenciaPersonal.belongsTo(models.solicitante, { foreignKey: 'id_solicitante', onUpdate: 'CASCADE' });
  };

  return ReferenciaPersonal;
};