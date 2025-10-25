const { where } = require('sequelize');
const db = require('../models');
const sequelize = db.sequelize;
const { mascota, animal, vacuna, enfermedad, medicina } = require('../models');

exports.create = async (req, res, next) => {
  const t = await sequelize.transaction();
  
  try {
    const {
      nombre_mascota,
      tamanio,
      peso,
      color,
      comportamiento,
      foto_principal,
      foto_secundaria,
      foto_adicional,
      id_animal,
      vacunas,
      enfermedades
    } = req.body;

    // Validar que el animal exista
    const animalExists = await animal.findByPk(id_animal);
    if (!animalExists) {
      return res.status(404).json({ message: 'El animal especificado no existe' });
    }

    // Crear la mascota
    const nuevaMascota = await mascota.create({
      nombre_mascota,
      tamanio,
      peso,
      color,
      comportamiento,
      foto_principal,
      foto_secundaria,
      foto_adicional,
      id_animal
    }, { transaction: t });

    // Crear vacunas si se enviaron Y tienen contenido
    if (vacunas && Array.isArray(vacunas) && vacunas.length > 0) {
      const vacunasValidas = vacunas.filter(v => 
        v && (v.descripcion || v.aplicada !== undefined || v.fecha_aplicacion)
      );
      
      if (vacunasValidas.length > 0) {
        const vacunasData = vacunasValidas.map(v => ({
          ...v,
          id_mascota: nuevaMascota.id
        }));
        await vacuna.bulkCreate(vacunasData, { transaction: t });
      }
    }

    // Crear enfermedades con sus medicinas si se enviaron Y tienen contenido
    if (enfermedades && Array.isArray(enfermedades) && enfermedades.length > 0) {
      const enfermedadesValidas = enfermedades.filter(enf => 
        enf && (enf.descripcion || enf.tratamiento)
      );
      
      for (const enf of enfermedadesValidas) {
        const nuevaEnfermedad = await enfermedad.create({
          descripcion: enf.descripcion,
          tratamiento: enf.tratamiento,
          id_mascota: nuevaMascota.id
        }, { transaction: t });

        // Crear medicinas para esta enfermedad si se enviaron Y tienen contenido
        if (enf.medicinas && Array.isArray(enf.medicinas) && enf.medicinas.length > 0) {
          const medicinasValidas = enf.medicinas.filter(m => 
            m && (m.nombre || m.descripcion || m.indicaciones)
          );
          
          if (medicinasValidas.length > 0) {
            const medicinasData = medicinasValidas.map(m => ({
              ...m,
              id_enfermedad: nuevaEnfermedad.id
            }));
            await medicina.bulkCreate(medicinasData, { transaction: t });
          }
        }
      }
    }

    await t.commit();

    // Obtener la mascota completa con sus relaciones
    const mascotaCompleta = await mascota.findByPk(nuevaMascota.id, {
      include: [
        { model: animal },
        { model: vacuna },
        { 
          model: enfermedad,
          include: [{ model: medicina }]
        }
      ]
    });

    res.status(201).json(mascotaCompleta);
  } catch (e) {
    await t.rollback();
    next(e);
  }
};

exports.update = async (req, res, next) => {
  const t = await sequelize.transaction();
  
  try {
    const { id } = req.params;
    const {
      nombre_mascota,
      tamanio,
      peso,
      color,
      comportamiento,
      foto_principal,
      foto_secundaria,
      foto_adicional,
      id_animal,
      vacunas,
      enfermedades
    } = req.body;

    // Verificar que la mascota exista
    const mascotaExistente = await mascota.findByPk(id);
    if (!mascotaExistente) {
      await t.rollback();
      return res.status(404).json({ message: 'La mascota no existe' });
    }

    // Validar que el animal exista
    const animalExists = await animal.findByPk(id_animal);
    if (!animalExists) {
      await t.rollback();
      return res.status(404).json({ message: 'El animal especificado no existe' });
    }

    // Actualizar la mascota
    await mascotaExistente.update({
      nombre_mascota,
      tamanio,
      peso,
      color,
      comportamiento,
      foto_principal,
      foto_secundaria,
      foto_adicional,
      id_animal
    }, { transaction: t });

    // Eliminar vacunas existentes
    await vacuna.destroy({
      where: { id_mascota: id },
      transaction: t
    });

    // Crear nuevas vacunas si se enviaron Y tienen contenido
    if (vacunas && Array.isArray(vacunas) && vacunas.length > 0) {
      const vacunasValidas = vacunas.filter(v => 
        v && (v.descripcion || v.aplicada !== undefined || v.fecha_aplicacion)
      );
      
      if (vacunasValidas.length > 0) {
        const vacunasData = vacunasValidas.map(v => ({
          ...v,
          id_mascota: id
        }));
        await vacuna.bulkCreate(vacunasData, { transaction: t });
      }
    }

    // Eliminar enfermedades existentes (esto también eliminará las medicinas por cascada si está configurado)
    const enfermedadesExistentes = await enfermedad.findAll({
      where: { id_mascota: id },
      transaction: t
    });

    for (const enf of enfermedadesExistentes) {
      await medicina.destroy({
        where: { id_enfermedad: enf.id },
        transaction: t
      });
    }

    await enfermedad.destroy({
      where: { id_mascota: id },
      transaction: t
    });

    // Crear nuevas enfermedades con sus medicinas si se enviaron Y tienen contenido
    if (enfermedades && Array.isArray(enfermedades) && enfermedades.length > 0) {
      const enfermedadesValidas = enfermedades.filter(enf => 
        enf && (enf.descripcion || enf.tratamiento)
      );
      
      for (const enf of enfermedadesValidas) {
        const nuevaEnfermedad = await enfermedad.create({
          descripcion: enf.descripcion,
          tratamiento: enf.tratamiento,
          id_mascota: id
        }, { transaction: t });

        // Crear medicinas para esta enfermedad si se enviaron Y tienen contenido
        if (enf.medicinas && Array.isArray(enf.medicinas) && enf.medicinas.length > 0) {
          const medicinasValidas = enf.medicinas.filter(m => 
            m && (m.nombre || m.descripcion || m.indicaciones)
          );
          
          if (medicinasValidas.length > 0) {
            const medicinasData = medicinasValidas.map(m => ({
              ...m,
              id_enfermedad: nuevaEnfermedad.id
            }));
            await medicina.bulkCreate(medicinasData, { transaction: t });
          }
        }
      }
    }

    await t.commit();

    // Obtener la mascota completa con sus relaciones
    const mascotaActualizada = await mascota.findByPk(id, {
      include: [
        { model: animal },
        { model: vacuna },
        { 
          model: enfermedad,
          include: [{ model: medicina }]
        }
      ]
    });

    res.status(200).json(mascotaActualizada);
  } catch (e) {
    await t.rollback();
    next(e);
  }
};

exports.list = async (req, res, next) => {
  
  const { q, inactive } = req.query;
  const where = {}

  if (q) {
    where[Op.or] = [
      { nombre_mascota: { [Op.like]: `%${q}%` } }
    ];
  }

  if (inactive !== undefined && inactive !== '2') {
    where.inactive = parseInt(inactive, 10);
  }

  try {
    const items = await mascota.findAll({
      order: [['date', 'DESC']],
      attributes: ['id', 'nombre_mascota', 'tamanio', 'foto_principal', 'peso', 'color', 'comportamiento', 'date', 'id_animal'],
      where,
      include: [
        { 
          model: animal,
          attributes: ['id', 'especie', 'raza']
        }
      ]
    });
    res.json(items);
  } catch (e) { 
    next(e); 
  }
};

exports.get = async (req, res, next) => {
  try {
    const item = await mascota.findByPk(req.params.id, {
      attributes: ['id', 'nombre_mascota', 'tamanio', 'peso', 'color', 'comportamiento', 'date', 'inactive', 'id_animal'],
      include: [
        { model: animal },
        { model: vacuna },
        { 
          model: enfermedad,
          include: [{ model: medicina }]
        }
      ]
    });
    if (!item) return res.status(404).json({ message: 'Mascota no encontrada' });
    res.json(item);
  } catch (e) { 
    next(e); 
  }
};

exports.get_photos = async (req, res, next) => {

  let OnlyPrincipal = req.params.only_principal === 'true' ? true : false;
  let Photos = OnlyPrincipal ? ['foto_principal'] : ['foto_principal', 'foto_secundaria', 'foto_adicional'];

  try {
    const item = await mascota.findByPk(req.params.id, {

      attributes: Photos,
      include: []
    });
    if (!item) return res.status(404).json({ message: 'Mascota no encontrada' });
    res.json(item);
  } catch (e) { 
    next(e); 
  }
};