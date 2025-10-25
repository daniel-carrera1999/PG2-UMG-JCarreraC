const fs = require('fs');
const path = require('path');
const { Sequelize } = require('sequelize');
const dbConfig = require('../config/database');

const basename = path.basename(__filename);
const sequelize = new Sequelize(dbConfig);

const db = { sequelize, Sequelize };

// 1) Cargar todos los modelos (*.js) exceptuando este archivo
fs.readdirSync(__dirname)
  .filter(file =>
    file.endsWith('.js') &&
    file !== basename
  )
  .forEach(file => {
    const defineModel = require(path.join(__dirname, file));
    const model = defineModel(sequelize); // cada modelo exporta (sequelize) => model
    db[model.name] = model;               // usa el name exacto del modelo/tabla (e.g., 'usuario')
  });

// 2) Ejecutar asociaciones locales de cada modelo, si existen
Object.values(db).forEach(model => {
  if (typeof model.associate === 'function') {
    model.associate(db);
  }
});

module.exports = db;
