const { rol, permiso, modulo, usuario } = require('../models');

exports.list = async (req, res, next) => {
  try {
    const { includeRelations } = req.query;
    
    const options = {
      order: [['id', 'ASC']]
    };
    
    if (includeRelations === 'true') {
      options.include = [
        { model: permiso, include: [modulo] },
        { model: usuario, through: { attributes: [] } }
      ];
    }
    
    const items = await rol.findAll(options);
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
    
    const usuariosCount = await item.countUsuarios();
    if (usuariosCount > 0) {
      return res.status(406).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.6',
        title: 'Conflicto de integridad referencial',
        status: 406,
        detail: `No se puede eliminar el rol porque tiene ${usuariosCount} usuario(s) asociado(s)`,
        datetime: new Date().toISOString(),
        instance: req.originalUrl,
        errors: {
          'id_rol': [`El rol con ID ${req.params.id} tiene ${usuariosCount} usuario(s) relacionado(s)`]
        }
      });
    }
    
    await item.destroy();
    res.status(204).send();
  } catch (e) { next(e); }
};
