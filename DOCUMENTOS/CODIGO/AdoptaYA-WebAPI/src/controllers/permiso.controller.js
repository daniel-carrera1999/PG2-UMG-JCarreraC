const { permiso, rol, modulo } = require('../models');

exports.list = async (req, res, next) => {
  try {
    const items = await permiso.findAll({
      include: [rol, modulo],
      order: [['id_rol', 'ASC'], ['id_modulo', 'ASC']]
    });
    res.json(items);
  } catch (e) { next(e); }
};

exports.get = async (req, res, next) => {
  try {
    const { id_rol, id_modulo } = req.params;
    const item = await permiso.findOne({
      where: { id_rol, id_modulo },
      include: [rol, modulo]
    });
    if (!item) return res.status(404).json({ message: 'Not found' });
    res.json(item);
  } catch (e) { next(e); }
};

exports.create = async (req, res, next) => {
  try { res.status(201).json(await permiso.create(req.body)); }
  catch (e) { next(e); }
};

exports.update = async (req, res, next) => {
  try {
    const { id_rol, id_modulo } = req.params;
    const item = await permiso.findOne({ where: { id_rol, id_modulo } });
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.update(req.body);
    res.json(item);
  } catch (e) { next(e); }
};

exports.remove = async (req, res, next) => {
  try {
    const { id_rol, id_modulo } = req.params;
    const item = await permiso.findOne({ where: { id_rol, id_modulo } });
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.destroy();
    res.status(204).send();
  } catch (e) { next(e); }
};
