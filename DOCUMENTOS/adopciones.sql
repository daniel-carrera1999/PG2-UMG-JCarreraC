-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema adopciones
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `adopciones` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `adopciones` ;

-- -----------------------------------------------------
-- Table `solicitante`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `solicitante` (
  `id` INT NOT NULL,
  `nombres` VARCHAR(90) NULL,
  `apellidos` VARCHAR(90) NULL,
  `fecha_nacimiento` DATE NULL,
  `celular` VARCHAR(75) NULL,
  `telefono_casa` VARCHAR(75) NULL,
  `correo` VARCHAR(120) NULL,
  `direccion` VARCHAR(200) NULL,
  `ingresos` DOUBLE NULL,
  `estado_civil` VARCHAR(45) NULL,
  `ocupacion` VARCHAR(100) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  PRIMARY KEY (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `animal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `animal` (
  `id` INT NOT NULL,
  `especie` VARCHAR(45) NULL,
  `raza` VARCHAR(45) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  PRIMARY KEY (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `mascota`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mascota` (
  `id` INT NOT NULL,
  `nombre_mascota` VARCHAR(150) NULL,
  `tamanio` VARCHAR(45) NULL,
  `peso` DOUBLE NULL,
  `color` VARCHAR(45) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_animal` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_mascota_animal1_idx` (`id_animal`),
  CONSTRAINT `fk_mascota_animal1`
    FOREIGN KEY (`id_animal`)
    REFERENCES `animal` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `usuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `usuario` (
  `id` INT NOT NULL,
  `username` VARCHAR(25) NULL,
  `correo` VARCHAR(100) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `nombre` VARCHAR(100) NULL,
  `apellido` VARCHAR(100) NULL,
  `date` DATETIME NULL,
  `inactive` TINYINT NULL,
  PRIMARY KEY (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `adopcion`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `adopcion` (
  `id` INT NOT NULL,
  `fecha_adopcion` DATE NULL,
  `no_doc` INT NULL,
  `adjunto` VARCHAR(1000) NULL,
  `date` DATETIME NULL,
  `status` TINYINT NULL,
  `id_solicitante` INT NOT NULL,
  `id_mascota` INT NOT NULL,
  `id_usuario` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_adopcion_solicitante1` FOREIGN KEY (`id_solicitante`) REFERENCES `solicitante` (`id`),
  CONSTRAINT `fk_adopcion_mascota1` FOREIGN KEY (`id_mascota`) REFERENCES `mascota` (`id`),
  CONSTRAINT `fk_adopcion_usuario1` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `bitacora`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bitacora` (
  `id` INT NOT NULL,
  `tabla` VARCHAR(50) NULL,
  `accion` VARCHAR(10) NULL,
  `fecha` TIMESTAMP NULL,
  `datos` TEXT NULL,
  `id_usuario` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_bitacora_usuario1` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `comportamiento`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `comportamiento` (
  `id` INT NOT NULL,
  `observaciones` VARCHAR(500) NULL,
  `comportamientocol` VARCHAR(1000) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_mascota` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_comportamiento_mascota1` FOREIGN KEY (`id_mascota`) REFERENCES `mascota` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `enfermedad`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `enfermedad` (
  `id` INT NOT NULL,
  `descripcion` VARCHAR(300) NULL,
  `tratamiento` VARCHAR(600) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_mascota` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_enfermedad_mascota1` FOREIGN KEY (`id_mascota`) REFERENCES `mascota` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `medicina`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `medicina` (
  `id` INT NOT NULL,
  `nombre` VARCHAR(45) NULL,
  `descripcion` VARCHAR(250) NULL,
  `indicaciones` VARCHAR(250) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_enfermedad` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_medicina_enfermedad1` FOREIGN KEY (`id_enfermedad`) REFERENCES `enfermedad` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `referencia_personal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `referencia_personal` (
  `id` INT NOT NULL,
  `nombre` VARCHAR(200) NULL,
  `telefono` VARCHAR(75) NULL,
  `vinculo` VARCHAR(45) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_solicitante` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_referencia_personal_solicitante1` FOREIGN KEY (`id_solicitante`) REFERENCES `solicitante` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `retorno`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `retorno` (
  `id` INT NOT NULL,
  `fecha_de_retorno` DATE NULL,
  `observaciones` VARCHAR(45) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_adopcion` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_retorno_adopcion1` FOREIGN KEY (`id_adopcion`) REFERENCES `adopcion` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `seguimiento`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `seguimiento` (
  `id` INT NOT NULL,
  `fecha_seguimientol` DATE NULL,
  `observaciones` VARCHAR(500) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_adopcion` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_seguimiento_adopcion1` FOREIGN KEY (`id_adopcion`) REFERENCES `adopcion` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `vacuna`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `vacuna` (
  `id` INT NOT NULL,
  `descripcion` VARCHAR(155) NULL,
  `aplicada` TINYINT NULL,
  `fecha_aplicacion` DATE NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_mascota` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_vacuna_mascota1` FOREIGN KEY (`id_mascota`) REFERENCES `mascota` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `visita`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `visita` (
  `id` INT NOT NULL,
  `fecha_visita` DATETIME NULL,
  `espacio_ideal` TINYINT NULL,
  `entorno` VARCHAR(500) NULL,
  `observaciones` VARCHAR(500) NULL,
  `date` TIMESTAMP NULL,
  `inactive` TINYINT NULL,
  `id_adopcion` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_visita_adopcion1` FOREIGN KEY (`id_adopcion`) REFERENCES `adopcion` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `rol`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `rol` (
  `id` INT NOT NULL,
  `nombre` VARCHAR(45) NULL,
  `descripcion` VARCHAR(100) NULL,
  PRIMARY KEY (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `modulo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `modulo` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(100) NULL,
  `path` VARCHAR(255) NULL,
  `descripcion` VARCHAR(255) NULL,
  PRIMARY KEY (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `permiso`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `permiso` (
  `create` TINYINT NULL,
  `read` TINYINT NULL,
  `update` TINYINT NULL,
  `delete` TINYINT NULL,
  `id_rol` INT NOT NULL,
  `id_modulo` INT NOT NULL,
  INDEX `fk_id_rol_idx` (`id_rol`),
  INDEX `fk_id_modulo_idx` (`id_modulo`),
  CONSTRAINT `fk_permiso_rol` FOREIGN KEY (`id_rol`) REFERENCES `rol` (`id`),
  CONSTRAINT `fk_permiso_modulo` FOREIGN KEY (`id_modulo`) REFERENCES `modulo` (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `rol_usuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `rol_usuario` (
  `id` INT GENERATED ALWAYS AS () VIRTUAL,
  `id_rol` INT NOT NULL,
  `id_usuario` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_rol_usuario_rol_idx` (`id_rol`),
  INDEX `fk_rol_usuario_usuario_idx` (`id_usuario`),
  CONSTRAINT `fk_rol_usuario_rol` FOREIGN KEY (`id_rol`) REFERENCES `rol` (`id`),
  CONSTRAINT `fk_rol_usuario_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id`)
) ENGINE = InnoDB;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
