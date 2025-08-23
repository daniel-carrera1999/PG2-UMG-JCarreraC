const { ValidationError } = require('sequelize');
const { buildProblem, ApiError } = require('../utils/problem');

module.exports = (err, req, res, next) => {
  // Default
  let status = 500;
  let title = 'Internal Server Error';
  let type = 'about:blank';
  let detail = err?.message || 'Internal server error';
  let errorsDict = {};

  // ApiError propio
  if (err instanceof ApiError) {
    status = err.status ?? status;
    title = err.title ?? title;
    type = err.type ?? type;
    detail = err.message ?? detail;
    errorsDict = err.errors ?? {};
  }

  // Sequelize: Validation / Unique constraint
  else if (err instanceof ValidationError || err?.name === 'SequelizeUniqueConstraintError' || err?.name === 'SequelizeValidationError') {
    status = 400;
    title = 'Validation Error';
    type = 'https://example.com/problems/validation-error';
    const dict = {};
    for (const e of err.errors || []) {
      const key = e.path || 'general';
      if (!dict[key]) dict[key] = [];
      dict[key].push(e.message);
    }
    errorsDict = dict;
    detail = 'One or more validation errors occurred.';
  }

  // JWT inválido/expirado
  else if (err?.name === 'JsonWebTokenError' || err?.name === 'TokenExpiredError') {
    status = 401;
    title = 'Unauthorized';
    type = 'https://example.com/problems/unauthorized';
    detail = err.message || 'Invalid or expired token';
  }

  // Construir respuesta con formato
  const problem = buildProblem({
    type,
    title,
    status,
    detail,
    instance: req.originalUrl,
    errors: errorsDict
  });

  res.status(status).json(problem);
};
