const { Op } = require('sequelize');
const { animal } = require('../models');

exports.list = async (req, res, next) => {
  try {
    const { q, inactive } = req.query;

    const where = {};

    if (q) {
      where[Op.or] = [
        { especie: { [Op.like]: `%${q}%` } },
        { raza: { [Op.like]: `%${q}%` } }
      ];
    }

    if (inactive !== undefined && inactive !== '2') {
      where.inactive = parseInt(inactive, 10);
    }

    const rows = await animal.findAll({
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
    const item = await animal.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    res.json(item);
  } catch (e) {
    next(e);
  }
};

exports.create = async (req, res, next) => {
  try {
    const newAnimal = await animal.create(req.body);
    res.status(201).json(newAnimal);
  } catch (e) {
    next(e);
  }
};

exports.update = async (req, res, next) => {
  try {
    const item = await animal.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.update(req.body);
    res.json(item);
  } catch (e) {
    next(e);
  }
};

exports.remove = async (req, res, next) => {
  try {
    const item = await animal.findByPk(req.params.id);
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.destroy();
    res.status(204).send();
  } catch (e) {
    next(e);
  }
};