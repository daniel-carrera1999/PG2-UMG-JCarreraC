const { registrarBitacora } = require('../helpers/bitacora');
const { adopcion, mascota, enfermedad, medicina, vacuna, animal, solicitante, referencia_personal, usuario, seguimiento, retorno, sequelize } = require('../models');
const { Op } = require('sequelize');

exports.listDisponibles = async (req, res, next) => {
  
  const { q, inactive } = req.query;
  const where = {};

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
        },
        {
          model: adopcion,
          attributes: ['id', 'status'],
          required: false // LEFT JOIN para incluir mascotas sin adopción
        }
      ]
    });

    // Filtrar mascotas sin adopción o con adopción cancelada
    const mascotasDisponibles = items.filter(mascota => {
      const adopciones = mascota.adopcions || [];
      
      // Si no tiene adopciones, está disponible
      if (adopciones.length === 0) {
        return true;
      }
      
      // Si todas sus adopciones están canceladas, está disponible
      return adopciones.every(adopcion => adopcion.status === 'Cancelada');
    }).map(mascota => {
      // Remover el array de adopcions del objeto
      const mascotaJSON = mascota.toJSON();
      delete mascotaJSON.adopcions;
      return mascotaJSON;
    });

    res.json(mascotasDisponibles);
  } catch (e) { 
    next(e); 
  }
};

exports.create = async (req, res, next) => {
  const { id_usuario, id_mascota } = req.body;

  // Validar campos requeridos
  const errors = {};
  
  if (!id_usuario) errors.id_usuario = ['El campo id_usuario es requerido'];
  if (!id_mascota) errors.id_mascota = ['El campo id_mascota es requerido'];

  if (Object.keys(errors).length > 0) {
    return res.status(406).json({
      type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
      title: 'Uno o más errores de validación ocurrieron.',
      status: 406,
      detail: 'Por favor revise los errores',
      datetime: new Date().toISOString(),
      instance: req.originalUrl,
      errors
    });
  }

  const transaction = await sequelize.transaction();

  try {
    // Verificar que la mascota existe
    const mascotaExiste = await mascota.findByPk(id_mascota, { transaction });
    
    if (!mascotaExiste) {
      await transaction.rollback();
      return res.status(406).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
        title: 'Uno o más errores de validación ocurrieron.',
        status: 406,
        detail: 'La mascota no existe',
        datetime: new Date().toISOString(),
        instance: req.originalUrl,
        errors: {
          id_mascota: ['La mascota especificada no existe']
        }
      });
    }

    // Verificar si la mascota tiene adopciones activas
    const adopcionesExistentes = await adopcion.findAll({
      where: { id_mascota },
      transaction
    });

    // Validar que no haya adopciones activas (solo permitir si no hay o están canceladas)
    const tieneAdopcionActiva = adopcionesExistentes.some(
      adop => adop.status !== 'Cancelada'
    );

    if (tieneAdopcionActiva) {
      await transaction.rollback();
      return res.status(406).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
        title: 'Uno o más errores de validación ocurrieron.',
        status: 406,
        detail: 'La mascota no está disponible para adopción',
        datetime: new Date().toISOString(),
        instance: req.originalUrl,
        errors: {
          id_mascota: ['La mascota ya tiene una adopción activa']
        }
      });
    }

    // Buscar el solicitante asociado al usuario
    const solicitanteData = await solicitante.findOne({
      where: { id_usuario },
      transaction
    });

    if (!solicitanteData) {
      await transaction.rollback();
      return res.status(406).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
        title: 'Uno o más errores de validación ocurrieron.',
        status: 406,
        detail: 'El usuario no tiene un perfil de solicitante',
        datetime: new Date().toISOString(),
        instance: req.originalUrl,
        errors: {
          id_usuario: ['Debe completar su perfil de solicitante antes de realizar una adopción']
        }
      });
    }

    // Validar que el solicitante tenga todos los campos requeridos completos
    const camposFaltantes = [];
    
    if (!solicitanteData.nombres) camposFaltantes.push('nombres');
    if (!solicitanteData.apellidos) camposFaltantes.push('apellidos');
    if (!solicitanteData.fecha_nacimiento) camposFaltantes.push('fecha de nacimiento');
    if (!solicitanteData.celular) camposFaltantes.push('celular');
    if (!solicitanteData.correo) camposFaltantes.push('correo electrónico');
    if (!solicitanteData.direccion) camposFaltantes.push('dirección');
    if (!solicitanteData.ingresos) camposFaltantes.push('ingresos mensuales');

    if (camposFaltantes.length > 0) {
      await transaction.rollback();
      return res.status(406).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
        title: 'Uno o más errores de validación ocurrieron.',
        status: 406,
        detail: 'El perfil del solicitante está incompleto',
        datetime: new Date().toISOString(),
        instance: req.originalUrl,
        errors: {
          solicitante: [`Debe completar los siguientes campos: ${camposFaltantes.join(', ')}`]
        }
      });
    }

    // Verificar que tenga al menos una referencia personal
    const referencias = await referencia_personal.findAll({
      where: { id_solicitante: solicitanteData.id },
      transaction
    });

    if (referencias.length === 0) {
      await transaction.rollback();
      return res.status(406).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
        title: 'Uno o más errores de validación ocurrieron.',
        status: 406,
        detail: 'El solicitante debe tener al menos una referencia personal',
        datetime: new Date().toISOString(),
        instance: req.originalUrl,
        errors: {
          referencias: ['Debe agregar al menos una referencia personal antes de solicitar una adopción']
        }
      });
    }

    // Crear la adopción
    const nuevaAdopcion = await adopcion.create({
      fecha_adopcion: new Date(),
      date: new Date(),
      status: 'Pendiente',
      id_solicitante: solicitanteData.id,
      id_mascota
    }, { transaction });

    // Registrar en bitácora
    await registrarBitacora({
      tabla: 'adopcion',
      accion: 'INSERT',
      datos: {
        id: nuevaAdopcion.id,
        status: nuevaAdopcion.status,
        id_solicitante: nuevaAdopcion.id_solicitante,
        id_mascota: nuevaAdopcion.id_mascota,
        fecha_adopcion: nuevaAdopcion.fecha_adopcion
      },
      id_usuario,
      transaction
    });

    await transaction.commit();

    res.status(201).json(nuevaAdopcion);

  } catch (e) {
    await transaction.rollback();
    
    // Error genérico del servidor
    res.status(500).json({
      type: 'https://tools.ietf.org/html/rfc7231#section-6.6.1',
      title: 'Ocurrió un error al procesar su solicitud.',
      status: 500,
      detail: e.message || 'Error interno del servidor',
      datetime: new Date().toISOString(),
      instance: req.originalUrl,
      errors: {}
    });
  }
};

exports.listSolicitudesUsuario = async (req, res) => {
  try {
    const { idUsuario } = req.params;

    // Validar que el idUsuario sea un número
    if (!idUsuario || isNaN(idUsuario)) {
      return res.status(400).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
        title: 'Error de validación',
        status: 400,
        detail: 'El ID de usuario debe ser un número válido',
        datetime: new Date().toISOString(),
        instance: req.originalUrl
      });
    }

    // Verificar que el usuario existe
    const usuarioExiste = await usuario.findByPk(idUsuario);
    if (!usuarioExiste) {
      return res.status(404).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.4',
        title: 'Recurso no encontrado',
        status: 404,
        detail: 'Usuario no encontrado',
        datetime: new Date().toISOString(),
        instance: req.originalUrl
      });
    }

    // Buscar solicitudes de adopción
    const solicitudes = await adopcion.findAll({
      attributes: ['id', 'date', 'status'],
      include: [
        {
          model: solicitante,
          attributes: [],
          where: { id_usuario: idUsuario },
          required: true
        },
        {
          model: mascota,
          attributes: ['nombre_mascota', 'id_animal'],
          required: true,
          include: [
            {
              model: animal,
              attributes: ['raza', 'especie'],
              required: true
            }
          ]
        }
      ],
      order: [['date', 'DESC']],
      raw: true
    });

    // Formatear la respuesta
    const solicitudesFormateadas = solicitudes.map(solicitud => ({
      id_adopcion: solicitud.id,
      date: solicitud.date,
      status: solicitud.status,
      nombre_mascota: solicitud['mascotum.nombre_mascota'],
      raza: solicitud['mascotum.animal.raza'],
      especie: solicitud['mascotum.animal.especie']
    }));

    res.status(200).json(solicitudesFormateadas);

  } catch (error) {
    console.error('Error al obtener solicitudes:', error);
    res.status(500).json({
      type: 'https://tools.ietf.org/html/rfc7231#section-6.6.1',
      title: 'Error interno del servidor',
      status: 500,
      detail: 'Error al obtener las solicitudes de adopción',
      datetime: new Date().toISOString(),
      instance: req.originalUrl,
      error: error.message
    });
  }
};

exports.listTodasSolicitudes = async (req, res) => {
  try {
    // Buscar todas las solicitudes de adopción
    const solicitudes = await adopcion.findAll({
      attributes: ['id', 'date', 'status'],
      include: [
        {
          model: mascota,
          attributes: ['nombre_mascota', 'id_animal'],
          required: true,
          include: [
            {
              model: animal,
              attributes: ['raza', 'especie'],
              required: true
            }
          ]
        }
      ],
      order: [['date', 'DESC']],
      raw: true
    });

    // Formatear la respuesta
    const solicitudesFormateadas = solicitudes.map(solicitud => ({
      id_adopcion: solicitud.id,
      date: solicitud.date,
      status: solicitud.status,
      nombre_mascota: solicitud['mascotum.nombre_mascota'],
      raza: solicitud['mascotum.animal.raza'],
      especie: solicitud['mascotum.animal.especie']
    }));

    res.status(200).json(solicitudesFormateadas);

  } catch (error) {
    console.error('Error al obtener solicitudes:', error);
    res.status(500).json({
      type: 'https://tools.ietf.org/html/rfc7231#section-6.6.1',
      title: 'Error interno del servidor',
      status: 500,
      detail: 'Error al obtener las solicitudes de adopción',
      datetime: new Date().toISOString(),
      instance: req.originalUrl,
      error: error.message
    });
  }
};

exports.getDetalleAdopcion = async (req, res) => {
  try {
    const { idAdopcion } = req.params;

    // Validar que el idAdopcion sea un número
    if (!idAdopcion || isNaN(idAdopcion)) {
      return res.status(400).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
        title: 'Error de validación',
        status: 400,
        detail: 'El ID de adopción debe ser un número válido',
        datetime: new Date().toISOString(),
        instance: req.originalUrl
      });
    }

    // Buscar la adopción con toda la información relacionada
    const adopcionDetalle = await adopcion.findOne({
      where: { id: idAdopcion },
      include: [
        {
          model: solicitante,
          attributes: {
            exclude: ['inactive', 'id_usuario']
          },
          include: [
            {
              model: usuario,
              attributes: ['id', 'username', 'correo', 'nombre', 'apellido']
            }
          ]
        },
        {
          model: mascota,
          attributes: {
            exclude: ['inactive']
          },
          include: [
            {
              model: animal,
              attributes: {
                exclude: ['inactive', 'date']
              }
            },
            {
              model: enfermedad,
              attributes: {
                exclude: ['inactive']
              },
              required: false,
              include: [
                {
                  model: medicina,
                  attributes: {
                    exclude: ['inactive']
                  },
                  required: false
                }
              ]
            },
            {
              model: vacuna,
              attributes: {
                exclude: ['inactive']
              },
              required: false
            }
          ]
        }
      ]
    });

    // Verificar si existe la adopción
    if (!adopcionDetalle) {
      return res.status(404).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.4',
        title: 'Recurso no encontrado',
        status: 404,
        detail: 'Solicitud de adopción no encontrada',
        datetime: new Date().toISOString(),
        instance: req.originalUrl
      });
    }

    // Convertir a JSON para formatear
    const adopcionJSON = adopcionDetalle.toJSON();

    // Estructurar la respuesta de manera clara
    const respuesta = {
      adopcion: {
        id: adopcionJSON.id,
        fecha_adopcion: adopcionJSON.fecha_adopcion,
        no_doc: adopcionJSON.no_doc,
        adjunto: adopcionJSON.adjunto,
        date: adopcionJSON.date,
        status: adopcionJSON.status
      },
      solicitante: {
        id: adopcionJSON.solicitante?.id,
        nombres: adopcionJSON.solicitante?.nombres,
        apellidos: adopcionJSON.solicitante?.apellidos,
        fecha_nacimiento: adopcionJSON.solicitante?.fecha_nacimiento,
        celular: adopcionJSON.solicitante?.celular,
        telefono_casa: adopcionJSON.solicitante?.telefono_casa,
        correo: adopcionJSON.solicitante?.correo,
        direccion: adopcionJSON.solicitante?.direccion,
        ingresos: adopcionJSON.solicitante?.ingresos,
        estado_civil: adopcionJSON.solicitante?.estado_civil,
        ocupacion: adopcionJSON.solicitante?.ocupacion,
        date: adopcionJSON.solicitante?.date,
        usuario: adopcionJSON.solicitante?.usuario || null
      },
      mascota: {
        id: adopcionJSON.mascotum?.id,
        nombre_mascota: adopcionJSON.mascotum?.nombre_mascota,
        tamanio: adopcionJSON.mascotum?.tamanio,
        peso: adopcionJSON.mascotum?.peso,
        color: adopcionJSON.mascotum?.color,
        comportamiento: adopcionJSON.mascotum?.comportamiento,
        foto_principal: adopcionJSON.mascotum?.foto_principal,
        foto_secundaria: adopcionJSON.mascotum?.foto_secundaria,
        foto_adicional: adopcionJSON.mascotum?.foto_adicional,
        date: adopcionJSON.mascotum?.date,
        animal: {
          id: adopcionJSON.mascotum?.animal?.id,
          especie: adopcionJSON.mascotum?.animal?.especie,
          raza: adopcionJSON.mascotum?.animal?.raza
        },
        enfermedades: adopcionJSON.mascotum?.enfermedads?.map(enfermedad => ({
          id: enfermedad.id,
          descripcion: enfermedad.descripcion,
          tratamiento: enfermedad.tratamiento,
          date: enfermedad.date,
          medicinas: enfermedad.medicinas?.map(medicina => ({
            id: medicina.id,
            nombre: medicina.nombre,
            descripcion: medicina.descripcion,
            indicaciones: medicina.indicaciones,
            date: medicina.date
          })) || []
        })) || [],
        vacunas: adopcionJSON.mascotum?.vacunas?.map(vacuna => ({
          id: vacuna.id,
          descripcion: vacuna.descripcion,
          aplicada: vacuna.aplicada,
          fecha_aplicacion: vacuna.fecha_aplicacion,
          date: vacuna.date
        })) || []
      }
    };

    res.status(200).json(respuesta);

  } catch (error) {
    console.error('Error al obtener detalle de adopción:', error);
    res.status(500).json({
      type: 'https://tools.ietf.org/html/rfc7231#section-6.6.1',
      title: 'Error interno del servidor',
      status: 500,
      detail: 'Error al obtener el detalle de la solicitud de adopción',
      datetime: new Date().toISOString(),
      instance: req.originalUrl,
      error: error.message
    });
  }
};

exports.getDetalleAdopcionManagement = async (req, res) => {
  try {
    const { idAdopcion } = req.params;

    // Validar que el idAdopcion sea un número
    if (!idAdopcion || isNaN(idAdopcion)) {
      return res.status(400).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.1',
        title: 'Error de validación',
        status: 400,
        detail: 'El ID de adopción debe ser un número válido',
        datetime: new Date().toISOString(),
        instance: req.originalUrl
      });
    }

    // Buscar la adopción con toda la información relacionada
    const adopcionDetalle = await adopcion.findOne({
      where: { id: idAdopcion },
      include: [
        {
          model: solicitante,
          attributes: {
            exclude: ['inactive', 'id_usuario']
          },
          include: [
            {
              model: usuario,
              attributes: ['id', 'username', 'correo', 'nombre', 'apellido']
            }
          ]
        },
        {
          model: mascota,
          attributes: {
            exclude: ['inactive']
          },
          include: [
            {
              model: animal,
              attributes: {
                exclude: ['inactive', 'date']
              }
            },
            {
              model: enfermedad,
              attributes: {
                exclude: ['inactive']
              },
              required: false,
              include: [
                {
                  model: medicina,
                  attributes: {
                    exclude: ['inactive']
                  },
                  required: false
                }
              ]
            },
            {
              model: vacuna,
              attributes: {
                exclude: ['inactive']
              },
              required: false
            }
          ]
        },
        {
          model: seguimiento,
          attributes: {
            exclude: ['inactive']
          },
          required: false
        },
        {
          model: retorno,
          attributes: {
            exclude: ['inactive']
          },
          required: false
        }
      ]
    });

    // Verificar si existe la adopción
    if (!adopcionDetalle) {
      return res.status(404).json({
        type: 'https://tools.ietf.org/html/rfc7231#section-6.5.4',
        title: 'Recurso no encontrado',
        status: 404,
        detail: 'Solicitud de adopción no encontrada',
        datetime: new Date().toISOString(),
        instance: req.originalUrl
      });
    }

    // Convertir a JSON para formatear
    const adopcionJSON = adopcionDetalle.toJSON();

    // Estructurar la respuesta de manera clara
    const respuesta = {
      adopcion: {
        id: adopcionJSON.id,
        fecha_adopcion: adopcionJSON.fecha_adopcion,
        no_doc: adopcionJSON.no_doc,
        adjunto: adopcionJSON.adjunto,
        date: adopcionJSON.date,
        status: adopcionJSON.status
      },
      solicitante: {
        id: adopcionJSON.solicitante?.id,
        nombres: adopcionJSON.solicitante?.nombres,
        apellidos: adopcionJSON.solicitante?.apellidos,
        fecha_nacimiento: adopcionJSON.solicitante?.fecha_nacimiento,
        celular: adopcionJSON.solicitante?.celular,
        telefono_casa: adopcionJSON.solicitante?.telefono_casa,
        correo: adopcionJSON.solicitante?.correo,
        direccion: adopcionJSON.solicitante?.direccion,
        ingresos: adopcionJSON.solicitante?.ingresos,
        estado_civil: adopcionJSON.solicitante?.estado_civil,
        ocupacion: adopcionJSON.solicitante?.ocupacion,
        date: adopcionJSON.solicitante?.date,
        usuario: adopcionJSON.solicitante?.usuario || null
      },
      mascota: {
        id: adopcionJSON.mascotum?.id,
        nombre_mascota: adopcionJSON.mascotum?.nombre_mascota,
        tamanio: adopcionJSON.mascotum?.tamanio,
        peso: adopcionJSON.mascotum?.peso,
        color: adopcionJSON.mascotum?.color,
        comportamiento: adopcionJSON.mascotum?.comportamiento,
        foto_principal: adopcionJSON.mascotum?.foto_principal,
        foto_secundaria: adopcionJSON.mascotum?.foto_secundaria,
        foto_adicional: adopcionJSON.mascotum?.foto_adicional,
        date: adopcionJSON.mascotum?.date,
        animal: {
          id: adopcionJSON.mascotum?.animal?.id,
          especie: adopcionJSON.mascotum?.animal?.especie,
          raza: adopcionJSON.mascotum?.animal?.raza
        },
        enfermedades: adopcionJSON.mascotum?.enfermedads?.map(enfermedad => ({
          id: enfermedad.id,
          descripcion: enfermedad.descripcion,
          tratamiento: enfermedad.tratamiento,
          date: enfermedad.date,
          medicinas: enfermedad.medicinas?.map(medicina => ({
            id: medicina.id,
            nombre: medicina.nombre,
            descripcion: medicina.descripcion,
            indicaciones: medicina.indicaciones,
            date: medicina.date
          })) || []
        })) || [],
        vacunas: adopcionJSON.mascotum?.vacunas?.map(vacuna => ({
          id: vacuna.id,
          descripcion: vacuna.descripcion,
          aplicada: vacuna.aplicada,
          fecha_aplicacion: vacuna.fecha_aplicacion,
          date: vacuna.date
        })) || []
      },
      seguimientos: adopcionJSON.seguimientos?.map(seg => ({
        id: seg.id,
        fecha_seguimiento: seg.fecha_seguimiento,
        observaciones: seg.observaciones,
        date: seg.date
      })) || [],
      retornos: adopcionJSON.retornos?.map(ret => ({
        id: ret.id,
        fecha_de_retorno: ret.fecha_de_retorno,
        observaciones: ret.observaciones,
        date: ret.date
      })) || []
    };

    res.status(200).json(respuesta);

  } catch (error) {
    console.error('Error al obtener detalle de adopción:', error);
    res.status(500).json({
      type: 'https://tools.ietf.org/html/rfc7231#section-6.6.1',
      title: 'Error interno del servidor',
      status: 500,
      detail: 'Error al obtener el detalle de la solicitud de adopción',
      datetime: new Date().toISOString(),
      instance: req.originalUrl,
      error: error.message
    });
  }
};