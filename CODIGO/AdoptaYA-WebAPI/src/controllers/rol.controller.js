const { rol, permiso, modulo, usuario } = require('../models');

exports.list = async (req, res, next) => {
  try {
    const items = await rol.findAll({
      order: [['id', 'ASC']],
      include: [
        { model: permiso, include: [modulo] },
        { model: usuario, through: { attributes: [] } }
      ]
    });
    res.json(items);
  } catch (e) { next(e); }
};

exports.get = async (req, res, next) => {
  try {
    const item = await rol.findByPk(req.params.id, {
      include: [
        { model: permiso, include: [modulo] },
        { model: usuario, through: { attributes: [] } }
      ]
    });
    if (!item) return res.status(404).json({ message: 'Not found' });
    res.json(item);
  } catch (e) { next(e); }
};

exports.create = async (req, res, next) => {
  try { res.status(201).json(await rol.create(req.body)); }
  catch (e) { next(e); }
};

exports.update = async (req, res, next) => {
  try {
    const item = await rol.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.update(req.body);
    res.json(item);
  } catch (e) { next(e); }
};

exports.remove = async (req, res, next) => {
  try {
    const item = await rol.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.destroy();
    res.status(204).send();
  } catch (e) { next(e); }
};
