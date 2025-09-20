const { usuario, rol, rol_usuario } = require('../models');
const { Op } = require('sequelize');
const bcrypt = require('bcrypt');

exports.list = async (req, res, next) => {
  try {
    const { page = 1, size = 10, q } = req.query;
    const limit = Math.min(parseInt(size, 10) || 10, 100);
    const offset = (Math.max(parseInt(page, 10) || 1, 1) - 1) * limit;

    const where = q ? {
      [Op.or]: [
        { username: { [Op.like]: `%${q}%` } },
        { correo:   { [Op.like]: `%${q}%` } },
        { nombre:   { [Op.like]: `%${q}%` } },
        { apellido: { [Op.like]: `%${q}%` } }
      ]
    } : {};

    const { rows, count } = await usuario.findAndCountAll({
      attributes: { exclude: ['password'] },
      where, limit, offset, order: [['id', 'ASC']],
      include: [{ model: rol, through: { attributes: [] } }]
    });

    res.json({ data: rows, page: Number(page), size: limit, total: count });
  } catch (e) { next(e); }
};

exports.get = async (req, res, next) => {
  try {
    const item = await usuario.findByPk(req.params.id, {
      attributes: { exclude: ['password'] }, // ⬅️
      include: [{ model: rol, through: { attributes: [] } }]
    });
    if (!item) return res.status(404).json({ message: 'Not found' });
    res.json(item);
  } catch (e) { next(e); }
};

exports.create = async (req, res, next) => {
  try {
    const { username, correo, password, nombre, apellido } = req.body;
    const hashed = await bcrypt.hash(password, 10);
    const item = await usuario.create({ username, correo, password: hashed, nombre, apellido });
    const item_rol = await rol_usuario.create({ id_rol: 2, id_usuario: item.id });
    const { password: _omit, ...safe } = item.get({ plain: true });
    res.status(201).json(safe);
  } catch (e) { next(e); }
};

exports.update = async (req, res, next) => {
  try {
    const item = await usuario.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });

    const { username, correo, password, nombre, apellido } = req.body;
    const fields = { username, correo, nombre, apellido };
    if (password) fields.password = await bcrypt.hash(password, 10);

    await item.update(fields);
    const { password: _omit, ...safe } = item.get({ plain: true });
    res.json(safe);
  } catch (e) { next(e); }
};

exports.remove = async (req, res, next) => {
  try {
    const item = await usuario.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.destroy();
    res.status(204).send();
  } catch (e) { next(e); }
};
