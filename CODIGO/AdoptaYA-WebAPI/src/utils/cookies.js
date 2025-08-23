const baseOpts = {
  httpOnly: true,
  secure: process.env.NODE_ENV === 'production',
  sameSite: 'strict',
  domain: process.env.COOKIE_DOMAIN || undefined,
  path: '/api/auth',
};

exports.setRefreshCookie = (res, token) => {
  const twentyMin = 20 * 60 * 1000;
  res.cookie('refresh_token', token, { ...baseOpts, maxAge: twentyMin });
};

exports.clearRefreshCookie = (res) => {
  res.clearCookie('refresh_token', baseOpts);
};
