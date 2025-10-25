const { verifyAccess } = require('../utils/jwt');

exports.verifyAccessToken = (req, res, next) => {
  try {
    const header = req.headers.authorization || '';
    const [, token] = header.split(' ');
    if (!token) return res.status(401).json({ message: 'Missing token' });

    const payload = verifyAccess(token);
    req.auth = payload;
    next();
  } catch {
    return res.status(401).json({ message: 'Invalid or expired token' });
  }
};
