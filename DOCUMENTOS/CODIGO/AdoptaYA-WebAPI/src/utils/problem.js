// Helper para construir el payload
function buildProblem({ type, title, status, detail, instance, errors }) {
  return {
    Type: type || 'about:blank',
    Title: title || 'Error',
    Status: status || 500,
    Detail: detail || 'Internal server error',
    Datetime: new Date().toISOString(),
    Instance: instance || '',
    Errors: errors || {}
  };
}

// Error de dominio para lanzar desde controladores/servicios
class ApiError extends Error {
  constructor({ status = 500, title = 'Error', detail = 'Internal server error', type = 'about:blank', errors = {} } = {}) {
    super(detail);
    this.name = 'ApiError';
    this.status = status;
    this.title = title;
    this.type = type;
    this.errors = errors;
  }
}

module.exports = { buildProblem, ApiError };
