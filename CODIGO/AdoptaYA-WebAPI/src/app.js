const express = require('express');
const cookieParser = require('cookie-parser');
const routes = require('./routes');
const notFound = require('./middlewares/notFound');
const error = require('./middlewares/error');

const app = express();
app.use(express.json());
app.use(cookieParser());

app.use('/api', routes);

app.use(notFound);
app.use(error);

module.exports = app;
