const { modulo, permiso, rol } = require('../models');

exports.list = async (req, res, next) => {
  try {
    const items = await modulo.findAll({
      order: [['id', 'ASC']],
      include: [{ model: permiso, include: [rol] }]
    });
    res.json(items);
  } catch (e) { next(e); }
};

exports.get = async (req, res, next) => {
  try {
    const item = await modulo.findByPk(req.params.id, {
      include: [{ model: permiso, include: [rol] }]
    });
    if (!item) return res.status(404).json({ message: 'Not found' });
    res.json(item);
  } catch (e) { next(e); }
};

exports.create = async (req, res, next) => {
  try { res.status(201).json(await modulo.create(req.body)); }
  catch (e) { next(e); }
};

exports.update = async (req, res, next) => {
  try {
    const item = await modulo.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.update(req.body);
    res.json(item);
  } catch (e) { next(e); }
};

exports.remove = async (req, res, next) => {
  try {
    const item = await modulo.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });

    const permisosCount = await item.countPermisos();
    
    if (permisosCount > 0) {
      return res.status(406).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.6',
        title: 'Conflicto de integridad referencial',
        status: 406,
        detail: `No se puede eliminar el módulo porque tiene ${permisosCount} permiso(s) asociado(s)`,
        datetime: new Date().toISOString(),
        instance: req.originalUrl,
        errors: {
          'id_modulo': [`El módulo con ID ${req.params.id} tiene ${permisosCount} permiso(s) relacionado(s)`]
        }
      });
    }

    await item.destroy();
    res.status(204).send();
  } catch (e) { next(e); }
};
