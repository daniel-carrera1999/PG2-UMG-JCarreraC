const { referencia_personal, solicitante } = require('../models');

exports.list = async (req, res, next) => {
  try {
    const items = await referencia_personal.findAll({
      include: [solicitante],
      order: [['id', 'ASC']]
    });
    res.json(items);
  } catch (e) { next(e); }
};

exports.get = async (req, res, next) => {
  try {
    const { id } = req.params;
    const items = await referencia_personal.findAll({
      where: { id_solicitante: id }
    });
    res.json(items);
  } catch (e) { next(e); }
};

exports.create = async (req, res, next) => {
  try {
    const referencias = req.body;
    
    const nuevasReferencias = await referencia_personal.bulkCreate(referencias);
    
    res.status(201).json(nuevasReferencias);
  } catch (e) { 
    next(e); 
  }
};

exports.update = async (req, res, next) => {
  try {
    const { id } = req.params;
    const referencias = req.body;
    
    await referencia_personal.destroy({ 
      where: { id_solicitante: id } 
    });
    
    const nuevasReferencias = await referencia_personal.bulkCreate(referencias);
    
    res.json(nuevasReferencias);
  } catch (e) { 
    next(e); 
  }
};

exports.remove = async (req, res, next) => {
  try {
    const { id } = req.params;
    const item = await referencia_personal.findOne({ where: { id } });
    if (!item) return res.status(404).json({ message: 'Not found' });
    await item.destroy();
    res.status(204).send();
  } catch (e) { next(e); }
};