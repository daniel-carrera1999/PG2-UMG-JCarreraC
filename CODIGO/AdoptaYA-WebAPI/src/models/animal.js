const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const animal = sequelize.define('animal', {
    id:         { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true },
    especie:    { type: DataTypes.STRING(45), allowNull: true },
    raza:       { type: DataTypes.STRING(45), allowNull: true },
    date:       { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:   { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 }
  }, {
    tableName: 'animal'
  });

  animal.associate = (models) => {
  };

  return animal;
};