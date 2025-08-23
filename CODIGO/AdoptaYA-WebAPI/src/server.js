// src/server.js
require('dotenv').config();
const app = require('./app');
const { sequelize } = require('./models');

const port = parseInt(process.env.PORT, 10) || 3000;
let server;

(async () => {
  try {
    // Verifica conexión a MySQL (no altera tus tablas)
    await sequelize.authenticate();
    console.log('✅ Conexión a MySQL OK');

    server = app.listen(port, () => {
      console.log(`🚀 API corriendo en http://localhost:${port}`);
    });
  } catch (err) {
    console.error('❌ Error conectando a la BD:', err);
    process.exit(1);
  }
})();

// Cierre elegante
const shutdown = async (signal) => {
  try {
    console.log(`\n${signal} recibido. Cerrando...`);
    if (server) {
      await new Promise((resolve) => server.close(resolve));
      console.log('🧹 HTTP server cerrado');
    }
    await sequelize.close();
    console.log('🔌 Conexión a MySQL cerrada');
    process.exit(0);
  } catch (e) {
    console.error('⚠️ Error al cerrar:', e);
    process.exit(1);
  }
};

process.on('SIGINT', () => shutdown('SIGINT'));
process.on('SIGTERM', () => shutdown('SIGTERM'));
