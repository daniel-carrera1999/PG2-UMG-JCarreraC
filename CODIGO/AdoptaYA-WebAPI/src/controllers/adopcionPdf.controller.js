// controllers/adopcionPDFController.js
const PDFDocument = require('pdfkit');
const moment = require('moment');
const { adopcion, solicitante, usuario, referencia_personal, mascota, animal, enfermedad, medicina, vacuna, seguimiento, retorno } = require('../models');

exports.generarPDFAdopcion = async (req, res) => {
  try {
    const { idAdopcion } = req.params;

    const adopcionDetalle = await adopcion.findOne({
      where: { id: idAdopcion },
      include: [
        {
          model: solicitante,
          include: [
            { model: usuario },
            { model: referencia_personal, required: false }
          ]
        },
        {
          model: mascota,
          include: [
            { model: animal },
            { model: enfermedad, include: [medicina], required: false },
            { model: vacuna, required: false }
          ]
        },
        { model: seguimiento, required: false },
        { model: retorno, required: false }
      ]
    });

    if (!adopcionDetalle) {
      return res.status(404).json({ message: 'Solicitud no encontrada' });
    }

    const data = adopcionDetalle.toJSON();

    // ======== CONFIGURAR RESPUESTA ========
    res.setHeader('Content-Type', 'application/pdf');
    res.setHeader('Content-Disposition', `attachment; filename="Solicitud_Adopcion_${idAdopcion}.pdf"`);

    const doc = new PDFDocument({ margin: 40, size: 'A4' });
    doc.pipe(res);

    // ======== ENCABEZADO ========
    doc.fontSize(20).text('Solicitud de Adopción', { align: 'center' });
    doc.moveDown(0.5);
    doc.fontSize(10).text(`Generado el: ${moment().format('DD/MM/YYYY HH:mm')}`, { align: 'right' });
    doc.moveDown();

    // ======== FOTO PRINCIPAL (posicionada a la derecha del encabezado) ========
    if (data.mascotum?.foto_principal) {
        try {
            const base64Data = data.mascotum.foto_principal.replace(/^data:image\/\w+;base64,/, '');
            const imageBuffer = Buffer.from(base64Data, 'base64');
            const image = doc.openImage(imageBuffer);

            const maxWidth = 120;  // tamaño máximo (ajustable)
            const maxHeight = 120;
            const ratio = Math.min(maxWidth / image.width, maxHeight / image.height);

            const displayWidth = image.width * ratio;
            const displayHeight = image.height * ratio;

            // Coordenadas: parte superior derecha del encabezado
            const x = doc.page.width - displayWidth - 60; // margen derecho
            const y = 120; // altura fija debajo del título

            doc.image(imageBuffer, x, y, {
            width: displayWidth,
            height: displayHeight
            });
        } catch (err) {
            console.warn('Error al renderizar imagen:', err.message);
        }
    }

    // ======== INFORMACIÓN DE LA SOLICITUD ========
    doc.fontSize(14).text('Información de la Solicitud');
    doc.moveDown(0.5);
    doc.fontSize(11)
      .text(`ID de Solicitud: ${data.id}`)
      .text(`Estado: ${data.status}`)
      .text(`Fecha de Registro: ${moment(data.date).format('DD/MM/YYYY HH:mm')}`)
      .text(`Fecha de Adopción: ${data.fecha_adopcion ? moment(data.fecha_adopcion).format('DD/MM/YYYY') : 'Pendiente'}`)
      .text(`No. Documento: ${data.no_doc || 'N/A'}`)
      .moveDown();

    // ======== INFORMACIÓN DEL SOLICITANTE ========
    const s = data.solicitante;
    doc.fontSize(14).text('Información del Solicitante');
    doc.moveDown(0.5);
    doc.fontSize(11)
      .text(`Nombre Completo: ${s.nombres} ${s.apellidos}`)
      .text(`Fecha de Nacimiento: ${s.fecha_nacimiento ? moment(s.fecha_nacimiento).format('DD/MM/YYYY') : 'N/A'}`)
      .text(`Correo: ${s.correo}`)
      .text(`Celular: ${s.celular}`)
      .text(`Teléfono Casa: ${s.telefono_casa || 'N/A'}`)
      .text(`Dirección: ${s.direccion}`)
      .text(`Ingresos: Q. ${s.ingresos ? s.ingresos.toFixed(2) : 'N/A'}`)
      .text(`Estado Civil: ${s.estado_civil || 'N/A'}`)
      .text(`Ocupación: ${s.ocupacion || 'N/A'}`)
      .moveDown();

    // ======== INFORMACIÓN DE LA MASCOTA ========
    const m = data.mascotum;
    doc.fontSize(14).text('Información de la Mascota');
    doc.moveDown(0.5);
    doc.fontSize(11)
      .text(`Nombre: ${m.nombre_mascota}`)
      .text(`Especie: ${m.animal?.especie}`)
      .text(`Raza: ${m.animal?.raza}`)
      .text(`Tamaño: ${m.tamanio}`)
      .text(`Peso: ${m.peso} kg`)
      .text(`Color: ${m.color}`)
      .text(`Comportamiento: ${m.comportamiento || 'N/A'}`)
      .moveDown();

    // ======== VACUNAS ========
    doc.fontSize(14).text('Vacunas');
    doc.moveDown(0.5);
    let temp = doc.x; // Guardar posición X actual

    if (m.vacunas?.length) {
        const startX = doc.x;
        let startY = doc.y;
        const colWidths = [40, 220, 120, 120]; // #, Descripción, Estado, Fecha aplicada
        const rowHeight = 20;
        const bottomLimit = doc.page.height - doc.page.margins.bottom - 40;
        const headers = ['#', 'Descripción', 'Estado', 'Fecha aplicada'];

        // === Función para dibujar cabecera ===
        function drawHeader(y) {
            doc.fontSize(11).font('Helvetica-Bold');
            let x = startX;
            headers.forEach((h, i) => {
            doc.rect(x, y, colWidths[i], rowHeight).stroke();
            doc.text(h, x + 5, y + 5, {
                width: colWidths[i] - 10,
                ellipsis: true
            });
            x += colWidths[i];
            });
            return y + rowHeight;
        }

        // === Función para verificar espacio y saltar página si es necesario ===
        function ensureSpaceForRow(currentY) {
            if (currentY + rowHeight > bottomLimit) {
            doc.addPage();
            return drawHeader(doc.page.margins.top);
            }
            return currentY;
        }

        // === Dibujar cabecera solo si hay espacio para al menos una fila ===
        if (startY + rowHeight * 2 > bottomLimit) {
            doc.addPage();
            startY = drawHeader(doc.page.margins.top);
        } else {
            startY = drawHeader(startY);
        }

        // === Filas ===
        doc.font('Helvetica');
        m.vacunas.forEach((v, idx) => {
            startY = ensureSpaceForRow(startY);

            let x = startX;
            const values = [
            idx + 1,
            v.descripcion || '',
            v.aplicada ? 'Aplicada' : 'No aplicada',
            v.fecha_aplicacion
                ? moment(v.fecha_aplicacion).format('DD/MM/YYYY')
                : 'Sin fecha'
            ];

            values.forEach((val, i) => {
            doc.rect(x, startY, colWidths[i], rowHeight).stroke();
            doc.text(String(val), x + 5, startY + 5, {
                width: colWidths[i] - 10,
                ellipsis: true
            });
            x += colWidths[i];
            });

            startY += rowHeight;
        });

        // Ajustar cursor al final
        doc.y = startY + 10;
    } else {
        doc.fontSize(11).text('No hay vacunas registradas.').moveDown();
    }

    doc.moveDown();

    doc.x = temp; // Asegurar que el cursor esté en la posición correcta

    // ======== ENFERMEDADES ========
    doc.fontSize(14).text('Enfermedades');
    doc.moveDown(0.5);
    if (m.enfermedads?.length) {
      m.enfermedads.forEach(e => {
        doc.fontSize(11)
          .text(`• ${e.descripcion}`)
          .text(`  Tratamiento: ${e.tratamiento || 'N/A'}`)
          .moveDown(0.2);
        if (e.medicinas?.length) {
          e.medicinas.forEach(md => {
            doc.text(`     - ${md.nombre}: ${md.descripcion} (${md.indicaciones})`);
          });
        }
        doc.moveDown(0.5);
      });
    } else {
      doc.text('No hay enfermedades registradas.').moveDown();
    }

    // ======== SEGUIMIENTOS ========
    doc.fontSize(14).text('Seguimientos');
    doc.moveDown(0.5);
    if (data.seguimientos?.length) {
      data.seguimientos.forEach(seg => {
        doc.fontSize(11)
          .text(`• Fecha: ${moment(seg.fecha_seguimiento).format('DD/MM/YYYY')}`)
          .text(`  Observaciones: ${seg.observaciones}`)
          .moveDown(0.2);
      });
    } else {
      doc.text('No hay seguimientos registrados.').moveDown();
    }

    // ======== RETORNOS ========
    if(data.retornos?.length) {
        doc.fontSize(14).text('Retorno');
        doc.moveDown(0.5);

      data.retornos.forEach(ret => {
        doc.fontSize(11)
          .text(`• Fecha: ${moment(ret.fecha_de_retorno).format('DD/MM/YYYY')}`)
          .text(`  Observaciones: ${ret.observaciones}`)
          .moveDown(0.2);
      });
    }

    // ======== REFERENCIAS PERSONALES ========
    doc.moveDown(1);
    doc.fontSize(14).text('Referencias Personales');
    doc.moveDown(0.5);

    if (s.referencia_personals?.length) {
    const startX = doc.x;
    let startY = doc.y;
    const colWidths = [40, 200, 120, 120]; // #, Nombre, Teléfono, Vínculo
    const rowHeight = 20;
    const bottomLimit = doc.page.height - doc.page.margins.bottom - 40;
    const headers = ['#', 'Nombre', 'Teléfono', 'Vínculo'];

    // === Función para dibujar cabecera ===
    function drawHeader(y) {
        doc.fontSize(11).font('Helvetica-Bold');
        let x = startX;
        headers.forEach((h, i) => {
        doc.rect(x, y, colWidths[i], rowHeight).stroke();
        doc.text(h, x + 5, y + 5, { width: colWidths[i] - 10 });
        x += colWidths[i];
        });
        return y + rowHeight;
    }

    // === Función para verificar espacio y saltar página si es necesario ===
    function ensureSpaceForRow(currentY) {
        if (currentY + rowHeight > bottomLimit) {
        doc.addPage();
        return drawHeader(doc.page.margins.top);
        }
        return currentY;
    }

    // === Dibujar cabecera solo si hay espacio para al menos una fila ===
    if (startY + rowHeight * 2 > bottomLimit) {
        doc.addPage();
        startY = drawHeader(doc.page.margins.top);
    } else {
        startY = drawHeader(startY);
    }

    // === Filas ===
    doc.font('Helvetica');
    s.referencia_personals.forEach((r, idx) => {
        startY = ensureSpaceForRow(startY);

        let x = startX;
        const values = [
        idx + 1,
        r.nombre || '',
        r.telefono || '',
        r.vinculo || ''
        ];

        values.forEach((v, i) => {
        doc.rect(x, startY, colWidths[i], rowHeight).stroke();
        doc.text(String(v), x + 5, startY + 5, {
            width: colWidths[i] - 10,
            ellipsis: true
        });
        x += colWidths[i];
        });

        startY += rowHeight;
    });

    doc.y = startY + 10;
    } else {
    doc.fontSize(11).text('No hay referencias registradas.').moveDown();
    }

    // ======== FINAL ========
    doc.moveDown(1);
    doc.fontSize(10).text('--- Fin del Documento ---', { align: 'center', italics: true });

    doc.end();

  } catch (error) {
    console.error('Error al generar PDF:', error);
    res.status(500).json({ message: 'Error al generar el PDF', error: error.message });
  }
};
