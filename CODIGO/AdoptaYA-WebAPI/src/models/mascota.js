const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Mascota = sequelize.define('mascota', {
    id:                { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    nombre_mascota:    { type: DataTypes.STRING(150), allowNull: true },
    tamanio:           { type: DataTypes.STRING(45), allowNull: true },
    peso:              { type: DataTypes.DOUBLE, allowNull: true },
    color:             { type: DataTypes.STRING(45), allowNull: true },
    comportamiento:    { type: DataTypes.STRING(1000), allowNull: true },
    foto_principal:    { type: DataTypes.TEXT('long'), allowNull: true },
    foto_secundaria:   { type: DataTypes.TEXT('long'), allowNull: true },
    foto_adicional:    { type: DataTypes.TEXT('long'), allowNull: true },
    date:              { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:          { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 },
    id_animal:         { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'mascota',
    timestamps: false
  });

  Mascota.associate = (models) => {
    Mascota.belongsTo(models.animal, { foreignKey: 'id_animal' });
    Mascota.hasMany(models.enfermedad, { foreignKey: 'id_mascota' });
    Mascota.hasMany(models.vacuna, { foreignKey: 'id_mascota' });
  };

  return Mascota;
};