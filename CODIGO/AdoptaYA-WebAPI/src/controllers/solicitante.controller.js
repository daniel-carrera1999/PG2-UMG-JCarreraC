const { Op } = require('sequelize');
const { solicitante } = require('../models');

exports.list = async (req, res, next) => {
  try {
    const { q, inactive } = req.query;

    const where = {};

    if (q) {
      where[Op.or] = [
        { nombres:        { [Op.like]: `%${q}%` } },
        { apellidos:      { [Op.like]: `%${q}%` } },
        { correo:         { [Op.like]: `%${q}%` } },
        { ocupacion:      { [Op.like]: `%${q}%` } },
        { estado_civil:   { [Op.like]: `%${q}%` } }
      ];
    }

    if (inactive !== undefined && inactive !== '2') {
      where.inactive = parseInt(inactive, 10);
    }

    const rows = await solicitante.findAll({
      where,
      order: [['id', 'ASC']]
    });

    res.json(rows);
  } catch (e) {
    next(e);
  }
};

exports.get = async (req, res, next) => {
  try {
    const item = await solicitante.findOne({
      where: { id_usuario: req.params.id }
    });

    res.json(item || {});
  } catch (e) {
    next(e);
  }
};

exports.create = async (req, res, next) => {
  try {
    const newSolicitante = await solicitante.create(req.body);
    res.status(201).json(newSolicitante);
  } catch (e) {
    next(e);
  }
};

exports.update = async (req, res, next) => {
  try {
    const item = await solicitante.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.update(req.body);
    res.json(item);
  } catch (e) {
    next(e);
  }
};

exports.remove = async (req, res, next) => {
  try {
    const item = await solicitante.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.destroy();
    res.status(204).send();
  } catch (e) {
    next(e);
  }
};
