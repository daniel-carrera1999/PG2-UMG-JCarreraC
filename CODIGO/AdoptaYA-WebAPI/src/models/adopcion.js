const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Adopcion = sequelize.define('adopcion', {
    id:              { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    fecha_adopcion:  { type: DataTypes.DATEONLY, allowNull: true },
    no_doc:          { type: DataTypes.INTEGER, allowNull: true },
    adjunto:         { type: DataTypes.STRING(1000), allowNull: true },
    date:            { type: DataTypes.DATE, allowNull: true },
    status:          { type: DataTypes.STRING(50), allowNull: true },
    id_solicitante:  { type: DataTypes.INTEGER.UNSIGNED, allowNull: false },
    id_mascota:      { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'adopcion',
    timestamps: false
  });

  Adopcion.associate = (models) => {
    Adopcion.belongsTo(models.solicitante, { foreignKey: 'id_solicitante' });
    Adopcion.belongsTo(models.mascota, { foreignKey: 'id_mascota' });
    Adopcion.hasMany(models.seguimiento, { foreignKey: 'id_adopcion' });
    Adopcion.hasMany(models.retorno, { foreignKey: 'id_adopcion' });
  };

  return Adopcion;
};