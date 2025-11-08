const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const Solicitante = sequelize.define('solicitante', {
    id:               { type: DataTypes.INTEGER.UNSIGNED, primaryKey: true, autoIncrement: true, allowNull: false },
    nombres:          { type: DataTypes.STRING(90), allowNull: true },
    apellidos:        { type: DataTypes.STRING(90), allowNull: true },
    fecha_nacimiento: { type: DataTypes.DATEONLY, allowNull: true },
    celular:          { type: DataTypes.STRING(75), allowNull: true },
    telefono_casa:    { type: DataTypes.STRING(75), allowNull: true },
    correo:           { type: DataTypes.STRING(120), allowNull: true },
    direccion:        { type: DataTypes.STRING(200), allowNull: true },
    ingresos:         { type: DataTypes.DOUBLE, allowNull: true },
    estado_civil:     { type: DataTypes.STRING(45), allowNull: true },
    ocupacion:        { type: DataTypes.STRING(100), allowNull: true },
    id_usuario:       { type: DataTypes.INTEGER.UNSIGNED, allowNull: true, unique: true },
    date:             { type: DataTypes.DATE, allowNull: true, defaultValue: DataTypes.NOW },
    inactive:         { type: DataTypes.TINYINT(1), allowNull: true, defaultValue: 0 }
  }, {
    tableName: 'solicitante',
    timestamps: false
  });

  Solicitante.associate = (models) => {
    Solicitante.belongsTo(models.usuario, { foreignKey: 'id_usuario', onUpdate: 'CASCADE', onDelete: 'SET NULL' });
  };

  return Solicitante;
};
