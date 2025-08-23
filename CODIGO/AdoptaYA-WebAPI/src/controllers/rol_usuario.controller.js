const { rol_usuario, rol, usuario } = require('../models');

exports.list = async (req, res, next) => {
  try {
    const items = await rol_usuario.findAll({
      include: [rol, usuario],
      order: [['id_rol', 'ASC'], ['id_usuario', 'ASC']]
    });
    res.json(items);
  } catch (e) { next(e); }
};

exports.get = async (req, res, next) => {
  try {
    const { id_rol, id_usuario } = req.params;
    const item = await rol_usuario.findOne({
      where: { id_rol, id_usuario },
      include: [rol, usuario]
    });
    if (!item) return res.status(404).json({ message: 'Not found' });
    res.json(item);
  } catch (e) { next(e); }
};

exports.create = async (req, res, next) => {
  try { res.status(201).json(await rol_usuario.create(req.body)); }
  catch (e) { next(e); }
};

exports.remove = async (req, res, next) => {
  try {
    const { id_rol, id_usuario } = req.params;
    const item = await rol_usuario.findOne({ where: { id_rol, id_usuario } });
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.destroy();
    res.status(204).send();
  } catch (e) { next(e); }
};
