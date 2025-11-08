const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Retorno = sequelize.define('retorno', {
    id:               { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    fecha_de_retorno: { type: DataTypes.DATEONLY, allowNull: true },
    observaciones:    { type: DataTypes.STRING(255), allowNull: true },
    date:             { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:         { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 },
    id_adopcion:      { type: DataTypes.INTEGER.UNSIGNED, allowNull: false }
  }, {
    tableName: 'retorno',
    timestamps: false
  });

  Retorno.associate = (models) => {
    Retorno.belongsTo(models.adopcion, { foreignKey: 'id_adopcion' });
  };

  return Retorno;
};