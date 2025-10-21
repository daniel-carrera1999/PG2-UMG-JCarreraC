const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Enfermedad = sequelize.define('enfermedad', {
    id:           { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    descripcion:  { type: DataTypes.STRING(300), allowNull: true },
    tratamiento:  { type: DataTypes.STRING(600), allowNull: true },
    date:         { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:     { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 },
    id_mascota:   { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'enfermedad',
    timestamps: false
  });

  Enfermedad.associate = (models) => {
    Enfermedad.belongsTo(models.mascota, { foreignKey: 'id_mascota' });
    Enfermedad.hasMany(models.medicina, { foreignKey: 'id_enfermedad' });
  };

  return Enfermedad;
};