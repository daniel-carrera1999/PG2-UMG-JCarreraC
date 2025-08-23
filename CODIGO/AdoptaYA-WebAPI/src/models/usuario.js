// src/models/usuario.js
const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const usuario = sequelize.define('usuario', {
    id:        { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true },
    username:  { type: DataTypes.STRING(25) },
    correo:    { type: DataTypes.STRING(100), allowNull: false, unique: 'ux_usuario_correo' },
    password:  { type: DataTypes.STRING(100), allowNull: false },
    nombre:    { type: DataTypes.STRING(100) },
    apellido:  { type: DataTypes.STRING(100) },
    date:      { type: DataTypes.DATE, allowNull: true },
    inactive:  { type: DataTypes.TINYINT(1) }
  }, {
    tableName: 'usuario',
    freezeTableName: true,
    timestamps: false
  });

  usuario.associate = (models) => {
    usuario.belongsToMany(models.rol, {
      through: models.rol_usuario,
      foreignKey: 'id_usuario',
      otherKey: 'id_rol'
    });
  };

  return usuario;
};
