const bcrypt = require('bcrypt');
const { usuario } = require('../models');
const { signAccessToken, signRefreshToken, verifyRefresh } = require('../utils/jwt');
const { setRefreshCookie, clearRefreshCookie  } = require('../utils/cookies');
const { loadUserFull } = require('../services/user.service');
const { ApiError } = require('../utils/problem');
const { toLoginResponseDTO } = require('../utils/auth.responseDTO');

// POST /api/auth/login
exports.login = async (req, res, next) => {
  try {
    const correo = String(req.body.correo || '').trim().toLowerCase();
    const password = String(req.body.password || '');

    if (!correo || !password) {
      return next(new ApiError({
        status: 400,
        title: 'Bad Request',
        type: 'https://example.com/problems/bad-request',
        detail: 'correo y password son obligatorios',
        errors: { correo: !correo ? ['correo Requerido'] : [], password: !password ? ['password Requerido'] : [] }
      }));
    }

    const u = await usuario.findOne({ where: { correo } });
    if (!u) {
      // anti timing-attack dummy compare
      await bcrypt.compare(password, '$2b$10$8x3X0Vg3q2nPZ0xHqK5bOO6zH8k5o1tFQ0NwFQ0NwFQ0NwFQ0NwF.');
      return next(new ApiError({
        status: 401,
        title: 'Unauthorized',
        type: 'https://example.com/problems/unauthorized',
        detail: 'Credenciales inválidas'
      }));
    }

    if (u.inactive === 1) {
      return next(new ApiError({
        status: 403,
        title: 'Forbidden',
        type: 'https://example.com/problems/forbidden',
        detail: 'Usuario inactivo'
      }));
    }

    const ok = await bcrypt.compare(password, u.password);
    if (!ok) {
      return next(new ApiError({
        status: 401,
        title: 'Unauthorized',
        type: 'https://example.com/problems/unauthorized',
        detail: 'Credenciales inválidas'
      }));
    }

    // payload con id, username y correo
    const payload = { sub: u.id, username: u.username, correo: u.correo };
    const accessToken = signAccessToken(payload);
    const refreshToken = signRefreshToken(payload);

    // set cookie refresh_token
    setRefreshCookie(res, refreshToken);
    
    // set access token en header
    res.set('Authorization', `Bearer ${accessToken}`);

    // cargar usuario con rol+modulos+permisos
    const full = await loadUserFull(u.id);

    // Validar rol asignado
    const roles = full.rols || [];
    if (roles.length === 0) {
      return next(new ApiError({
        status: 403,
        title: 'Forbidden',
        type: 'https://example.com/problems/no-role',
        detail: 'El usuario no tiene rol asignado'
      }));
    }

    // devolver DTO estructurado
    return res.json(toLoginResponseDTO(accessToken, full));
  } catch (e) {
    next(e);
  }
};

// POST /api/auth/refresh-token
exports.refresh = async (req, res, next) => {
  try {
    const token = req.cookies?.refresh_token;
    if (!token) {
      return next(new ApiError({
        status: 401,
        title: 'Unauthorized',
        type: 'https://example.com/problems/unauthorized',
        detail: 'Missing refresh token'
      }));
    }

    let payload;
    try {
      payload = verifyRefresh(token); // { sub, username, correo, iat, exp }
    } catch {
      return next(new ApiError({
        status: 401,
        title: 'Unauthorized',
        type: 'https://example.com/problems/unauthorized',
        detail: 'Invalid or expired refresh token'
      }));
    }

    // generar nuevos tokens
    const newAccess = signAccessToken({
      sub: payload.sub,
      username: payload.username,
      correo: payload.correo
    });
    const newRefresh = signRefreshToken({
      sub: payload.sub,
      username: payload.username,
      correo: payload.correo
    });

    // set nueva cookie refresh_token
    setRefreshCookie(res, newRefresh);

    // set access token en header
    res.set('Authorization', `Bearer ${newAccess}`);

    // devolver solo el nuevo access_token
    return res.json({
      access_token: newAccess,
      token_type: 'Bearer',
      expires_in: 15 * 60 * 1000
    });
  } catch (e) {
    next(e);
  }
};

// POST /api/auth/logout
exports.logout = async (req, res, next) => {
  try {
    clearRefreshCookie(res);
    return res.status(204).send();
  } catch (e) { next(e); }
};

// GET /api/auth/me  (requiere middleware verifyAccessToken)
exports.me = async (req, res, next) => {
  try {
    const full = await loadUserFull(req.auth.sub);
    if (!full) {
      return next(new ApiError({
        status: 404,
        title: 'Not Found',
        type: 'https://example.com/problems/not-found',
        detail: 'Usuario no encontrado'
      }));
    }
    res.json(full);
  } catch (e) { next(e); }
};
