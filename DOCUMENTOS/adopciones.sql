-- MySQL Workbench Forward Engineering (corregido)
SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema adoptaya
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `adoptaya` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `adoptaya`;

-- Eliminar todas las tablas del esquema adoptaya
SET FOREIGN_KEY_CHECKS=0;

DROP TABLE IF EXISTS `comportamiento`;
DROP TABLE IF EXISTS `medicina`;
DROP TABLE IF EXISTS `enfermedad`;
DROP TABLE IF EXISTS `vacuna`;
DROP TABLE IF EXISTS `referencia_personal`;
DROP TABLE IF EXISTS `retorno`;
DROP TABLE IF EXISTS `seguimiento`;
DROP TABLE IF EXISTS `bitacora`;
DROP TABLE IF EXISTS `adopcion`;
DROP TABLE IF EXISTS `rol_usuario`;
DROP TABLE IF EXISTS `permiso`;
DROP TABLE IF EXISTS `modulo`;
DROP TABLE IF EXISTS `rol`;
DROP TABLE IF EXISTS `mascota`;
DROP TABLE IF EXISTS `animal`;
DROP TABLE IF EXISTS `solicitante`;
DROP TABLE IF EXISTS `usuario`;

SET FOREIGN_KEY_CHECKS=1;

-- -----------------------------------------------------
-- Table `usuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `usuario` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(25),
  `correo` VARCHAR(100) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `nombre` VARCHAR(100),
  `apellido` VARCHAR(100),
  `date` DATETIME DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE KEY `ux_usuario_correo` (`correo`)
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `solicitante`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `solicitante` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombres` VARCHAR(90),
  `apellidos` VARCHAR(90),
  `fecha_nacimiento` DATE,
  `celular` VARCHAR(75),
  `telefono_casa` VARCHAR(75),
  `correo` VARCHAR(120),
  `direccion` VARCHAR(200),
  `ingresos` DOUBLE,
  `estado_civil` VARCHAR(45),
  `ocupacion` VARCHAR(100),
  `id_usuario` INT UNSIGNED NULL,
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE KEY `ux_solicitante_usuario` (`id_usuario`),
  INDEX `fk_solicitante_usuario1_idx` (`id_usuario`),
  CONSTRAINT `fk_solicitante_usuario1` 
    FOREIGN KEY (`id_usuario`) 
    REFERENCES `usuario` (`id`) 
    ON UPDATE CASCADE 
    ON DELETE SET NULL
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `animal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `animal` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `especie` VARCHAR(45),
  `raza` VARCHAR(45),
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `mascota`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mascota` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre_mascota` VARCHAR(150),
  `tamanio` VARCHAR(45),
  `peso` DOUBLE,
  `color` VARCHAR(45),
  `comportamiento` VARCHAR(1000),
  `foto_principal` LONGTEXT,
  `foto_secundaria` LONGTEXT,
  `foto_adicional` LONGTEXT,
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  `id_animal` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_mascota_animal1_idx` (`id_animal`),
  CONSTRAINT `fk_mascota_animal1`
    FOREIGN KEY (`id_animal`)
    REFERENCES `animal` (`id`)
    ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `adopcion`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `adopcion` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `fecha_adopcion` DATE,
  `no_doc` INT,
  `adjunto` LONGTEXT,
  `date` DATETIME NULL,
  `status` VARCHAR(50) DEFAULT 'Pendiente',
  `id_solicitante` INT UNSIGNED NOT NULL,
  `id_mascota` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_adopcion_solicitante1_idx` (`id_solicitante`),
  INDEX `fk_adopcion_mascota1_idx` (`id_mascota`),
  CONSTRAINT `fk_adopcion_solicitante1` FOREIGN KEY (`id_solicitante`) REFERENCES `solicitante` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `fk_adopcion_mascota1`      FOREIGN KEY (`id_mascota`)     REFERENCES `mascota` (`id`)     ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `bitacora`
-- -----------------------------------------------------
  CREATE TABLE IF NOT EXISTS `bitacora` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `tabla` VARCHAR(50),
    `accion` VARCHAR(10),
    `fecha` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
    `datos` TEXT,
    `id_usuario` INT UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    INDEX `fk_bitacora_usuario1_idx` (`id_usuario`),
    CONSTRAINT `fk_bitacora_usuario1` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE
  ) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `enfermedad`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `enfermedad` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `descripcion` VARCHAR(300),
  `tratamiento` VARCHAR(600),
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  `id_mascota` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_enfermedad_mascota1_idx` (`id_mascota`),
  CONSTRAINT `fk_enfermedad_mascota1` FOREIGN KEY (`id_mascota`) REFERENCES `mascota` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `medicina`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `medicina` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(45),
  `descripcion` VARCHAR(250),
  `indicaciones` VARCHAR(250),
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  `id_enfermedad` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_medicina_enfermedad1_idx` (`id_enfermedad`),
  CONSTRAINT `fk_medicina_enfermedad1` FOREIGN KEY (`id_enfermedad`) REFERENCES `enfermedad` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `referencia_personal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `referencia_personal` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(200),
  `telefono` VARCHAR(75),
  `vinculo` VARCHAR(45),
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  `id_solicitante` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_referencia_personal_solicitante1_idx` (`id_solicitante`),
  CONSTRAINT `fk_referencia_personal_solicitante1` FOREIGN KEY (`id_solicitante`) REFERENCES `solicitante` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `retorno`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `retorno` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `fecha_de_retorno` DATE,
  `observaciones` VARCHAR(255),
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  `id_adopcion` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_retorno_adopcion1_idx` (`id_adopcion`),
  CONSTRAINT `fk_retorno_adopcion1` FOREIGN KEY (`id_adopcion`) REFERENCES `adopcion` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `seguimiento`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `seguimiento` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `fecha_seguimiento` DATE,
  `observaciones` VARCHAR(500),
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  `id_adopcion` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_seguimiento_adopcion1_idx` (`id_adopcion`),
  CONSTRAINT `fk_seguimiento_adopcion1` FOREIGN KEY (`id_adopcion`) REFERENCES `adopcion` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `vacuna`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vacuna` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `descripcion` VARCHAR(155),
  `aplicada` TINYINT(1),
  `fecha_aplicacion` DATE,
  `date` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  `inactive` TINYINT(1) DEFAULT 0,
  `id_mascota` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_vacuna_mascota1_idx` (`id_mascota`),
  CONSTRAINT `fk_vacuna_mascota1` FOREIGN KEY (`id_mascota`) REFERENCES `mascota` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `rol`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `rol` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(45),
  `descripcion` VARCHAR(100),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `modulo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `modulo` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(100),
  `path` VARCHAR(255),
  `icon` VARCHAR(255),
  `descripcion` VARCHAR(255),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `permiso`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `permiso` (
  `id_rol` INT UNSIGNED NOT NULL,
  `id_modulo` INT UNSIGNED NOT NULL,
  `create` TINYINT(1) NULL,
  `read`   TINYINT(1) NULL,
  `update` TINYINT(1) NULL,
  `delete` TINYINT(1) NULL,
  PRIMARY KEY (`id_rol`, `id_modulo`),
  INDEX `fk_id_modulo_idx` (`id_modulo`),
  CONSTRAINT `fk_permiso_rol`    FOREIGN KEY (`id_rol`)    REFERENCES `rol` (`id`)       ON UPDATE CASCADE ON DELETE CASCADE,
  CONSTRAINT `fk_permiso_modulo` FOREIGN KEY (`id_modulo`) REFERENCES `modulo` (`id`)    ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB;

-- -----------------------------------------------------
-- Table `rol_usuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `rol_usuario` (
  `id_rol` INT UNSIGNED NOT NULL,
  `id_usuario` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id_rol`, `id_usuario`),
  INDEX `fk_rol_usuario_usuario_idx` (`id_usuario`),
  CONSTRAINT `fk_rol_usuario_rol`     FOREIGN KEY (`id_rol`)     REFERENCES `rol` (`id`)       ON UPDATE CASCADE ON DELETE CASCADE,
  CONSTRAINT `fk_rol_usuario_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id`)   ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;


-- -----------------------------------------------------
-- Configuración Inicial
-- -----------------------------------------------------

INSERT INTO rol (nombre, descripcion) VALUES ('Administrador', 'Acceso total');
INSERT INTO rol (nombre, descripcion) VALUES ('Solicitante', 'Acciones de solicitante');

INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Inicio', '/home', 'home', 'Página principal');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Adoptar mascota', '/adoptar_mascota', 'pets', 'Realizar solicitudes de adopción');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Mis solicitudes', '/mis_solicitudes', 'assignment_ind', 'Ver el estado de mis solicitudes de adopción');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Módulos', '/modulos', 'dashboard_2', 'Gestión de módulos');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Roles y permisos', '/roles', 'encrypted', 'Gestión de roles y permisos');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Usuarios', '/usuarios', 'person', 'Gestión de usuarios de la aplicación');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Animales', '/animales', 'cruelty_free', 'Gestión de animales y razas');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Mascotas', '/mascotas', 'pet_supplies', 'Gestión de mascotas');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Gestión de adopciones', '/adopciones', 'home_and_garden', 'Gestión de solicitudes de adopción');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Bitácora', '/log', 'history', 'Bitácora de actividades');
INSERT INTO modulo (nombre, `path`, icon, descripcion) VALUES ('Mi Perfil', '/mi_perfil', 'account_circle', 'Información de cuenta');

INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 1, 0, 1, 0, 0);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 2, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 3, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 4, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 5, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 6, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 7, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 8, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 9, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 10, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (1, 11, 1, 1, 1, 1);

INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (2, 1, 0, 1, 0, 0);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (2, 2, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (2, 3, 1, 1, 1, 1);
INSERT INTO permiso (id_rol, id_modulo, `create`, `read`, `update`, `delete`) VALUES (2, 11, 1, 1, 1, 1);

INSERT INTO usuario (username, correo, password, nombre, apellido) VALUES ('admin', 'admin@admin.com', '$2b$10$LzQb1tnFPMfYVatgUTPaf.XAQam2wMLH.SxKN/YzUtC13DqkSDycS', 'admin', 'admin');
INSERT INTO rol_usuario (id_rol, id_usuario) VALUES (1, 1);

INSERT INTO animal (especie, raza) VALUES
('Perro', 'Labrador Retriever'),
('Perro', 'Golden Retriever'),
('Gato', 'Persa'),
('Gato', 'Siamés'),
('Conejo', 'Holland Lop'),
('Conejo', 'Mini Rex');