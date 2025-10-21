const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Medicina = sequelize.define('medicina', {
    id:             { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    nombre:         { type: DataTypes.STRING(45), allowNull: true },
    descripcion:    { type: DataTypes.STRING(250), allowNull: true },
    indicaciones:   { type: DataTypes.STRING(250), allowNull: true },
    date:           { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:       { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 },
    id_enfermedad:  { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'medicina',
    timestamps: false
  });

  Medicina.associate = (models) => {
    Medicina.belongsTo(models.enfermedad, { foreignKey: 'id_enfermedad' });
  };

  return Medicina;
};