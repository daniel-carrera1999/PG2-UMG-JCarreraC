const { buildProblem } = require('../utils/problem');

module.exports = (req, res, next) => {
  const problem = buildProblem({
    type: 'https://example.com/problems/not-found',
    title: 'Not Found',
    status: 404,
    detail: `Resource ${req.originalUrl} not found`,
    instance: req.originalUrl
  });
  res.status(404).json(problem);
};
