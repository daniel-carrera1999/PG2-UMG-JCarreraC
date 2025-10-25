const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const rol = sequelize.define('rol', {
    id:          { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true },
    nombre:      { type: DataTypes.STRING(45) },
    descripcion: { type: DataTypes.STRING(100) }
  }, {
    tableName: 'rol'
  });

  rol.associate = (models) => {
    // rol <-> usuario (N:M) vía rol_usuario
    rol.belongsToMany(models.usuario, {
      through: models.rol_usuario,
      foreignKey: 'id_rol',
      otherKey: 'id_usuario'
    });

    // rol -> permiso (1:N)
    rol.hasMany(models.permiso, { foreignKey: 'id_rol' });
  };

  return rol;
};
