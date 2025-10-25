const jwt = require('jsonwebtoken');

const ACCESS_EXP = process.env.ACCESS_TOKEN_EXPIRES || '15m';
const REFRESH_EXP = process.env.REFRESH_TOKEN_EXPIRES || '20m';

exports.signAccessToken = (payload) =>
  jwt.sign(payload, process.env.JWT_ACCESS_SECRET, { expiresIn: ACCESS_EXP });

exports.signRefreshToken = (payload) =>
  jwt.sign(payload, process.env.JWT_REFRESH_SECRET, { expiresIn: REFRESH_EXP });

exports.verifyAccess = (token) =>
  jwt.verify(token, process.env.JWT_ACCESS_SECRET);

exports.verifyRefresh = (token) =>
  jwt.verify(token, process.env.JWT_REFRESH_SECRET);
