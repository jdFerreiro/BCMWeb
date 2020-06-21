
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/21/2020 16:00:15
-- Generated from EDMX file: D:\Source\Repos\jdFerreiro\BCMWeb\BCMWeb\Data\EF\BCMWebEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [bcmwebtools];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_tblAprobacion_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoAprobacion] DROP CONSTRAINT [FK_tblAprobacion_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblAuditoria_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblAuditoria] DROP CONSTRAINT [FK_tblAuditoria_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblAuditoriaProcesoCritico_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblAuditoriaProcesoCritico] DROP CONSTRAINT [FK_tblAuditoriaProcesoCritico_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPReanudacionPersonaClave_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPReanudacionPersonaClave] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPReanudacionPersonaClave_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPReanudacionTarea_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPReanudacionTarea] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPReanudacionTarea_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPRecuperacionPersonaClave_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRecuperacionPersonaClave] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPRecuperacionPersonaClave_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPRecuperacionRecurso_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRecuperacionRecurso] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPRecuperacionRecurso_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPRespuestaRecurso_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRespuestaRecurso] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPRespuestaRecurso_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPRestauracionEquipo_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRestauracionEquipo] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPRestauracionEquipo_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPRestauracionInfraestructura_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRestauracionInfraestructura] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPRestauracionInfraestructura_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPRestauracionMobiliario_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRestauracionMobiliario] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPRestauracionMobiliario_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBCPRestauracionOtro_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRestauracionOtro] DROP CONSTRAINT [FK_tblBCPDocumento_tblBCPRestauracionOtro_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPDocumento] DROP CONSTRAINT [FK_tblBCPDocumento_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPDocumento_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPDocumento] DROP CONSTRAINT [FK_tblBCPDocumento_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPReanudacionTarea_tblBCPReanudacionTareaActividad_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPReanudacionTareaActividad] DROP CONSTRAINT [FK_tblBCPReanudacionTarea_tblBCPReanudacionTareaActividad_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBCPRespuestaAccion_tblBCPDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRespuestaAccion] DROP CONSTRAINT [FK_tblBCPRespuestaAccion_tblBCPDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAmenaza_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAmenaza] DROP CONSTRAINT [FK_tblBIAAmenaza_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAmenaza_tblControlRiesgo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAmenaza] DROP CONSTRAINT [FK_tblBIAAmenaza_tblControlRiesgo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAmenaza_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAmenaza] DROP CONSTRAINT [FK_tblBIAAmenaza_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAmenaza_tblEstadoRiesgo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAmenaza] DROP CONSTRAINT [FK_tblBIAAmenaza_tblEstadoRiesgo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAmenaza_tblFuenteRiesgo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAmenaza] DROP CONSTRAINT [FK_tblBIAAmenaza_tblFuenteRiesgo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAmenaza_tblImpactoRiesgo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAmenaza] DROP CONSTRAINT [FK_tblBIAAmenaza_tblImpactoRiesgo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAmenaza_tblProbabilidadRiesgo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAmenaza] DROP CONSTRAINT [FK_tblBIAAmenaza_tblProbabilidadRiesgo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAmenaza_tblSeveridadRiesgo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAmenaza] DROP CONSTRAINT [FK_tblBIAAmenaza_tblSeveridadRiesgo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAAplicacion_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAAplicacion] DROP CONSTRAINT [FK_tblBIAAplicacion_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIACadenaServicio_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIACadenaServicio] DROP CONSTRAINT [FK_tblBIACadenaServicio_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAClienteProducto_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAClienteProceso] DROP CONSTRAINT [FK_tblBIAClienteProducto_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAComentario_tblBIADocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAComentario] DROP CONSTRAINT [FK_tblBIAComentario_tblBIADocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIADocumentacion_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIADocumentacion] DROP CONSTRAINT [FK_tblBIADocumentacion_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIADocumento_tblUnidadOrganizativa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIADocumento] DROP CONSTRAINT [FK_tblBIADocumento_tblUnidadOrganizativa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAEntrada_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAEntrada] DROP CONSTRAINT [FK_tblBIAEntrada_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAEventoControl_tblBIAAmenazaEvento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAEventoControl] DROP CONSTRAINT [FK_tblBIAEventoControl_tblBIAAmenazaEvento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAGranImpacto_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAGranImpacto] DROP CONSTRAINT [FK_tblBIAGranImpacto_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAGranImpacto_tblMes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAGranImpacto] DROP CONSTRAINT [FK_tblBIAGranImpacto_tblMes];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAImpactoFinanciero_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAImpactoFinanciero] DROP CONSTRAINT [FK_tblBIAImpactoFinanciero_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAImpactoFinanciero_tblEscala]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAImpactoFinanciero] DROP CONSTRAINT [FK_tblBIAImpactoFinanciero_tblEscala];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAImpactoFinanciero_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAImpactoFinanciero] DROP CONSTRAINT [FK_tblBIAImpactoFinanciero_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAImpactoOperacional_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAImpactoOperacional] DROP CONSTRAINT [FK_tblBIAImpactoOperacional_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAImpactoOperacional_tblEscala]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAImpactoOperacional] DROP CONSTRAINT [FK_tblBIAImpactoOperacional_tblEscala];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAImpactoOperacional_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAImpactoOperacional] DROP CONSTRAINT [FK_tblBIAImpactoOperacional_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAInterdependencia_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAInterdependencia] DROP CONSTRAINT [FK_tblBIAInterdependencia_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAMTD_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAMTD] DROP CONSTRAINT [FK_tblBIAMTD_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAMTD_tblEscala]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAMTD] DROP CONSTRAINT [FK_tblBIAMTD_tblEscala];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAMTD_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAMTD] DROP CONSTRAINT [FK_tblBIAMTD_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAPersonaClave_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAPersonaClave] DROP CONSTRAINT [FK_tblBIAPersonaClave_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAPersonaClave_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAPersonaClave] DROP CONSTRAINT [FK_tblBIAPersonaClave_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAPersonaClave_tblDocumentoPersonaClave]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAPersonaClave] DROP CONSTRAINT [FK_tblBIAPersonaClave_tblDocumentoPersonaClave];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAPersonaRespaldoProceso_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAPersonaRespaldoProceso] DROP CONSTRAINT [FK_tblBIAPersonaRespaldoProceso_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAPersonaRespaldoProceso_tblPersona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAPersonaRespaldoProceso] DROP CONSTRAINT [FK_tblBIAPersonaRespaldoProceso_tblPersona];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAProbabilidadRiesgo_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblProbabilidadRiesgo] DROP CONSTRAINT [FK_tblBIAProbabilidadRiesgo_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAProceso_tblBIADocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAProceso] DROP CONSTRAINT [FK_tblBIAProceso_tblBIADocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAProceso_tblEstadoProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAProceso] DROP CONSTRAINT [FK_tblBIAProceso_tblEstadoProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAProceso_tblUnidadOrganizativa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAProceso] DROP CONSTRAINT [FK_tblBIAProceso_tblUnidadOrganizativa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAProcesoAlterno_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAProcesoAlterno] DROP CONSTRAINT [FK_tblBIAProcesoAlterno_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAProveedor_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAProveedor] DROP CONSTRAINT [FK_tblBIAProveedor_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIARespaldoPrimario_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIARespaldoPrimario] DROP CONSTRAINT [FK_tblBIARespaldoPrimario_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIARespaldoSecundario_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIARespaldoSecundario] DROP CONSTRAINT [FK_tblBIARespaldoSecundario_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIARPO_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIARPO] DROP CONSTRAINT [FK_tblBIARPO_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIARPO_tblEscala]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIARPO] DROP CONSTRAINT [FK_tblBIARPO_tblEscala];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIARPO_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIARPO] DROP CONSTRAINT [FK_tblBIARPO_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIARTO_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIARTO] DROP CONSTRAINT [FK_tblBIARTO_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIARTO_tblEscala]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIARTO] DROP CONSTRAINT [FK_tblBIARTO_tblEscala];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIARTO_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIARTO] DROP CONSTRAINT [FK_tblBIARTO_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAUnidadTrabajo_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAUnidadTrabajo] DROP CONSTRAINT [FK_tblBIAUnidadTrabajo_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAUnidadTrabajo_tblUnidadOrganizativa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAUnidadTrabajo] DROP CONSTRAINT [FK_tblBIAUnidadTrabajo_tblUnidadOrganizativa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAUnidadTrabajoPersonas_tblBIAClienteProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAUnidadTrabajoPersonas] DROP CONSTRAINT [FK_tblBIAUnidadTrabajoPersonas_tblBIAClienteProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAUnidadTrabajoPersonas_tblBIAUnidadTrabajoProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAUnidadTrabajoPersonas] DROP CONSTRAINT [FK_tblBIAUnidadTrabajoPersonas_tblBIAUnidadTrabajoProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAUnidadTrabajoProceso_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAUnidadTrabajoProceso] DROP CONSTRAINT [FK_tblBIAUnidadTrabajoProceso_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAUnidadTrabajoProceso_tblBIAUnidadTrabajo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAUnidadTrabajoProceso] DROP CONSTRAINT [FK_tblBIAUnidadTrabajoProceso_tblBIAUnidadTrabajo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAWRT_tblBIAProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAWRT] DROP CONSTRAINT [FK_tblBIAWRT_tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAWRT_tblEscala]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAWRT] DROP CONSTRAINT [FK_tblBIAWRT_tblEscala];
GO
IF OBJECT_ID(N'[dbo].[FK_tblBIAWRT_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIAWRT] DROP CONSTRAINT [FK_tblBIAWRT_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCargo_tblPersona_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersona] DROP CONSTRAINT [FK_tblCargo_tblPersona_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCertificacion_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoCertificacion] DROP CONSTRAINT [FK_tblCertificacion_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCertificacion_tblPersona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoCertificacion] DROP CONSTRAINT [FK_tblCertificacion_tblPersona];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCiudad_tblEstado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCiudad] DROP CONSTRAINT [FK_tblCiudad_tblEstado];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCiudad_tblPais]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCiudad] DROP CONSTRAINT [FK_tblCiudad_tblPais];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCriticidad_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCriticidad] DROP CONSTRAINT [FK_tblCriticidad_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCriticidad_tblTipoEscala]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCriticidad] DROP CONSTRAINT [FK_tblCriticidad_tblTipoEscala];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_Estado_tblEstado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_Estado] DROP CONSTRAINT [FK_tblCultura_Estado_tblEstado];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_EstadoDocumento_tblEstadoDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_EstadoDocumento] DROP CONSTRAINT [FK_tblCultura_EstadoDocumento_tblEstadoDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_EstadoEmpresa_tblEstadoEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_EstadoEmpresa] DROP CONSTRAINT [FK_tblCultura_EstadoEmpresa_tblEstadoEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_EstadoProceso_tblEstadoProceso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_EstadoProceso] DROP CONSTRAINT [FK_tblCultura_EstadoProceso_tblEstadoProceso];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_EstadoUsuario_tblEstadoUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_EstadoUsuario] DROP CONSTRAINT [FK_tblCultura_EstadoUsuario_tblEstadoUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_Mes_tblMes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_Mes] DROP CONSTRAINT [FK_tblCultura_Mes_tblMes];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_NivelImpacto_tblNivelImpacto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_NivelImpacto] DROP CONSTRAINT [FK_tblCultura_NivelImpacto_tblNivelImpacto];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_PBEPruebaEjecucionEstatus_tblPBEPruebaEjecucionEstatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_PBEPruebaEstatus] DROP CONSTRAINT [FK_tblCultura_PBEPruebaEjecucionEstatus_tblPBEPruebaEjecucionEstatus];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_PlanTrabajoEstatus_tblPlanTrabajoEstatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_PlanTrabajoEstatus] DROP CONSTRAINT [FK_tblCultura_PlanTrabajoEstatus_tblPlanTrabajoEstatus];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_PMTProgramacionTipoActualizacion_tblPMTProgramacionTipoActualizacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_PMTProgramacionTipoActualizacion] DROP CONSTRAINT [FK_tblCultura_PMTProgramacionTipoActualizacion_tblPMTProgramacionTipoActualizacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_PMTProgramacionTipoNotificacion_tblPMTProgramacionTipoNotificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_PMTProgramacionTipoNotificacion] DROP CONSTRAINT [FK_tblCultura_PMTProgramacionTipoNotificacion_tblPMTProgramacionTipoNotificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoCorreo_tblTipoCorreo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoCorreo] DROP CONSTRAINT [FK_tblCultura_TipoCorreo_tblTipoCorreo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoDireccion_tblTipoDireccion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoDireccion] DROP CONSTRAINT [FK_tblCultura_TipoDireccion_tblTipoDireccion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoFrecuencia_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoFrecuencia] DROP CONSTRAINT [FK_tblCultura_TipoFrecuencia_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoImpacto_tblTipoImpacto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoImpacto] DROP CONSTRAINT [FK_tblCultura_TipoImpacto_tblTipoImpacto];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoInterdependencia_tblTipoInterdependencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoInterdependencia] DROP CONSTRAINT [FK_tblCultura_TipoInterdependencia_tblTipoInterdependencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoRespaldo_tblTipoRespaldo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoRespaldo] DROP CONSTRAINT [FK_tblCultura_TipoRespaldo_tblTipoRespaldo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoResultadoPrueba_tblTipoResultadoPrueba]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoResultadoPrueba] DROP CONSTRAINT [FK_tblCultura_TipoResultadoPrueba_tblTipoResultadoPrueba];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoTablaContenido_tblTipoTablaContenido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoTablaContenido] DROP CONSTRAINT [FK_tblCultura_TipoTablaContenido_tblTipoTablaContenido];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoTelefono_tblTipoTelefono]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoTelefono] DROP CONSTRAINT [FK_tblCultura_TipoTelefono_tblTipoTelefono];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCultura_TipoUbicacionInformacion_tblTipoUbicacionInformacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_TipoUbicacionInformacion] DROP CONSTRAINT [FK_tblCultura_TipoUbicacionInformacion_tblTipoUbicacionInformacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCulture_Ciudad_tblCiudad]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_Ciudad] DROP CONSTRAINT [FK_tblCulture_Ciudad_tblCiudad];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCulture_FuenteIncidente_tblFuenteIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCulture_FuenteIncidente] DROP CONSTRAINT [FK_tblCulture_FuenteIncidente_tblFuenteIncidente];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCulture_NaturalezaIncidente_tblNaturalezaIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCulture_NaturalezaIncidente] DROP CONSTRAINT [FK_tblCulture_NaturalezaIncidente_tblNaturalezaIncidente];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCulture_NivelUsuario_tblNivelUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_NivelUsuario] DROP CONSTRAINT [FK_tblCulture_NivelUsuario_tblNivelUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCulture_Pais_tblPais]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCultura_Pais] DROP CONSTRAINT [FK_tblCulture_Pais_tblPais];
GO
IF OBJECT_ID(N'[dbo].[FK_tblCulture_TipoIncidente_tblTipoIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCulture_TipoIncidente] DROP CONSTRAINT [FK_tblCulture_TipoIncidente_tblTipoIncidente];
GO
IF OBJECT_ID(N'[EVT].[FK_tblDispositivo_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [EVT].[tblDispositivo] DROP CONSTRAINT [FK_tblDispositivo_tblUsuario];
GO
IF OBJECT_ID(N'[EVT].[FK_tblDispositivoConexion_tblDispositivo]', 'F') IS NOT NULL
    ALTER TABLE [EVT].[tblDispositivoConexion] DROP CONSTRAINT [FK_tblDispositivoConexion_tblDispositivo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDispositivoConexion_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDispositivoConexion] DROP CONSTRAINT [FK_tblDispositivoConexion_tblEmpresa];
GO
IF OBJECT_ID(N'[EVT].[FK_tblDispositivoConexion_tblEmpresa1]', 'F') IS NOT NULL
    ALTER TABLE [EVT].[tblDispositivoConexion] DROP CONSTRAINT [FK_tblDispositivoConexion_tblEmpresa1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDispositivoConexion_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDispositivoConexion] DROP CONSTRAINT [FK_tblDispositivoConexion_tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDispositivoConexion_tblUsuarioDispositivo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDispositivoConexion] DROP CONSTRAINT [FK_tblDispositivoConexion_tblUsuarioDispositivo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDispositivoEnvio_tblDispositivo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDispositivoEnvio] DROP CONSTRAINT [FK_tblDispositivoEnvio_tblDispositivo];
GO
IF OBJECT_ID(N'[EVT].[FK_tblDispositivoEnvio_tblDispositivo1]', 'F') IS NOT NULL
    ALTER TABLE [EVT].[tblDispositivoEnvio] DROP CONSTRAINT [FK_tblDispositivoEnvio_tblDispositivo1];
GO
IF OBJECT_ID(N'[EVT].[FK_tblDispositivoEnvio_tblEmpresaModulo]', 'F') IS NOT NULL
    ALTER TABLE [EVT].[tblDispositivoEnvio] DROP CONSTRAINT [FK_tblDispositivoEnvio_tblEmpresaModulo];
GO
IF OBJECT_ID(N'[EVT].[FK_tblDispositivoEnvio_tblUsuarioEnvia]', 'F') IS NOT NULL
    ALTER TABLE [EVT].[tblDispositivoEnvio] DROP CONSTRAINT [FK_tblDispositivoEnvio_tblUsuarioEnvia];
GO
IF OBJECT_ID(N'[EVT].[FK_tblDispositivoEnvio_tblUsuarioRecibe]', 'F') IS NOT NULL
    ALTER TABLE [EVT].[tblDispositivoEnvio] DROP CONSTRAINT [FK_tblDispositivoEnvio_tblUsuarioRecibe];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumento_tblBIADocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIADocumento] DROP CONSTRAINT [FK_tblDocumento_tblBIADocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumento_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumento] DROP CONSTRAINT [FK_tblDocumento_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumento_tblEstadoDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumento] DROP CONSTRAINT [FK_tblDocumento_tblEstadoDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumentoAnexo_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoAnexo] DROP CONSTRAINT [FK_tblDocumentoAnexo_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumentoAprobacion_tblPersona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoAprobacion] DROP CONSTRAINT [FK_tblDocumentoAprobacion_tblPersona];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumentoContenido_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoContenido] DROP CONSTRAINT [FK_tblDocumentoContenido_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumentoContenido_tblModulo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoContenido] DROP CONSTRAINT [FK_tblDocumentoContenido_tblModulo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumentoEntrevista_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoEntrevista] DROP CONSTRAINT [FK_tblDocumentoEntrevista_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumentoEntrevistaPersona_tblDocumentoEntrevista]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoEntrevistaPersona] DROP CONSTRAINT [FK_tblDocumentoEntrevistaPersona_tblDocumentoEntrevista];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumentoPersonaClave_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoPersonaClave] DROP CONSTRAINT [FK_tblDocumentoPersonaClave_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblDocumentoPersonaClave_tblPersona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoPersonaClave] DROP CONSTRAINT [FK_tblDocumentoPersonaClave_tblPersona];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblAuditoria_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblAuditoria] DROP CONSTRAINT [FK_tblEmpresa_tblAuditoria_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblBCPDocumento_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPDocumento] DROP CONSTRAINT [FK_tblEmpresa_tblBCPDocumento_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblBIADocumento_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBIADocumento] DROP CONSTRAINT [FK_tblEmpresa_tblBIADocumento_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblCargo_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblCargo] DROP CONSTRAINT [FK_tblEmpresa_tblCargo_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblCiudad]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresa] DROP CONSTRAINT [FK_tblEmpresa_tblCiudad];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblEscala_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEscala] DROP CONSTRAINT [FK_tblEmpresa_tblEscala_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblEstado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresa] DROP CONSTRAINT [FK_tblEmpresa_tblEstado];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblEstadoEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresa] DROP CONSTRAINT [FK_tblEmpresa_tblEstadoEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblLocalidad_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblLocalidad] DROP CONSTRAINT [FK_tblEmpresa_tblLocalidad_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblPais]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresa] DROP CONSTRAINT [FK_tblEmpresa_tblPais];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblPersona_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersona] DROP CONSTRAINT [FK_tblEmpresa_tblPersona_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblProveedor_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblProveedor] DROP CONSTRAINT [FK_tblEmpresa_tblProveedor_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblUnidadOrganizativa_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblUnidadOrganizativa] DROP CONSTRAINT [FK_tblEmpresa_tblUnidadOrganizativa_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresa_tblVicepresidencia_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblVicepresidencia] DROP CONSTRAINT [FK_tblEmpresa_tblVicepresidencia_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresaModulo_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresaModulo] DROP CONSTRAINT [FK_tblEmpresaModulo_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresaModulo_tblModulo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresaModulo] DROP CONSTRAINT [FK_tblEmpresaModulo_tblModulo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresaUsuario_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresaUsuario] DROP CONSTRAINT [FK_tblEmpresaUsuario_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresaUsuario_tblNivelUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresaUsuario] DROP CONSTRAINT [FK_tblEmpresaUsuario_tblNivelUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEmpresaUsuario_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEmpresaUsuario] DROP CONSTRAINT [FK_tblEmpresaUsuario_tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblEscala_tblTipoEscala]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEscala] DROP CONSTRAINT [FK_tblEscala_tblTipoEscala];
GO
IF OBJECT_ID(N'[dbo].[FK_tblFormatosEmail_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblFormatosEmail] DROP CONSTRAINT [FK_tblFormatosEmail_tblEmpresa];
GO
IF OBJECT_ID(N'[PMI].[FK_tblIncidentes_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [PMI].[tblIncidentes] DROP CONSTRAINT [FK_tblIncidentes_tblEmpresa];
GO
IF OBJECT_ID(N'[PMI].[FK_tblIncidentes_tblFuenteIncidente]', 'F') IS NOT NULL
    ALTER TABLE [PMI].[tblIncidentes] DROP CONSTRAINT [FK_tblIncidentes_tblFuenteIncidente];
GO
IF OBJECT_ID(N'[PMI].[FK_tblIncidentes_tblNaturalezaIncidente]', 'F') IS NOT NULL
    ALTER TABLE [PMI].[tblIncidentes] DROP CONSTRAINT [FK_tblIncidentes_tblNaturalezaIncidente];
GO
IF OBJECT_ID(N'[PMI].[FK_tblIncidentes_tblTipoIncidente]', 'F') IS NOT NULL
    ALTER TABLE [PMI].[tblIncidentes] DROP CONSTRAINT [FK_tblIncidentes_tblTipoIncidente];
GO
IF OBJECT_ID(N'[dbo].[FK_tblIniciativas_Anexo_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblIniciativas_Anexo] DROP CONSTRAINT [FK_tblIniciativas_Anexo_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblIniciativas_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblIniciativas] DROP CONSTRAINT [FK_tblIniciativas_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblIniciativas_tblIniciativaPrioridad]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblIniciativas] DROP CONSTRAINT [FK_tblIniciativas_tblIniciativaPrioridad];
GO
IF OBJECT_ID(N'[dbo].[FK_tblIniciativas_tblPlanTrabajoEstatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblIniciativas] DROP CONSTRAINT [FK_tblIniciativas_tblPlanTrabajoEstatus];
GO
IF OBJECT_ID(N'[dbo].[FK_tblLocalidad_tblCiudad]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblLocalidad] DROP CONSTRAINT [FK_tblLocalidad_tblCiudad];
GO
IF OBJECT_ID(N'[dbo].[FK_tblLocalidad_tblEstado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblLocalidad] DROP CONSTRAINT [FK_tblLocalidad_tblEstado];
GO
IF OBJECT_ID(N'[dbo].[FK_tblLocalidad_tblPais]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblLocalidad] DROP CONSTRAINT [FK_tblLocalidad_tblPais];
GO
IF OBJECT_ID(N'[dbo].[FK_tblModulo_NivelUsuario_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblModulo_NivelUsuario] DROP CONSTRAINT [FK_tblModulo_NivelUsuario_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblModulo_NivelUsuario_tblModulo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblModulo_NivelUsuario] DROP CONSTRAINT [FK_tblModulo_NivelUsuario_tblModulo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblModulo_NivelUsuario_tblNivelUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblModulo_NivelUsuario] DROP CONSTRAINT [FK_tblModulo_NivelUsuario_tblNivelUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblModulo_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblModulo] DROP CONSTRAINT [FK_tblModulo_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblModulo_Usuario_tblModulo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblModulo_Usuario] DROP CONSTRAINT [FK_tblModulo_Usuario_tblModulo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblModulo_Usuario_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblModulo_Usuario] DROP CONSTRAINT [FK_tblModulo_Usuario_tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblModuloAnexo_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblModuloAnexo] DROP CONSTRAINT [FK_tblModuloAnexo_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPais_tblEstado_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblEstado] DROP CONSTRAINT [FK_tblPais_tblEstado_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucion_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucion] DROP CONSTRAINT [FK_tblPBEPruebaEjecucion_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucion_tblPBEPruebaPlanificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucion] DROP CONSTRAINT [FK_tblPBEPruebaEjecucion_tblPBEPruebaPlanificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucionEjercicio_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicio] DROP CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicio_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucionEjercicio_tblPBEPruebaEjecucion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicio] DROP CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicio_tblPBEPruebaEjecucion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucionEjercicio_tblPBEPruebaEjecucionEstatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicio] DROP CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicio_tblPBEPruebaEjecucionEstatus];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucionEjercicioParticipante_tblPBEPruebaEjecucionEjercicio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicioParticipante] DROP CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicioParticipante_tblPBEPruebaEjecucionEjercicio];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucionEjercicioParticipante_tblPBEPruebaEjecucionParticipante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicioParticipante] DROP CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicioParticipante_tblPBEPruebaEjecucionParticipante];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucionEjercicioRecurso_tblPBEPruebaEjecucionEjercicio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicioRecurso] DROP CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicioRecurso_tblPBEPruebaEjecucionEjercicio];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucionParticipante_tblPBEPruebaEjecucion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucionParticipante] DROP CONSTRAINT [FK_tblPBEPruebaEjecucionParticipante_tblPBEPruebaEjecucion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaEjecucionResultado_tblPBEPruebaEjecucion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaEjecucionResultado] DROP CONSTRAINT [FK_tblPBEPruebaEjecucionResultado_tblPBEPruebaEjecucion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaPlanificacion_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaPlanificacion] DROP CONSTRAINT [FK_tblPBEPruebaPlanificacion_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaPlanificacionEjercicio_tblPBEPruebaPlanificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicio] DROP CONSTRAINT [FK_tblPBEPruebaPlanificacionEjercicio_tblPBEPruebaPlanificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaPlanificacionEjercicioParticipante_tblPBEPruebaPlanificacionEjercicio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioParticipante] DROP CONSTRAINT [FK_tblPBEPruebaPlanificacionEjercicioParticipante_tblPBEPruebaPlanificacionEjercicio];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaPlanificacionEjercicioParticipante_tblPBEPruebaPlanificacionParticipante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioParticipante] DROP CONSTRAINT [FK_tblPBEPruebaPlanificacionEjercicioParticipante_tblPBEPruebaPlanificacionParticipante];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaPlanificacionEjercicioRecurso_tblPBEPruebaPlanificacionEjercicio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioRecurso] DROP CONSTRAINT [FK_tblPBEPruebaPlanificacionEjercicioRecurso_tblPBEPruebaPlanificacionEjercicio];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaPlanificacionParticipante_tblPBEPruebaPlanificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaPlanificacionParticipante] DROP CONSTRAINT [FK_tblPBEPruebaPlanificacionParticipante_tblPBEPruebaPlanificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPBEPruebaPlanificacionRecurso_tblPBEPruebaPlanificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioRecurso] DROP CONSTRAINT [FK_tblPBEPruebaPlanificacionRecurso_tblPBEPruebaPlanificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersona_tblBCPRecuperacionPersonaClave_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPRecuperacionPersonaClave] DROP CONSTRAINT [FK_tblPersona_tblBCPRecuperacionPersonaClave_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersona_tblPersonaDireccion_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaDireccion] DROP CONSTRAINT [FK_tblPersona_tblPersonaDireccion_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersona_tblPersonaTelefono_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaTelefono] DROP CONSTRAINT [FK_tblPersona_tblPersonaTelefono_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersona_tblReanudacionPersonaClave_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblBCPReanudacionPersonaClave] DROP CONSTRAINT [FK_tblPersona_tblReanudacionPersonaClave_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersona_tblUnidadOrganizativa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersona] DROP CONSTRAINT [FK_tblPersona_tblUnidadOrganizativa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersona_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersona] DROP CONSTRAINT [FK_tblPersona_tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersonaClave_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblDocumentoPersonaClave] DROP CONSTRAINT [FK_tblPersonaClave_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersonaCorreo_tblPersona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaCorreo] DROP CONSTRAINT [FK_tblPersonaCorreo_tblPersona];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersonaCorreo_tblTipoCorreo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaCorreo] DROP CONSTRAINT [FK_tblPersonaCorreo_tblTipoCorreo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersonaDireccion_tblCiudad]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaDireccion] DROP CONSTRAINT [FK_tblPersonaDireccion_tblCiudad];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersonaDireccion_tblEstado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaDireccion] DROP CONSTRAINT [FK_tblPersonaDireccion_tblEstado];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersonaDireccion_tblPais]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaDireccion] DROP CONSTRAINT [FK_tblPersonaDireccion_tblPais];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersonaDireccion_tblTipoDireccion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaDireccion] DROP CONSTRAINT [FK_tblPersonaDireccion_tblTipoDireccion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPersonaTelefono_tblTipoTelefono]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPersonaTelefono] DROP CONSTRAINT [FK_tblPersonaTelefono_tblTipoTelefono];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPlanTrabajo_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPlanTrabajo] DROP CONSTRAINT [FK_tblPlanTrabajo_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPlanTrabajo_tblPlanTrabajoAccion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPlanTrabajo] DROP CONSTRAINT [FK_tblPlanTrabajo_tblPlanTrabajoAccion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPlanTrabajo_tblPlanTrabajoEstatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPlanTrabajo] DROP CONSTRAINT [FK_tblPlanTrabajo_tblPlanTrabajoEstatus];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPlanTrabajoAccion_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPlanTrabajoAccion] DROP CONSTRAINT [FK_tblPlanTrabajoAccion_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPlanTrabajoAuditoria_tblPlanTrabajo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPlanTrabajoAuditoria] DROP CONSTRAINT [FK_tblPlanTrabajoAuditoria_tblPlanTrabajo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPlanTrabajoAuditoria_tblPlanTrabajoAccion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPlanTrabajoAuditoria] DROP CONSTRAINT [FK_tblPlanTrabajoAuditoria_tblPlanTrabajoAccion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPlanTrabajoAuditoria_tblPlanTrabajoEstatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPlanTrabajoAuditoria] DROP CONSTRAINT [FK_tblPlanTrabajoAuditoria_tblPlanTrabajoEstatus];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPlanTrabajoAuditoria_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPlanTrabajoAuditoria] DROP CONSTRAINT [FK_tblPlanTrabajoAuditoria_tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTMensajeActualizacion_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTMensajeActualizacion] DROP CONSTRAINT [FK_tblPMTMensajeActualizacion_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTMensajeActualizacion_tblModulo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTMensajeActualizacion] DROP CONSTRAINT [FK_tblPMTMensajeActualizacion_tblModulo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacion_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacion] DROP CONSTRAINT [FK_tblPMTProgramacion_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacion_tblModulo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacion] DROP CONSTRAINT [FK_tblPMTProgramacion_tblModulo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacion_tblPMTProgramacionTipoActualizacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacion] DROP CONSTRAINT [FK_tblPMTProgramacion_tblPMTProgramacionTipoActualizacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacion_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacion] DROP CONSTRAINT [FK_tblPMTProgramacion_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacionDocumentos_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacionDocumentos] DROP CONSTRAINT [FK_tblPMTProgramacionDocumentos_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacionDocumentos_tblModulo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacionDocumentos] DROP CONSTRAINT [FK_tblPMTProgramacionDocumentos_tblModulo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacionDocumentos_tblPMTProgramacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacionDocumentos] DROP CONSTRAINT [FK_tblPMTProgramacionDocumentos_tblPMTProgramacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacionUsuario_tblPMTProgramacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacionUsuario] DROP CONSTRAINT [FK_tblPMTProgramacionUsuario_tblPMTProgramacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacionUsuario_tblPMTProgramacionTipoNotificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacionUsuario] DROP CONSTRAINT [FK_tblPMTProgramacionUsuario_tblPMTProgramacionTipoNotificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTProgramacionUsuario_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTProgramacionUsuario] DROP CONSTRAINT [FK_tblPMTProgramacionUsuario_tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTResponsableUpdate_Correo_tblPMTResponsableUpdate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTResponsableUpdate_Correo] DROP CONSTRAINT [FK_tblPMTResponsableUpdate_Correo_tblPMTResponsableUpdate];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTResponsableUpdate_Correo_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTResponsableUpdate_Correo] DROP CONSTRAINT [FK_tblPMTResponsableUpdate_Correo_tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTResponsableUpdate_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTResponsableUpdate] DROP CONSTRAINT [FK_tblPMTResponsableUpdate_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPMTResponsableUpdate_tblModulo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPMTResponsableUpdate] DROP CONSTRAINT [FK_tblPMTResponsableUpdate_tblModulo];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPPEFrecuencia_tblDocumento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPPEFrecuencia] DROP CONSTRAINT [FK_tblPPEFrecuencia_tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[FK_tblPPEFrecuencia_tblTipoFrecuencia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblPPEFrecuencia] DROP CONSTRAINT [FK_tblPPEFrecuencia_tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[FK_tblProducto_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblProducto] DROP CONSTRAINT [FK_tblProducto_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblUsuario_tblAuditoria_FK1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblAuditoria] DROP CONSTRAINT [FK_tblUsuario_tblAuditoria_FK1];
GO
IF OBJECT_ID(N'[dbo].[FK_tblUsuario_tblEstadoUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblUsuario] DROP CONSTRAINT [FK_tblUsuario_tblEstadoUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblUsuarioUnidadOrganizativa_tblEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa] DROP CONSTRAINT [FK_tblUsuarioUnidadOrganizativa_tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblUsuarioUnidadOrganizativa_tblNivelUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa] DROP CONSTRAINT [FK_tblUsuarioUnidadOrganizativa_tblNivelUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblUsuarioUnidadOrganizativa_tblUnidadOrganizativa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa] DROP CONSTRAINT [FK_tblUsuarioUnidadOrganizativa_tblUnidadOrganizativa];
GO
IF OBJECT_ID(N'[dbo].[FK_tblUsuarioUnidadOrganizativa_tblUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa] DROP CONSTRAINT [FK_tblUsuarioUnidadOrganizativa_tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_tblVicepresidencia_tblPais]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tblVicepresidencia] DROP CONSTRAINT [FK_tblVicepresidencia_tblPais];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[tblAuditoria]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblAuditoria];
GO
IF OBJECT_ID(N'[dbo].[tblAuditoriaProcesoCritico]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblAuditoriaProcesoCritico];
GO
IF OBJECT_ID(N'[dbo].[tblBCPDocumento]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPDocumento];
GO
IF OBJECT_ID(N'[dbo].[tblBCPReanudacionPersonaClave]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPReanudacionPersonaClave];
GO
IF OBJECT_ID(N'[dbo].[tblBCPReanudacionTarea]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPReanudacionTarea];
GO
IF OBJECT_ID(N'[dbo].[tblBCPReanudacionTareaActividad]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPReanudacionTareaActividad];
GO
IF OBJECT_ID(N'[dbo].[tblBCPRecuperacionPersonaClave]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPRecuperacionPersonaClave];
GO
IF OBJECT_ID(N'[dbo].[tblBCPRecuperacionRecurso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPRecuperacionRecurso];
GO
IF OBJECT_ID(N'[dbo].[tblBCPRespuestaAccion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPRespuestaAccion];
GO
IF OBJECT_ID(N'[dbo].[tblBCPRespuestaRecurso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPRespuestaRecurso];
GO
IF OBJECT_ID(N'[dbo].[tblBCPRestauracionEquipo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPRestauracionEquipo];
GO
IF OBJECT_ID(N'[dbo].[tblBCPRestauracionInfraestructura]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPRestauracionInfraestructura];
GO
IF OBJECT_ID(N'[dbo].[tblBCPRestauracionMobiliario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPRestauracionMobiliario];
GO
IF OBJECT_ID(N'[dbo].[tblBCPRestauracionOtro]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBCPRestauracionOtro];
GO
IF OBJECT_ID(N'[dbo].[tblBIAAmenaza]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAAmenaza];
GO
IF OBJECT_ID(N'[dbo].[tblBIAAmenazaEvento]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAAmenazaEvento];
GO
IF OBJECT_ID(N'[dbo].[tblBIAAplicacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAAplicacion];
GO
IF OBJECT_ID(N'[dbo].[tblBIACadenaServicio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIACadenaServicio];
GO
IF OBJECT_ID(N'[dbo].[tblBIAClienteProceso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAClienteProceso];
GO
IF OBJECT_ID(N'[dbo].[tblBIAComentario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAComentario];
GO
IF OBJECT_ID(N'[dbo].[tblBIADocumentacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIADocumentacion];
GO
IF OBJECT_ID(N'[dbo].[tblBIADocumento]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIADocumento];
GO
IF OBJECT_ID(N'[dbo].[tblBIAEntrada]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAEntrada];
GO
IF OBJECT_ID(N'[dbo].[tblBIAEventoControl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAEventoControl];
GO
IF OBJECT_ID(N'[dbo].[tblBIAEventoRiesgo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAEventoRiesgo];
GO
IF OBJECT_ID(N'[dbo].[tblBIAGranImpacto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAGranImpacto];
GO
IF OBJECT_ID(N'[dbo].[tblBIAImpactoFinanciero]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAImpactoFinanciero];
GO
IF OBJECT_ID(N'[dbo].[tblBIAImpactoOperacional]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAImpactoOperacional];
GO
IF OBJECT_ID(N'[dbo].[tblBIAInterdependencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAInterdependencia];
GO
IF OBJECT_ID(N'[dbo].[tblBIAMTD]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAMTD];
GO
IF OBJECT_ID(N'[dbo].[tblBIAPersonaClave]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAPersonaClave];
GO
IF OBJECT_ID(N'[dbo].[tblBIAPersonaRespaldoProceso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAPersonaRespaldoProceso];
GO
IF OBJECT_ID(N'[dbo].[tblBIAProceso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAProceso];
GO
IF OBJECT_ID(N'[dbo].[tblBIAProcesoAlterno]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAProcesoAlterno];
GO
IF OBJECT_ID(N'[dbo].[tblBIAProveedor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAProveedor];
GO
IF OBJECT_ID(N'[dbo].[tblBIARespaldoPrimario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIARespaldoPrimario];
GO
IF OBJECT_ID(N'[dbo].[tblBIARespaldoSecundario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIARespaldoSecundario];
GO
IF OBJECT_ID(N'[dbo].[tblBIARPO]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIARPO];
GO
IF OBJECT_ID(N'[dbo].[tblBIARTO]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIARTO];
GO
IF OBJECT_ID(N'[dbo].[tblBIAUnidadTrabajo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAUnidadTrabajo];
GO
IF OBJECT_ID(N'[dbo].[tblBIAUnidadTrabajoPersonas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAUnidadTrabajoPersonas];
GO
IF OBJECT_ID(N'[dbo].[tblBIAUnidadTrabajoProceso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAUnidadTrabajoProceso];
GO
IF OBJECT_ID(N'[dbo].[tblBIAWRT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBIAWRT];
GO
IF OBJECT_ID(N'[dbo].[tblCargo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCargo];
GO
IF OBJECT_ID(N'[dbo].[tblCiudad]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCiudad];
GO
IF OBJECT_ID(N'[dbo].[tblControlRiesgo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblControlRiesgo];
GO
IF OBJECT_ID(N'[dbo].[tblCriticidad]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCriticidad];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_Ciudad]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_Ciudad];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_Estado]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_Estado];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_EstadoDocumento]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_EstadoDocumento];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_EstadoEmpresa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_EstadoEmpresa];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_EstadoProceso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_EstadoProceso];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_EstadoUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_EstadoUsuario];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_Mes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_Mes];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_NivelImpacto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_NivelImpacto];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_NivelUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_NivelUsuario];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_Pais]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_Pais];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_PBEPruebaEstatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_PBEPruebaEstatus];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_PlanTrabajoEstatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_PlanTrabajoEstatus];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_PMTProgramacionTipoActualizacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_PMTProgramacionTipoActualizacion];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_PMTProgramacionTipoNotificacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_PMTProgramacionTipoNotificacion];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoCorreo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoCorreo];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoDireccion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoDireccion];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoFrecuencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoImpacto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoImpacto];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoInterdependencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoInterdependencia];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoRespaldo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoRespaldo];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoResultadoPrueba]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoResultadoPrueba];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoTablaContenido]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoTablaContenido];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoTelefono]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoTelefono];
GO
IF OBJECT_ID(N'[dbo].[tblCultura_TipoUbicacionInformacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCultura_TipoUbicacionInformacion];
GO
IF OBJECT_ID(N'[dbo].[tblCulture_FuenteIncidente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCulture_FuenteIncidente];
GO
IF OBJECT_ID(N'[dbo].[tblCulture_NaturalezaIncidente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCulture_NaturalezaIncidente];
GO
IF OBJECT_ID(N'[dbo].[tblCulture_TipoIncidente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblCulture_TipoIncidente];
GO
IF OBJECT_ID(N'[dbo].[tblDispositivo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDispositivo];
GO
IF OBJECT_ID(N'[dbo].[tblDispositivoConexion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDispositivoConexion];
GO
IF OBJECT_ID(N'[dbo].[tblDispositivoEnvio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDispositivoEnvio];
GO
IF OBJECT_ID(N'[dbo].[tblDocumento]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDocumento];
GO
IF OBJECT_ID(N'[dbo].[tblDocumentoAnexo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDocumentoAnexo];
GO
IF OBJECT_ID(N'[dbo].[tblDocumentoAprobacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDocumentoAprobacion];
GO
IF OBJECT_ID(N'[dbo].[tblDocumentoCertificacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDocumentoCertificacion];
GO
IF OBJECT_ID(N'[dbo].[tblDocumentoContenido]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDocumentoContenido];
GO
IF OBJECT_ID(N'[dbo].[tblDocumentoEntrevista]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDocumentoEntrevista];
GO
IF OBJECT_ID(N'[dbo].[tblDocumentoEntrevistaPersona]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDocumentoEntrevistaPersona];
GO
IF OBJECT_ID(N'[dbo].[tblDocumentoPersonaClave]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDocumentoPersonaClave];
GO
IF OBJECT_ID(N'[dbo].[tblEmpresa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEmpresa];
GO
IF OBJECT_ID(N'[dbo].[tblEmpresaModulo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEmpresaModulo];
GO
IF OBJECT_ID(N'[dbo].[tblEmpresaUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEmpresaUsuario];
GO
IF OBJECT_ID(N'[dbo].[tblEscala]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEscala];
GO
IF OBJECT_ID(N'[dbo].[tblEstado]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEstado];
GO
IF OBJECT_ID(N'[dbo].[tblEstadoDocumento]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEstadoDocumento];
GO
IF OBJECT_ID(N'[dbo].[tblEstadoEmpresa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEstadoEmpresa];
GO
IF OBJECT_ID(N'[dbo].[tblEstadoProceso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEstadoProceso];
GO
IF OBJECT_ID(N'[dbo].[tblEstadoRiesgo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEstadoRiesgo];
GO
IF OBJECT_ID(N'[dbo].[tblEstadoUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblEstadoUsuario];
GO
IF OBJECT_ID(N'[dbo].[tblFormatosEmail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblFormatosEmail];
GO
IF OBJECT_ID(N'[dbo].[tblFuenteRiesgo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblFuenteRiesgo];
GO
IF OBJECT_ID(N'[dbo].[tblImpactoRiesgo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblImpactoRiesgo];
GO
IF OBJECT_ID(N'[dbo].[tblIniciativaPrioridad]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblIniciativaPrioridad];
GO
IF OBJECT_ID(N'[dbo].[tblIniciativaResponsable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblIniciativaResponsable];
GO
IF OBJECT_ID(N'[dbo].[tblIniciativas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblIniciativas];
GO
IF OBJECT_ID(N'[dbo].[tblIniciativas_Anexo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblIniciativas_Anexo];
GO
IF OBJECT_ID(N'[dbo].[tblLocalidad]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblLocalidad];
GO
IF OBJECT_ID(N'[dbo].[tblMes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblMes];
GO
IF OBJECT_ID(N'[dbo].[tblModulo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblModulo];
GO
IF OBJECT_ID(N'[dbo].[tblModulo_NivelUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblModulo_NivelUsuario];
GO
IF OBJECT_ID(N'[dbo].[tblModulo_Usuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblModulo_Usuario];
GO
IF OBJECT_ID(N'[dbo].[tblModuloAnexo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblModuloAnexo];
GO
IF OBJECT_ID(N'[dbo].[tblNivelImpacto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblNivelImpacto];
GO
IF OBJECT_ID(N'[dbo].[tblNivelUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblNivelUsuario];
GO
IF OBJECT_ID(N'[dbo].[tblPais]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPais];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaEjecucion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaEjecucion];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaEjecucionEjercicio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaEjecucionEjercicio];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaEjecucionEjercicioParticipante]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaEjecucionEjercicioParticipante];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaEjecucionEjercicioRecurso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaEjecucionEjercicioRecurso];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaEjecucionParticipante]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaEjecucionParticipante];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaEjecucionResultado]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaEjecucionResultado];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaEstatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaEstatus];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaPlanificacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaPlanificacion];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaPlanificacionEjercicio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaPlanificacionEjercicio];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaPlanificacionEjercicioParticipante]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioParticipante];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaPlanificacionEjercicioRecurso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioRecurso];
GO
IF OBJECT_ID(N'[dbo].[tblPBEPruebaPlanificacionParticipante]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPBEPruebaPlanificacionParticipante];
GO
IF OBJECT_ID(N'[dbo].[tblPersona]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPersona];
GO
IF OBJECT_ID(N'[dbo].[tblPersonaCorreo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPersonaCorreo];
GO
IF OBJECT_ID(N'[dbo].[tblPersonaDireccion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPersonaDireccion];
GO
IF OBJECT_ID(N'[dbo].[tblPersonaTelefono]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPersonaTelefono];
GO
IF OBJECT_ID(N'[dbo].[tblPlanTrabajo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPlanTrabajo];
GO
IF OBJECT_ID(N'[dbo].[tblPlanTrabajoAccion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPlanTrabajoAccion];
GO
IF OBJECT_ID(N'[dbo].[tblPlanTrabajoAuditoria]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPlanTrabajoAuditoria];
GO
IF OBJECT_ID(N'[dbo].[tblPlanTrabajoEstatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPlanTrabajoEstatus];
GO
IF OBJECT_ID(N'[dbo].[tblPMTMensajeActualizacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPMTMensajeActualizacion];
GO
IF OBJECT_ID(N'[dbo].[tblPMTProgramacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPMTProgramacion];
GO
IF OBJECT_ID(N'[dbo].[tblPMTProgramacionDocumentos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPMTProgramacionDocumentos];
GO
IF OBJECT_ID(N'[dbo].[tblPMTProgramacionTipoActualizacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPMTProgramacionTipoActualizacion];
GO
IF OBJECT_ID(N'[dbo].[tblPMTProgramacionTipoNotificacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPMTProgramacionTipoNotificacion];
GO
IF OBJECT_ID(N'[dbo].[tblPMTProgramacionUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPMTProgramacionUsuario];
GO
IF OBJECT_ID(N'[dbo].[tblPMTResponsableUpdate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPMTResponsableUpdate];
GO
IF OBJECT_ID(N'[dbo].[tblPMTResponsableUpdate_Correo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPMTResponsableUpdate_Correo];
GO
IF OBJECT_ID(N'[dbo].[tblPPEFrecuencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPPEFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[tblProbabilidadRiesgo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblProbabilidadRiesgo];
GO
IF OBJECT_ID(N'[dbo].[tblProducto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblProducto];
GO
IF OBJECT_ID(N'[dbo].[tblProveedor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblProveedor];
GO
IF OBJECT_ID(N'[dbo].[tblSeveridadRiesgo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblSeveridadRiesgo];
GO
IF OBJECT_ID(N'[dbo].[tblTipoCorreo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoCorreo];
GO
IF OBJECT_ID(N'[dbo].[tblTipoDireccion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoDireccion];
GO
IF OBJECT_ID(N'[dbo].[tblTipoEscala]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoEscala];
GO
IF OBJECT_ID(N'[dbo].[tblTipoFrecuencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoFrecuencia];
GO
IF OBJECT_ID(N'[dbo].[tblTipoImpacto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoImpacto];
GO
IF OBJECT_ID(N'[dbo].[tblTipoInterdependencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoInterdependencia];
GO
IF OBJECT_ID(N'[dbo].[tblTipoRespaldo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoRespaldo];
GO
IF OBJECT_ID(N'[dbo].[tblTipoResultadoPrueba]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoResultadoPrueba];
GO
IF OBJECT_ID(N'[dbo].[tblTipoTablaContenido]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoTablaContenido];
GO
IF OBJECT_ID(N'[dbo].[tblTipoTelefono]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoTelefono];
GO
IF OBJECT_ID(N'[dbo].[tblTipoUbicacionInformacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblTipoUbicacionInformacion];
GO
IF OBJECT_ID(N'[dbo].[tblUnidadOrganizativa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblUnidadOrganizativa];
GO
IF OBJECT_ID(N'[dbo].[tblUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblUsuario];
GO
IF OBJECT_ID(N'[dbo].[tblUsuarioUnidadOrganizativa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblUsuarioUnidadOrganizativa];
GO
IF OBJECT_ID(N'[dbo].[tblVicepresidencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblVicepresidencia];
GO
IF OBJECT_ID(N'[EVT].[tblDispositivo]', 'U') IS NOT NULL
    DROP TABLE [EVT].[tblDispositivo];
GO
IF OBJECT_ID(N'[EVT].[tblDispositivoConexion]', 'U') IS NOT NULL
    DROP TABLE [EVT].[tblDispositivoConexion];
GO
IF OBJECT_ID(N'[EVT].[tblDispositivoEnvio]', 'U') IS NOT NULL
    DROP TABLE [EVT].[tblDispositivoEnvio];
GO
IF OBJECT_ID(N'[PMI].[tblFuenteIncidente]', 'U') IS NOT NULL
    DROP TABLE [PMI].[tblFuenteIncidente];
GO
IF OBJECT_ID(N'[PMI].[tblIncidentes]', 'U') IS NOT NULL
    DROP TABLE [PMI].[tblIncidentes];
GO
IF OBJECT_ID(N'[PMI].[tblNaturalezaIncidente]', 'U') IS NOT NULL
    DROP TABLE [PMI].[tblNaturalezaIncidente];
GO
IF OBJECT_ID(N'[PMI].[tblTipoIncidente]', 'U') IS NOT NULL
    DROP TABLE [PMI].[tblTipoIncidente];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'tblAuditoria'
CREATE TABLE [dbo].[tblAuditoria] (
    [IdAuditoria] bigint IDENTITY(1,1) NOT NULL,
    [IdEmpresa] bigint  NULL,
    [IdDocumento] bigint  NULL,
    [IdTipoDocumento] bigint  NULL,
    [FechaRegistro] datetime  NOT NULL,
    [DireccionIP] nvarchar(250)  NOT NULL,
    [Mensaje] nvarchar(450)  NOT NULL,
    [Accion] nvarchar(450)  NOT NULL,
    [IdUsuario] bigint  NULL,
    [Negocios] bit  NOT NULL,
    [DatosModificados] nvarchar(max)  NULL
);
GO

-- Creating table 'tblAuditoriaProcesoCritico'
CREATE TABLE [dbo].[tblAuditoriaProcesoCritico] (
    [IdProceso] bigint  NOT NULL,
    [IdAuditoriaProcesoCritico] bigint IDENTITY(1,1) NOT NULL,
    [FechaActualizacion] datetime  NOT NULL,
    [EstadoAnterior] bit  NOT NULL,
    [EstadoActual] bit  NOT NULL,
    [IdEmpresa] bigint  NOT NULL
);
GO

-- Creating table 'tblBCPDocumento'
CREATE TABLE [dbo].[tblBCPDocumento] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint IDENTITY(1,1) NOT NULL,
    [IdDocumento] bigint  NULL,
    [IdTipoDocumento] bigint  NULL,
    [IdDocumentoBIA] bigint  NULL,
    [IdProceso] bigint  NULL,
    [IdUnidadOrganizativa] bigint  NULL,
    [Proceso] nvarchar(500)  NULL,
    [SubProceso] nvarchar(500)  NULL,
    [Responsable] nvarchar(500)  NULL
);
GO

-- Creating table 'tblBCPReanudacionPersonaClave'
CREATE TABLE [dbo].[tblBCPReanudacionPersonaClave] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdPersona] bigint IDENTITY(1,1) NOT NULL,
    [Actividad] nvarchar(450)  NOT NULL,
    [Nombre] nvarchar(450)  NOT NULL,
    [Cedula] nvarchar(450)  NOT NULL,
    [TelefonoOficina] nvarchar(450)  NOT NULL,
    [TelefonoCelular] nvarchar(450)  NOT NULL,
    [TelefonoHabitacion] nvarchar(450)  NOT NULL,
    [Correo] nvarchar(450)  NOT NULL,
    [DireccionHabitacion] nvarchar(max)  NOT NULL,
    [IdPersonaClavePrincipal] bigint  NULL
);
GO

-- Creating table 'tblBCPReanudacionTarea'
CREATE TABLE [dbo].[tblBCPReanudacionTarea] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdBCPReanudacionTarea] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblBCPReanudacionTareaActividad'
CREATE TABLE [dbo].[tblBCPReanudacionTareaActividad] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdBCPReanudacionTarea] bigint  NOT NULL,
    [IdBCPReanudacionTareaActividad] bigint IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Procesado] bit  NOT NULL
);
GO

-- Creating table 'tblBCPRecuperacionPersonaClave'
CREATE TABLE [dbo].[tblBCPRecuperacionPersonaClave] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdPersona] bigint IDENTITY(1,1) NOT NULL,
    [Actividad] nvarchar(450)  NOT NULL,
    [Nombre] nvarchar(450)  NOT NULL,
    [Cedula] nvarchar(450)  NOT NULL,
    [TelefonoOficina] nvarchar(450)  NOT NULL,
    [TelefonoCelular] nvarchar(450)  NOT NULL,
    [TelefonoHabitacion] nvarchar(450)  NOT NULL,
    [Correo] nvarchar(450)  NOT NULL,
    [DireccionHabitacion] nvarchar(max)  NOT NULL,
    [IdPersonaClavePrincipal] bigint  NULL
);
GO

-- Creating table 'tblBCPRecuperacionRecurso'
CREATE TABLE [dbo].[tblBCPRecuperacionRecurso] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdRecuperacionRecurso] bigint IDENTITY(1,1) NOT NULL,
    [Cantidad] smallint  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblBCPRespuestaAccion'
CREATE TABLE [dbo].[tblBCPRespuestaAccion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdBCPRespuestaAccion] bigint IDENTITY(1,1) NOT NULL,
    [IdPersona] bigint  NOT NULL,
    [NivelEscala] smallint  NOT NULL,
    [NombreEscala] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblBCPRespuestaRecurso'
CREATE TABLE [dbo].[tblBCPRespuestaRecurso] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdRespuestaRecurso] bigint IDENTITY(1,1) NOT NULL,
    [Cantidad] smallint  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblBCPRestauracionEquipo'
CREATE TABLE [dbo].[tblBCPRestauracionEquipo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdBCPRestauracionEquipo] bigint IDENTITY(1,1) NOT NULL,
    [Cantidad] int  NOT NULL,
    [Descripcion] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblBCPRestauracionInfraestructura'
CREATE TABLE [dbo].[tblBCPRestauracionInfraestructura] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdBCPRestauracionInfraestructura] bigint IDENTITY(1,1) NOT NULL,
    [Cantidad] int  NOT NULL,
    [Descripcion] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblBCPRestauracionMobiliario'
CREATE TABLE [dbo].[tblBCPRestauracionMobiliario] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdRestauracionMobiliario] bigint IDENTITY(1,1) NOT NULL,
    [Cantidad] int  NOT NULL,
    [Descripcion] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblBCPRestauracionOtro'
CREATE TABLE [dbo].[tblBCPRestauracionOtro] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBCP] bigint  NOT NULL,
    [IdBCPRestauracionOtro] bigint IDENTITY(1,1) NOT NULL,
    [Cantidad] int  NOT NULL,
    [Descripcion] varchar(450)  NOT NULL
);
GO

-- Creating table 'tblBIAAmenaza'
CREATE TABLE [dbo].[tblBIAAmenaza] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdAmenaza] bigint IDENTITY(1,1) NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Evento] nvarchar(max)  NULL,
    [TipoControlImplantado] nvarchar(max)  NULL,
    [ControlesImplantar] nvarchar(max)  NULL,
    [Probabilidad] smallint  NULL,
    [Impacto] smallint  NULL,
    [Control] smallint  NULL,
    [Severidad] smallint  NULL,
    [Fuente] char(1)  NULL,
    [Estado] smallint  NULL
);
GO

-- Creating table 'tblBIAAmenazaEvento'
CREATE TABLE [dbo].[tblBIAAmenazaEvento] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdAmenaza] bigint  NOT NULL,
    [IdAmenazaEvento] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [IdEventoRiesgo] bigint  NOT NULL
);
GO

-- Creating table 'tblBIAAplicacion'
CREATE TABLE [dbo].[tblBIAAplicacion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdAplicacion] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Usuario] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblBIACadenaServicio'
CREATE TABLE [dbo].[tblBIACadenaServicio] (
    [IdEmpresa] bigint  NOT NULL,
    [IdCadenaServicio] bigint  NOT NULL,
    [Descripcion] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tblBIAClienteProceso'
CREATE TABLE [dbo].[tblBIAClienteProceso] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdClienteProceso] bigint IDENTITY(1,1) NOT NULL,
    [Unidad] nvarchar(max)  NOT NULL,
    [Responsable] nvarchar(max)  NOT NULL,
    [Proceso] nvarchar(max)  NOT NULL,
    [Producto] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblBIAComentario'
CREATE TABLE [dbo].[tblBIAComentario] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBia] bigint  NOT NULL,
    [IdComentario] bigint IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblBIADocumentacion'
CREATE TABLE [dbo].[tblBIADocumentacion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdDocumento] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Ubicacion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblBIADocumento'
CREATE TABLE [dbo].[tblBIADocumento] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint IDENTITY(1,1) NOT NULL,
    [IdDocumento] bigint  NULL,
    [IdUnidadOrganizativa] bigint  NULL,
    [IdCadenaServicio] bigint  NULL,
    [IdTipoDocumento] bigint  NULL
);
GO

-- Creating table 'tblBIAEntrada'
CREATE TABLE [dbo].[tblBIAEntrada] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdEntrada] bigint IDENTITY(1,1) NOT NULL,
    [Unidad] nvarchar(max)  NOT NULL,
    [Evento] nvarchar(max)  NOT NULL,
    [Responsable] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblBIAEventoControl'
CREATE TABLE [dbo].[tblBIAEventoControl] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdAmenaza] bigint  NOT NULL,
    [IdAmenazaEvento] bigint  NOT NULL,
    [IdEventoControl] bigint IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Implantado] bit  NOT NULL
);
GO

-- Creating table 'tblBIAEventoRiesgo'
CREATE TABLE [dbo].[tblBIAEventoRiesgo] (
    [IdEventoRiesgo] bigint IDENTITY(1,1) NOT NULL,
    [IdEmpresa] bigint  NOT NULL,
    [Probabilidad] smallint  NOT NULL,
    [Impacto] smallint  NOT NULL,
    [Control] smallint  NOT NULL,
    [Severidad] bigint  NOT NULL,
    [IdEstadoRiesgo] bigint  NOT NULL,
    [IdFuenteRiesgo] bigint  NOT NULL
);
GO

-- Creating table 'tblBIAGranImpacto'
CREATE TABLE [dbo].[tblBIAGranImpacto] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdGranImpacto] bigint IDENTITY(1,1) NOT NULL,
    [IdMes] int  NOT NULL,
    [Observacion] nvarchar(450)  NOT NULL,
    [Explicacion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblBIAImpactoFinanciero'
CREATE TABLE [dbo].[tblBIAImpactoFinanciero] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdImpactoFinanciero] bigint IDENTITY(1,1) NOT NULL,
    [IdTipoFrecuencia] bigint  NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Impacto] nvarchar(max)  NOT NULL,
    [IdEscala] bigint  NULL,
    [UnidadTiempo] nvarchar(450)  NULL
);
GO

-- Creating table 'tblBIAImpactoOperacional'
CREATE TABLE [dbo].[tblBIAImpactoOperacional] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdImpactoOperacional] bigint IDENTITY(1,1) NOT NULL,
    [IdTipoFrecuencia] bigint  NULL,
    [ImpactoOperacional] nvarchar(450)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [IdEscala] bigint  NULL,
    [UnidadTiempo] nvarchar(450)  NULL
);
GO

-- Creating table 'tblBIAInterdependencia'
CREATE TABLE [dbo].[tblBIAInterdependencia] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdInterdependencia] bigint IDENTITY(1,1) NOT NULL,
    [Organizacion] nvarchar(max)  NOT NULL,
    [Servicio] nvarchar(max)  NOT NULL,
    [Contacto] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblBIAMTD'
CREATE TABLE [dbo].[tblBIAMTD] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdMTD] bigint IDENTITY(1,1) NOT NULL,
    [Observacion] nvarchar(max)  NULL,
    [IdTipoFrecuencia] bigint  NULL,
    [IdEscala] bigint  NULL
);
GO

-- Creating table 'tblBIAPersonaClave'
CREATE TABLE [dbo].[tblBIAPersonaClave] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdPersonaClave] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL
);
GO

-- Creating table 'tblBIAPersonaRespaldoProceso'
CREATE TABLE [dbo].[tblBIAPersonaRespaldoProceso] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdPersona] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL
);
GO

-- Creating table 'tblBIAProceso'
CREATE TABLE [dbo].[tblBIAProceso] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBia] bigint  NOT NULL,
    [IdProceso] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(1500)  NULL,
    [Descripcion] nvarchar(max)  NULL,
    [NroProceso] int  NULL,
    [FechaCreacion] datetime  NULL,
    [IdUnidadOrganizativa] bigint  NULL,
    [Critico] bit  NULL,
    [IdEstadoProceso] bigint  NULL,
    [FechaUltimoEstatus] datetime  NULL
);
GO

-- Creating table 'tblBIAProcesoAlterno'
CREATE TABLE [dbo].[tblBIAProcesoAlterno] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdProcesoAlterno] bigint IDENTITY(1,1) NOT NULL,
    [ProcesoAlterno] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblBIAProveedor'
CREATE TABLE [dbo].[tblBIAProveedor] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdProveedor] bigint IDENTITY(1,1) NOT NULL,
    [Organizacion] nvarchar(450)  NOT NULL,
    [Servicio] nvarchar(450)  NOT NULL,
    [Contacto] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblBIARespaldoPrimario'
CREATE TABLE [dbo].[tblBIARespaldoPrimario] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdRespaldo] bigint IDENTITY(1,1) NOT NULL,
    [Ubicacion] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblBIARespaldoSecundario'
CREATE TABLE [dbo].[tblBIARespaldoSecundario] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdRespaldo] bigint IDENTITY(1,1) NOT NULL,
    [Ubicacion] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblBIARPO'
CREATE TABLE [dbo].[tblBIARPO] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdRPO] bigint IDENTITY(1,1) NOT NULL,
    [Observacion] nvarchar(450)  NOT NULL,
    [IdTipoFrecuencia] bigint  NOT NULL,
    [IdEscala] bigint  NOT NULL
);
GO

-- Creating table 'tblBIARTO'
CREATE TABLE [dbo].[tblBIARTO] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdRTO] bigint IDENTITY(1,1) NOT NULL,
    [Observacion] nvarchar(max)  NOT NULL,
    [IdTipoFrecuencia] bigint  NOT NULL,
    [IdEscala] bigint  NOT NULL
);
GO

-- Creating table 'tblBIAUnidadTrabajo'
CREATE TABLE [dbo].[tblBIAUnidadTrabajo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdUnidadTrabajo] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] varchar(250)  NOT NULL,
    [IdUnidadOrganizativa] bigint  NOT NULL
);
GO

-- Creating table 'tblBIAUnidadTrabajoPersonas'
CREATE TABLE [dbo].[tblBIAUnidadTrabajoPersonas] (
    [IdEmpresa] bigint  NOT NULL,
    [IdUnidadTrabajo] bigint  NOT NULL,
    [IdUnidadTrabajoProceso] bigint  NOT NULL,
    [IdUnidadPersona] bigint IDENTITY(1,1) NOT NULL,
    [IdClienteProceso] bigint  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL
);
GO

-- Creating table 'tblBIAUnidadTrabajoProceso'
CREATE TABLE [dbo].[tblBIAUnidadTrabajoProceso] (
    [IdEmpresa] bigint  NOT NULL,
    [IdUnidadTrabajo] bigint  NOT NULL,
    [IdUnidadTrabajoProceso] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(450)  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL
);
GO

-- Creating table 'tblBIAWRT'
CREATE TABLE [dbo].[tblBIAWRT] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumentoBIA] bigint  NOT NULL,
    [IdProceso] bigint  NOT NULL,
    [IdWRT] bigint IDENTITY(1,1) NOT NULL,
    [Observacion] nvarchar(max)  NULL,
    [IdTipoFrecuencia] bigint  NULL,
    [IdEscala] bigint  NULL
);
GO

-- Creating table 'tblCargo'
CREATE TABLE [dbo].[tblCargo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdCargo] bigint IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblCiudad'
CREATE TABLE [dbo].[tblCiudad] (
    [IdPais] bigint  NOT NULL,
    [IdEstado] bigint  NOT NULL,
    [IdCiudad] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblControlRiesgo'
CREATE TABLE [dbo].[tblControlRiesgo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdControl] smallint  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblCriticidad'
CREATE TABLE [dbo].[tblCriticidad] (
    [FechaAplicacion] datetime  NOT NULL,
    [IdEmpresa] bigint  NOT NULL,
    [IdTipoEscala] bigint  NOT NULL,
    [DescripcionEscala] nvarchar(max)  NULL
);
GO

-- Creating table 'tblCultura_Ciudad'
CREATE TABLE [dbo].[tblCultura_Ciudad] (
    [Culture] nchar(5)  NOT NULL,
    [IdPais] bigint  NOT NULL,
    [IdEstado] bigint  NOT NULL,
    [IdCiudad] bigint  NOT NULL,
    [Nombre] nvarchar(250)  NULL
);
GO

-- Creating table 'tblCultura_Estado'
CREATE TABLE [dbo].[tblCultura_Estado] (
    [Culture] nchar(5)  NOT NULL,
    [IdPais] bigint  NOT NULL,
    [IdEstado] bigint  NOT NULL,
    [Nombre] nvarchar(250)  NULL
);
GO

-- Creating table 'tblCultura_EstadoDocumento'
CREATE TABLE [dbo].[tblCultura_EstadoDocumento] (
    [Culture] nchar(5)  NOT NULL,
    [IdEstadoDocumento] bigint  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_EstadoEmpresa'
CREATE TABLE [dbo].[tblCultura_EstadoEmpresa] (
    [Culture] nchar(5)  NOT NULL,
    [IdEstadoEmpresa] bigint  NOT NULL,
    [Descripcion] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tblCultura_EstadoProceso'
CREATE TABLE [dbo].[tblCultura_EstadoProceso] (
    [Culture] nchar(5)  NOT NULL,
    [IdEstadoProceso] bigint  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_EstadoUsuario'
CREATE TABLE [dbo].[tblCultura_EstadoUsuario] (
    [Culture] nchar(5)  NOT NULL,
    [IdEstadoUsuario] smallint  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_Mes'
CREATE TABLE [dbo].[tblCultura_Mes] (
    [Culture] nchar(5)  NOT NULL,
    [IdMes] int  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_NivelImpacto'
CREATE TABLE [dbo].[tblCultura_NivelImpacto] (
    [Culture] nchar(5)  NOT NULL,
    [IdNivelImpacto] int  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_NivelUsuario'
CREATE TABLE [dbo].[tblCultura_NivelUsuario] (
    [Culture] nchar(5)  NOT NULL,
    [IdNivelUsuario] bigint  NOT NULL,
    [Descripcion] nvarchar(450)  NULL
);
GO

-- Creating table 'tblCultura_Pais'
CREATE TABLE [dbo].[tblCultura_Pais] (
    [Culture] nchar(5)  NOT NULL,
    [IdPais] bigint  NOT NULL,
    [Nombre] nvarchar(250)  NULL
);
GO

-- Creating table 'tblCultura_PBEPruebaEstatus'
CREATE TABLE [dbo].[tblCultura_PBEPruebaEstatus] (
    [Cultura] nchar(5)  NOT NULL,
    [IdEstatus] smallint  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_PlanTrabajoEstatus'
CREATE TABLE [dbo].[tblCultura_PlanTrabajoEstatus] (
    [Culture] nchar(5)  NOT NULL,
    [IdEstatusActividad] smallint  NOT NULL,
    [Descripcion] nvarchar(450)  NULL
);
GO

-- Creating table 'tblCultura_PMTProgramacionTipoActualizacion'
CREATE TABLE [dbo].[tblCultura_PMTProgramacionTipoActualizacion] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoActualizacion] smallint  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_PMTProgramacionTipoNotificacion'
CREATE TABLE [dbo].[tblCultura_PMTProgramacionTipoNotificacion] (
    [Cultura] nchar(5)  NOT NULL,
    [IdTipoNotificacion] smallint  NOT NULL,
    [Descripcion] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tblCultura_TipoCorreo'
CREATE TABLE [dbo].[tblCultura_TipoCorreo] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoCorreo] bigint  NOT NULL,
    [Descripcion] nvarchar(250)  NULL
);
GO

-- Creating table 'tblCultura_TipoDireccion'
CREATE TABLE [dbo].[tblCultura_TipoDireccion] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoDireccion] bigint  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_TipoFrecuencia'
CREATE TABLE [dbo].[tblCultura_TipoFrecuencia] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoFrecuencia] bigint  NOT NULL,
    [Descripcion] nvarchar(450)  NULL
);
GO

-- Creating table 'tblCultura_TipoImpacto'
CREATE TABLE [dbo].[tblCultura_TipoImpacto] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoImpacto] int  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_TipoInterdependencia'
CREATE TABLE [dbo].[tblCultura_TipoInterdependencia] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoInterdependencia] int  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_TipoRespaldo'
CREATE TABLE [dbo].[tblCultura_TipoRespaldo] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoRespaldo] int  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_TipoResultadoPrueba'
CREATE TABLE [dbo].[tblCultura_TipoResultadoPrueba] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoResultadoPrueba] int  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_TipoTablaContenido'
CREATE TABLE [dbo].[tblCultura_TipoTablaContenido] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoTablaContenido] int  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_TipoTelefono'
CREATE TABLE [dbo].[tblCultura_TipoTelefono] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoTelefono] bigint  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCultura_TipoUbicacionInformacion'
CREATE TABLE [dbo].[tblCultura_TipoUbicacionInformacion] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoUbicacionInformacion] int  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblCulture_FuenteIncidente'
CREATE TABLE [dbo].[tblCulture_FuenteIncidente] (
    [Culture] nchar(5)  NOT NULL,
    [IdFuenteIncidente] int  NOT NULL,
    [Descripcion] nvarchar(250)  NULL
);
GO

-- Creating table 'tblCulture_NaturalezaIncidente'
CREATE TABLE [dbo].[tblCulture_NaturalezaIncidente] (
    [Culture] nchar(5)  NOT NULL,
    [IdNaturalezaIncidente] int  NOT NULL,
    [Descripcion] nvarchar(250)  NULL
);
GO

-- Creating table 'tblCulture_TipoIncidente'
CREATE TABLE [dbo].[tblCulture_TipoIncidente] (
    [Culture] nchar(5)  NOT NULL,
    [IdTipoIncidente] int  NOT NULL,
    [Descripcion] nvarchar(250)  NULL
);
GO

-- Creating table 'tblDispositivo'
CREATE TABLE [dbo].[tblDispositivo] (
    [IdDispositivo] bigint IDENTITY(1,1) NOT NULL,
    [fechaRegistro] datetime  NULL,
    [IdUnicoDispositivo] nvarchar(500)  NULL,
    [nombre] nvarchar(500)  NULL,
    [fabricante] nvarchar(500)  NULL,
    [modelo] nvarchar(500)  NULL,
    [plataforma] nvarchar(500)  NULL,
    [version] nvarchar(500)  NULL,
    [tipo] nvarchar(50)  NULL,
    [token] nvarchar(max)  NULL
);
GO

-- Creating table 'tblDispositivoConexion'
CREATE TABLE [dbo].[tblDispositivoConexion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDispositivo] bigint  NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [fechaConexion] datetime  NOT NULL,
    [DireccionIP] nvarchar(50)  NULL
);
GO

-- Creating table 'tblDispositivoEnvio'
CREATE TABLE [dbo].[tblDispositivoEnvio] (
    [IdDispositivo] bigint  NOT NULL,
    [IdSubModulo] bigint  NOT NULL,
    [IdEmpresa] bigint  NULL,
    [IdUsuarioEnvia] bigint  NULL,
    [FechaEnvio] datetime  NULL,
    [FechaDescarga] datetime  NULL,
    [Descargado] bit  NULL
);
GO

-- Creating table 'tblDocumento'
CREATE TABLE [dbo].[tblDocumento] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint IDENTITY(1,1) NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [NroDocumento] bigint  NOT NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaUltimaModificacion] datetime  NULL,
    [IdEstadoDocumento] bigint  NOT NULL,
    [FechaEstadoDocumento] datetime  NOT NULL,
    [Negocios] bit  NOT NULL,
    [NroVersion] int  NOT NULL,
    [VersionOriginal] int  NULL,
    [IdPersonaResponsable] bigint  NOT NULL,
    [RequiereCertificacion] bit  NOT NULL
);
GO

-- Creating table 'tblDocumentoAnexo'
CREATE TABLE [dbo].[tblDocumentoAnexo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [IdAnexo] bigint IDENTITY(1,1) NOT NULL,
    [ParentId] bigint  NULL,
    [Nombre] nvarchar(500)  NULL,
    [IsFolder] bit  NULL,
    [Data] varbinary(max)  NULL,
    [LastWriteTime] datetime  NULL
);
GO

-- Creating table 'tblDocumentoAprobacion'
CREATE TABLE [dbo].[tblDocumentoAprobacion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [IdAprobacion] bigint IDENTITY(1,1) NOT NULL,
    [Fecha] datetime  NULL,
    [IdPersona] bigint  NULL,
    [Procesado] bit  NOT NULL,
    [Aprobado] bit  NULL
);
GO

-- Creating table 'tblDocumentoCertificacion'
CREATE TABLE [dbo].[tblDocumentoCertificacion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [IdCertificacion] bigint IDENTITY(1,1) NOT NULL,
    [Fecha] datetime  NULL,
    [IdPersona] bigint  NULL,
    [Procesado] bit  NOT NULL,
    [Certificado] bit  NULL
);
GO

-- Creating table 'tblDocumentoContenido'
CREATE TABLE [dbo].[tblDocumentoContenido] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [IdSubModulo] bigint  NOT NULL,
    [ContenidoBin] varbinary(max)  NULL,
    [FechaCreacion] datetime  NULL
);
GO

-- Creating table 'tblDocumentoEntrevista'
CREATE TABLE [dbo].[tblDocumentoEntrevista] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [IdEntrevista] bigint IDENTITY(1,1) NOT NULL,
    [FechaInicio] datetime  NOT NULL,
    [FechaFinal] datetime  NOT NULL
);
GO

-- Creating table 'tblDocumentoEntrevistaPersona'
CREATE TABLE [dbo].[tblDocumentoEntrevistaPersona] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [IdEntrevista] bigint  NOT NULL,
    [IdPersonaEntrevista] bigint  NOT NULL,
    [EsEntrevistador] bit  NOT NULL,
    [Nombre] nvarchar(1500)  NULL,
    [Empresa] nvarchar(1500)  NULL
);
GO

-- Creating table 'tblDocumentoPersonaClave'
CREATE TABLE [dbo].[tblDocumentoPersonaClave] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [IdPersona] bigint  NOT NULL,
    [Nombre] nvarchar(450)  NOT NULL,
    [Cedula] nvarchar(450)  NOT NULL,
    [TelefonoOficina] nvarchar(450)  NOT NULL,
    [TelefonoCelular] nvarchar(450)  NOT NULL,
    [TelefonoHabitacion] nvarchar(450)  NOT NULL,
    [Correo] nvarchar(450)  NOT NULL,
    [DireccionHabitacion] nvarchar(max)  NOT NULL,
    [Principal] bit  NULL
);
GO

-- Creating table 'tblEmpresa'
CREATE TABLE [dbo].[tblEmpresa] (
    [IdEmpresa] bigint IDENTITY(1,1) NOT NULL,
    [NombreFiscal] nvarchar(250)  NOT NULL,
    [RegistroFiscal] nvarchar(50)  NOT NULL,
    [NombreComercial] nvarchar(250)  NOT NULL,
    [DireccionAdministrativa] nvarchar(max)  NOT NULL,
    [IdEstado] bigint  NOT NULL,
    [FechaUltimoEstado] datetime  NOT NULL,
    [LogoURL] nvarchar(250)  NOT NULL,
    [FechaInicioActividad] datetime  NOT NULL,
    [CrearProcesos] bit  NULL,
    [CrearUnidadOrganizativa] bit  NULL,
    [CrearUnidadTrabajo] bit  NULL,
    [CrearAplicaciones] bit  NULL,
    [CrearDocumento] bit  NULL,
    [IdPais] bigint  NULL,
    [IdPaisEstado] bigint  NULL,
    [IdPaisEstadoCiudad] bigint  NULL
);
GO

-- Creating table 'tblEmpresaModulo'
CREATE TABLE [dbo].[tblEmpresaModulo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [FechaCreacion] datetime  NULL
);
GO

-- Creating table 'tblEmpresaUsuario'
CREATE TABLE [dbo].[tblEmpresaUsuario] (
    [IdEmpresa] bigint  NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [FechaCreacion] datetime  NULL,
    [IdNivelUsuario] bigint  NOT NULL
);
GO

-- Creating table 'tblEscala'
CREATE TABLE [dbo].[tblEscala] (
    [IdEmpresa] bigint  NOT NULL,
    [IdEscala] bigint IDENTITY(1,1) NOT NULL,
    [IdTipoEscala] bigint  NOT NULL,
    [Valor] smallint  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [FechaRegistro] datetime  NULL,
    [PosicionEscala] int  NULL
);
GO

-- Creating table 'tblEstado'
CREATE TABLE [dbo].[tblEstado] (
    [IdPais] bigint  NOT NULL,
    [IdEstado] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblEstadoDocumento'
CREATE TABLE [dbo].[tblEstadoDocumento] (
    [IdEstadoDocumento] bigint  NOT NULL
);
GO

-- Creating table 'tblEstadoEmpresa'
CREATE TABLE [dbo].[tblEstadoEmpresa] (
    [IdEstadoEmpresa] bigint  NOT NULL
);
GO

-- Creating table 'tblEstadoProceso'
CREATE TABLE [dbo].[tblEstadoProceso] (
    [IdEstadoProceso] bigint  NOT NULL
);
GO

-- Creating table 'tblEstadoRiesgo'
CREATE TABLE [dbo].[tblEstadoRiesgo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdEstadoRiesgo] smallint  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [Color] varchar(50)  NOT NULL
);
GO

-- Creating table 'tblEstadoUsuario'
CREATE TABLE [dbo].[tblEstadoUsuario] (
    [IdEstadoUsuario] smallint  NOT NULL
);
GO

-- Creating table 'tblFormatosEmail'
CREATE TABLE [dbo].[tblFormatosEmail] (
    [IdEmpresa] bigint  NOT NULL,
    [IdCodigoModulo] bigint  NOT NULL,
    [IdCorreo] int  NOT NULL,
    [Descripcion] nvarchar(1500)  NOT NULL,
    [Subject] nvarchar(450)  NULL,
    [EmailBody] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tblFuenteRiesgo'
CREATE TABLE [dbo].[tblFuenteRiesgo] (
    [IdEmpresa] bigint  NOT NULL,
    [CodigoFuente] char(1)  NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tblImpactoRiesgo'
CREATE TABLE [dbo].[tblImpactoRiesgo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdImpacto] smallint  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblIniciativaPrioridad'
CREATE TABLE [dbo].[tblIniciativaPrioridad] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPrioridad] smallint  NOT NULL,
    [Nombre] nvarchar(50)  NULL
);
GO

-- Creating table 'tblIniciativaResponsable'
CREATE TABLE [dbo].[tblIniciativaResponsable] (
    [IdEmpresa] bigint  NOT NULL,
    [IdResponsable] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(150)  NULL
);
GO

-- Creating table 'tblIniciativas'
CREATE TABLE [dbo].[tblIniciativas] (
    [IdEmpresa] bigint  NOT NULL,
    [IdIniciativa] bigint IDENTITY(1,1) NOT NULL,
    [NroIniciativa] int  NULL,
    [Nombre] nvarchar(1500)  NULL,
    [Descripcion] nvarchar(max)  NULL,
    [IdUnidadOrganizativa] bigint  NULL,
    [UnidadOrganizativa] nvarchar(1500)  NULL,
    [NombreResponsable] nvarchar(1500)  NULL,
    [FechaInicioEstimada] datetime  NULL,
    [FechaInicioReal] datetime  NULL,
    [FechaCierreEstimada] datetime  NULL,
    [FechaCierreReal] datetime  NULL,
    [PresupuestoEstimado] decimal(19,4)  NULL,
    [PresupuestoReal] decimal(19,4)  NULL,
    [IdEstatusIniciativa] smallint  NULL,
    [Observacion] nvarchar(max)  NULL,
    [IdPrioridad] smallint  NULL,
    [MontoAbonado] decimal(19,4)  NULL,
    [MontoPendiente] decimal(19,4)  NULL,
    [PorcentajeAvance] decimal(5,2)  NULL,
    [HorasEstimadas] int  NULL,
    [HorasConsumidas] int  NULL
);
GO

-- Creating table 'tblIniciativas_Anexo'
CREATE TABLE [dbo].[tblIniciativas_Anexo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdIniciativa] bigint  NOT NULL,
    [IdAnexo] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(1500)  NULL,
    [RutaArchivo] nvarchar(1500)  NULL,
    [fechaRegistro] datetime  NULL
);
GO

-- Creating table 'tblLocalidad'
CREATE TABLE [dbo].[tblLocalidad] (
    [IdEmpresa] bigint  NOT NULL,
    [IdLocalidad] bigint IDENTITY(1,1) NOT NULL,
    [Codigo] nvarchar(50)  NOT NULL,
    [Nombre] nvarchar(450)  NOT NULL,
    [IdPais] bigint  NOT NULL,
    [IdEstado] bigint  NOT NULL,
    [IdCiudad] bigint  NOT NULL
);
GO

-- Creating table 'tblMes'
CREATE TABLE [dbo].[tblMes] (
    [IdMes] int  NOT NULL
);
GO

-- Creating table 'tblModulo'
CREATE TABLE [dbo].[tblModulo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [IdCodigoModulo] int  NULL,
    [IdModuloPadre] bigint  NOT NULL,
    [IdTipoElemento] smallint  NOT NULL,
    [Nombre] nvarchar(250)  NULL,
    [Accion] varchar(50)  NULL,
    [Controller] varchar(50)  NULL,
    [Titulo] nvarchar(450)  NULL,
    [Descripcion] nvarchar(4000)  NULL,
    [imageRoot] nvarchar(500)  NULL,
    [Activo] bit  NOT NULL,
    [Negocios] bit  NOT NULL,
    [Tecnologia] bit  NOT NULL
);
GO

-- Creating table 'tblModulo_NivelUsuario'
CREATE TABLE [dbo].[tblModulo_NivelUsuario] (
    [IdEmpresa] bigint  NOT NULL,
    [IdNivelUsuario] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [Actualizar] bit  NOT NULL,
    [Eliminar] bit  NOT NULL
);
GO

-- Creating table 'tblModulo_Usuario'
CREATE TABLE [dbo].[tblModulo_Usuario] (
    [IdEmpresa] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [Actualizar] bit  NOT NULL,
    [Eliminar] bit  NOT NULL
);
GO

-- Creating table 'tblModuloAnexo'
CREATE TABLE [dbo].[tblModuloAnexo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [Negocios] bit  NOT NULL,
    [IdAnexo] bigint IDENTITY(1,1) NOT NULL,
    [ParentId] bigint  NULL,
    [Nombre] nvarchar(500)  NULL,
    [IsFolder] bit  NULL,
    [Data] varbinary(max)  NULL,
    [LastWriteTime] datetime  NULL
);
GO

-- Creating table 'tblNivelImpacto'
CREATE TABLE [dbo].[tblNivelImpacto] (
    [IdNivelImpacto] int  NOT NULL
);
GO

-- Creating table 'tblNivelUsuario'
CREATE TABLE [dbo].[tblNivelUsuario] (
    [IdNivelUsuario] bigint  NOT NULL,
    [TodosDocs] bit  NOT NULL,
    [RolUsuario] varchar(50)  NULL
);
GO

-- Creating table 'tblPais'
CREATE TABLE [dbo].[tblPais] (
    [IdPais] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblPBEPruebaEjecucion'
CREATE TABLE [dbo].[tblPBEPruebaEjecucion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [FechaInicio] datetime  NOT NULL,
    [Lugar] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaEjecucionEjercicio'
CREATE TABLE [dbo].[tblPBEPruebaEjecucionEjercicio] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdEjercicio] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(500)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [FechaInicio] datetime  NOT NULL,
    [DuracionHoras] int  NOT NULL,
    [DuracionMinutos] int  NOT NULL,
    [IdEstatus] smallint  NULL,
    [IdEjercicioPlanificacion] bigint  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaEjecucionEjercicioParticipante'
CREATE TABLE [dbo].[tblPBEPruebaEjecucionEjercicioParticipante] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdEjercicio] bigint  NOT NULL,
    [IdParticipante] bigint  NOT NULL,
    [Responsable] bit  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaEjecucionEjercicioRecurso'
CREATE TABLE [dbo].[tblPBEPruebaEjecucionEjercicioRecurso] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdEjercicio] bigint  NOT NULL,
    [IdRecurso] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Cantidad] int  NOT NULL,
    [Responsable] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaEjecucionParticipante'
CREATE TABLE [dbo].[tblPBEPruebaEjecucionParticipante] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdParticipante] bigint IDENTITY(1,1) NOT NULL,
    [Empresa] nvarchar(500)  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [Responsable] bit  NOT NULL,
    [Correo] nvarchar(500)  NOT NULL,
    [Telefono] nvarchar(50)  NOT NULL,
    [IdPlanificacionParticipante] bigint  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaEjecucionResultado'
CREATE TABLE [dbo].[tblPBEPruebaEjecucionResultado] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdContenido] bigint  NOT NULL,
    [Contenido] varbinary(max)  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaEstatus'
CREATE TABLE [dbo].[tblPBEPruebaEstatus] (
    [IdEstatus] smallint  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaPlanificacion'
CREATE TABLE [dbo].[tblPBEPruebaPlanificacion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint IDENTITY(1,1) NOT NULL,
    [Negocios] bit  NOT NULL,
    [Prueba] nvarchar(500)  NOT NULL,
    [Propósito] nvarchar(max)  NOT NULL,
    [FechaInicio] datetime  NOT NULL,
    [Lugar] nvarchar(500)  NOT NULL,
    [IdEstatus] smallint  NULL
);
GO

-- Creating table 'tblPBEPruebaPlanificacionEjercicio'
CREATE TABLE [dbo].[tblPBEPruebaPlanificacionEjercicio] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdEjercicio] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(500)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [FechaInicio] datetime  NOT NULL,
    [DuracionHoras] int  NOT NULL,
    [DuracionMinutos] int  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaPlanificacionEjercicioParticipante'
CREATE TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioParticipante] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdEjercicio] bigint  NOT NULL,
    [IdParticipante] bigint  NOT NULL,
    [Responsable] bit  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaPlanificacionEjercicioRecurso'
CREATE TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioRecurso] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdEjercicio] bigint  NOT NULL,
    [IdRecurso] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [Descripcion] nvarchar(450)  NOT NULL,
    [Cantidad] int  NOT NULL,
    [Responsable] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblPBEPruebaPlanificacionParticipante'
CREATE TABLE [dbo].[tblPBEPruebaPlanificacionParticipante] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanificacion] bigint  NOT NULL,
    [IdParticipante] bigint IDENTITY(1,1) NOT NULL,
    [Empresa] nvarchar(500)  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [Responsable] bit  NOT NULL,
    [Correo] nvarchar(500)  NOT NULL,
    [Telefono] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tblPersona'
CREATE TABLE [dbo].[tblPersona] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPersona] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [Identificacion] nvarchar(50)  NULL,
    [IdUnidadOrganizativa] bigint  NULL,
    [IdCargo] bigint  NULL,
    [IdUsuario] bigint  NULL
);
GO

-- Creating table 'tblPersonaCorreo'
CREATE TABLE [dbo].[tblPersonaCorreo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPersona] bigint  NOT NULL,
    [IdPersonaCorreo] bigint IDENTITY(1,1) NOT NULL,
    [Correo] nvarchar(450)  NOT NULL,
    [IdTipoCorreo] bigint  NOT NULL
);
GO

-- Creating table 'tblPersonaDireccion'
CREATE TABLE [dbo].[tblPersonaDireccion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPersona] bigint  NOT NULL,
    [IdPersonaDireccion] bigint IDENTITY(1,1) NOT NULL,
    [IdTipoDireccion] bigint  NOT NULL,
    [Ubicacion] nvarchar(1500)  NULL,
    [IdPais] bigint  NOT NULL,
    [IdEstado] bigint  NOT NULL,
    [IdCiudad] bigint  NOT NULL
);
GO

-- Creating table 'tblPersonaTelefono'
CREATE TABLE [dbo].[tblPersonaTelefono] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPersona] bigint  NOT NULL,
    [IdPersonaTelefono] bigint IDENTITY(1,1) NOT NULL,
    [IdTipoTelefono] bigint  NOT NULL,
    [Telefono] varchar(50)  NOT NULL
);
GO

-- Creating table 'tblPlanTrabajo'
CREATE TABLE [dbo].[tblPlanTrabajo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdAccion] bigint  NOT NULL,
    [IdActividad] bigint IDENTITY(1,1) NOT NULL,
    [Recomendacion] nvarchar(450)  NOT NULL,
    [Responsable] nvarchar(450)  NOT NULL,
    [FechaPropuestaEjecucion] datetime  NOT NULL,
    [Ejecutada] smallint  NOT NULL,
    [IdActividadPadre] bigint  NULL
);
GO

-- Creating table 'tblPlanTrabajoAccion'
CREATE TABLE [dbo].[tblPlanTrabajoAccion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdPlanAccion] bigint IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblPlanTrabajoAuditoria'
CREATE TABLE [dbo].[tblPlanTrabajoAuditoria] (
    [IdEmpresa] bigint  NOT NULL,
    [IdAccion] bigint  NOT NULL,
    [IdActividad] bigint  NOT NULL,
    [IdCambioEstado] bigint IDENTITY(1,1) NOT NULL,
    [FechaCambioEstado] datetime  NOT NULL,
    [Estado] smallint  NOT NULL,
    [IdUsuarioCambioEstado] bigint  NOT NULL
);
GO

-- Creating table 'tblPlanTrabajoEstatus'
CREATE TABLE [dbo].[tblPlanTrabajoEstatus] (
    [IdEstatusActividad] smallint  NOT NULL
);
GO

-- Creating table 'tblPMTMensajeActualizacion'
CREATE TABLE [dbo].[tblPMTMensajeActualizacion] (
    [IdEmpresa] bigint  NOT NULL,
    [IdMensaje] bigint IDENTITY(1,1) NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [FechaActualizacion] datetime  NOT NULL,
    [FechaUltimoEnvio] datetime  NOT NULL
);
GO

-- Creating table 'tblPMTProgramacion'
CREATE TABLE [dbo].[tblPMTProgramacion] (
    [IdPMTProgramacion] bigint IDENTITY(1,1) NOT NULL,
    [IdEmpresa] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [FechaInicio] datetime  NOT NULL,
    [FechaFinal] datetime  NOT NULL,
    [IdEstado] bigint  NOT NULL,
    [Negocios] bit  NOT NULL,
    [IdTipoActualizacion] smallint  NOT NULL,
    [IdTipoFrecuencia] bigint  NOT NULL
);
GO

-- Creating table 'tblPMTProgramacionDocumentos'
CREATE TABLE [dbo].[tblPMTProgramacionDocumentos] (
    [IdPMTProgramacion] bigint  NOT NULL,
    [IdEmpresa] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL
);
GO

-- Creating table 'tblPMTProgramacionTipoActualizacion'
CREATE TABLE [dbo].[tblPMTProgramacionTipoActualizacion] (
    [IdTipoActualizacion] smallint  NOT NULL
);
GO

-- Creating table 'tblPMTProgramacionTipoNotificacion'
CREATE TABLE [dbo].[tblPMTProgramacionTipoNotificacion] (
    [IdTipoNotificacion] smallint  NOT NULL
);
GO

-- Creating table 'tblPMTProgramacionUsuario'
CREATE TABLE [dbo].[tblPMTProgramacionUsuario] (
    [IdPMTProgramacion] bigint  NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [IdTipoNotificacion] smallint  NULL,
    [FechaUltimaNotificacion] datetime  NULL
);
GO

-- Creating table 'tblPMTResponsableUpdate'
CREATE TABLE [dbo].[tblPMTResponsableUpdate] (
    [IdEmpresa] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [IdMensaje] bigint IDENTITY(1,1) NOT NULL,
    [IdDocumento] bigint  NULL,
    [IdTipoNotificacion] smallint  NULL
);
GO

-- Creating table 'tblPMTResponsableUpdate_Correo'
CREATE TABLE [dbo].[tblPMTResponsableUpdate_Correo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdModulo] bigint  NOT NULL,
    [IdMensaje] bigint  NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [FechaRegistro] datetime  NULL
);
GO

-- Creating table 'tblPPEFrecuencia'
CREATE TABLE [dbo].[tblPPEFrecuencia] (
    [IdEmpresa] bigint  NOT NULL,
    [IdDocumento] bigint  NOT NULL,
    [IdTipoDocumento] bigint  NOT NULL,
    [IdPPEFrecuencia] bigint IDENTITY(1,1) NOT NULL,
    [TipoPrueba] nvarchar(450)  NOT NULL,
    [Participantes] nvarchar(450)  NOT NULL,
    [Proposito] nvarchar(450)  NOT NULL,
    [IdTipoFrecuencia] bigint  NOT NULL
);
GO

-- Creating table 'tblProbabilidadRiesgo'
CREATE TABLE [dbo].[tblProbabilidadRiesgo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdProbabilidad] smallint  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [EtiquetaGrafico] nvarchar(250)  NULL
);
GO

-- Creating table 'tblProducto'
CREATE TABLE [dbo].[tblProducto] (
    [IdEmpresa] bigint  NOT NULL,
    [IdProducto] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'tblProveedor'
CREATE TABLE [dbo].[tblProveedor] (
    [IdEmpresa] bigint  NOT NULL,
    [IdProveedor] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'tblSeveridadRiesgo'
CREATE TABLE [dbo].[tblSeveridadRiesgo] (
    [IdEmpresa] bigint  NOT NULL,
    [IdSeveridad] smallint  NOT NULL,
    [Nombre] nvarchar(50)  NULL
);
GO

-- Creating table 'tblTipoCorreo'
CREATE TABLE [dbo].[tblTipoCorreo] (
    [IdTipoCorreo] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblTipoDireccion'
CREATE TABLE [dbo].[tblTipoDireccion] (
    [IdTipoDireccion] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblTipoEscala'
CREATE TABLE [dbo].[tblTipoEscala] (
    [IdEmpresa] bigint  NOT NULL,
    [IdTipoEscala] bigint  NOT NULL,
    [Descripcion] nvarchar(50)  NULL
);
GO

-- Creating table 'tblTipoFrecuencia'
CREATE TABLE [dbo].[tblTipoFrecuencia] (
    [IdTipoFrecuencia] bigint  NOT NULL
);
GO

-- Creating table 'tblTipoImpacto'
CREATE TABLE [dbo].[tblTipoImpacto] (
    [IdTipoImpacto] int  NOT NULL
);
GO

-- Creating table 'tblTipoInterdependencia'
CREATE TABLE [dbo].[tblTipoInterdependencia] (
    [IdTipoInterdependencia] int  NOT NULL
);
GO

-- Creating table 'tblTipoRespaldo'
CREATE TABLE [dbo].[tblTipoRespaldo] (
    [IdTipoRespaldo] int  NOT NULL
);
GO

-- Creating table 'tblTipoResultadoPrueba'
CREATE TABLE [dbo].[tblTipoResultadoPrueba] (
    [IdTipoResultadoPrueba] int  NOT NULL
);
GO

-- Creating table 'tblTipoTablaContenido'
CREATE TABLE [dbo].[tblTipoTablaContenido] (
    [IdTipoTablaContenido] int  NOT NULL
);
GO

-- Creating table 'tblTipoTelefono'
CREATE TABLE [dbo].[tblTipoTelefono] (
    [IdTipoTelefono] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblTipoUbicacionInformacion'
CREATE TABLE [dbo].[tblTipoUbicacionInformacion] (
    [IdTipoUbicacionInformacion] int  NOT NULL
);
GO

-- Creating table 'tblUnidadOrganizativa'
CREATE TABLE [dbo].[tblUnidadOrganizativa] (
    [IdEmpresa] bigint  NOT NULL,
    [IdUnidadOrganizativa] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(450)  NOT NULL,
    [IdUnidadPadre] bigint  NOT NULL
);
GO

-- Creating table 'tblUsuario'
CREATE TABLE [dbo].[tblUsuario] (
    [IdUsuario] bigint IDENTITY(1,1) NOT NULL,
    [CodigoUsuario] nvarchar(50)  NOT NULL,
    [ClaveUsuario] nvarchar(50)  NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [EstadoUsuario] smallint  NOT NULL,
    [FechaEstado] datetime  NULL,
    [FechaUltimaConexion] datetime  NULL,
    [PrimeraVez] bit  NULL,
    [Email] nvarchar(250)  NULL
);
GO

-- Creating table 'tblUsuarioUnidadOrganizativa'
CREATE TABLE [dbo].[tblUsuarioUnidadOrganizativa] (
    [IdEmpresa] bigint  NOT NULL,
    [IdUnidadOrganizativa] bigint  NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [IdNivelUsuario] bigint  NOT NULL
);
GO

-- Creating table 'tblVicepresidencia'
CREATE TABLE [dbo].[tblVicepresidencia] (
    [IdEmpresa] bigint  NOT NULL,
    [IdVicepresidencia] bigint IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(250)  NOT NULL,
    [CalleAvenida] nvarchar(450)  NOT NULL,
    [EdificioCasa] nvarchar(250)  NOT NULL,
    [PisoNivel] nvarchar(50)  NOT NULL,
    [TorreAla] nvarchar(250)  NOT NULL,
    [Urbanizacion] nvarchar(250)  NOT NULL,
    [IdCiudad] bigint  NOT NULL,
    [IdEstado] bigint  NOT NULL,
    [IdPais] bigint  NOT NULL
);
GO

-- Creating table 'tblDispositivo1'
CREATE TABLE [dbo].[tblDispositivo1] (
    [IdDispositivo] bigint IDENTITY(1,1) NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [FechaRegistro] datetime  NULL,
    [IMEI_Dispositivo] nvarchar(100)  NULL,
    [NombreDispositivo] nvarchar(500)  NULL,
    [Fabricante] nvarchar(500)  NULL,
    [Modelo] nvarchar(500)  NULL,
    [Plataforma] nvarchar(500)  NULL,
    [Version] nvarchar(500)  NULL,
    [TipoDispositivo] nvarchar(500)  NULL,
    [TokenDispositivo] nvarchar(max)  NULL
);
GO

-- Creating table 'tblDispositivoEnvio1'
CREATE TABLE [dbo].[tblDispositivoEnvio1] (
    [IdDispositivo] bigint  NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [IdEmpresa] bigint  NOT NULL,
    [IdSubModulo] bigint  NOT NULL,
    [IdUsuarioEnvia] bigint  NOT NULL,
    [FechaEnvio] datetime  NOT NULL,
    [FechaDescarga] datetime  NULL,
    [Descargado] bit  NULL
);
GO

-- Creating table 'tblFuenteIncidente'
CREATE TABLE [dbo].[tblFuenteIncidente] (
    [IdFuenteIncidente] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblIncidentes'
CREATE TABLE [dbo].[tblIncidentes] (
    [IdEmpresa] bigint  NOT NULL,
    [IdIncidente] bigint IDENTITY(1,1) NOT NULL,
    [IdTipoIncidente] int  NULL,
    [IdNaturalezaIncidente] int  NULL,
    [IdFuenteIncidente] int  NULL,
    [FechaIncidente] datetime  NULL,
    [Descripcion] nvarchar(max)  NULL,
    [LugarIncidente] nvarchar(max)  NULL,
    [Duracion] int  NULL,
    [NombreReportero] nvarchar(500)  NULL,
    [NombreSolucionador] nvarchar(max)  NULL,
    [Observaciones] nvarchar(max)  NULL
);
GO

-- Creating table 'tblNaturalezaIncidente'
CREATE TABLE [dbo].[tblNaturalezaIncidente] (
    [IdNaturalezaIncidente] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblTipoIncidente'
CREATE TABLE [dbo].[tblTipoIncidente] (
    [IdTipoIncidente] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tblDispositivoConexion1'
CREATE TABLE [dbo].[tblDispositivoConexion1] (
    [IdDispositivo] bigint  NOT NULL,
    [IdUsuario] bigint  NOT NULL,
    [FechaConexion] datetime  NOT NULL,
    [IdEmpresa] bigint  NULL,
    [IdSubModulo] bigint  NULL,
    [DirecciónIP] nvarchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdAuditoria] in table 'tblAuditoria'
ALTER TABLE [dbo].[tblAuditoria]
ADD CONSTRAINT [PK_tblAuditoria]
    PRIMARY KEY CLUSTERED ([IdAuditoria] ASC);
GO

-- Creating primary key on [IdProceso], [IdAuditoriaProcesoCritico] in table 'tblAuditoriaProcesoCritico'
ALTER TABLE [dbo].[tblAuditoriaProcesoCritico]
ADD CONSTRAINT [PK_tblAuditoriaProcesoCritico]
    PRIMARY KEY CLUSTERED ([IdProceso], [IdAuditoriaProcesoCritico] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPDocumento'
ALTER TABLE [dbo].[tblBCPDocumento]
ADD CONSTRAINT [PK_tblBCPDocumento]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdPersona] in table 'tblBCPReanudacionPersonaClave'
ALTER TABLE [dbo].[tblBCPReanudacionPersonaClave]
ADD CONSTRAINT [PK_tblBCPReanudacionPersonaClave]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdPersona] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdBCPReanudacionTarea] in table 'tblBCPReanudacionTarea'
ALTER TABLE [dbo].[tblBCPReanudacionTarea]
ADD CONSTRAINT [PK_tblBCPReanudacionTarea]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdBCPReanudacionTarea] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdBCPReanudacionTarea], [IdBCPReanudacionTareaActividad] in table 'tblBCPReanudacionTareaActividad'
ALTER TABLE [dbo].[tblBCPReanudacionTareaActividad]
ADD CONSTRAINT [PK_tblBCPReanudacionTareaActividad]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdBCPReanudacionTarea], [IdBCPReanudacionTareaActividad] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdPersona] in table 'tblBCPRecuperacionPersonaClave'
ALTER TABLE [dbo].[tblBCPRecuperacionPersonaClave]
ADD CONSTRAINT [PK_tblBCPRecuperacionPersonaClave]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdPersona] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdRecuperacionRecurso] in table 'tblBCPRecuperacionRecurso'
ALTER TABLE [dbo].[tblBCPRecuperacionRecurso]
ADD CONSTRAINT [PK_tblBCPRecuperacionRecurso]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdRecuperacionRecurso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdBCPRespuestaAccion] in table 'tblBCPRespuestaAccion'
ALTER TABLE [dbo].[tblBCPRespuestaAccion]
ADD CONSTRAINT [PK_tblBCPRespuestaAccion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdBCPRespuestaAccion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdRespuestaRecurso] in table 'tblBCPRespuestaRecurso'
ALTER TABLE [dbo].[tblBCPRespuestaRecurso]
ADD CONSTRAINT [PK_tblBCPRespuestaRecurso]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdRespuestaRecurso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdBCPRestauracionEquipo] in table 'tblBCPRestauracionEquipo'
ALTER TABLE [dbo].[tblBCPRestauracionEquipo]
ADD CONSTRAINT [PK_tblBCPRestauracionEquipo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdBCPRestauracionEquipo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdBCPRestauracionInfraestructura] in table 'tblBCPRestauracionInfraestructura'
ALTER TABLE [dbo].[tblBCPRestauracionInfraestructura]
ADD CONSTRAINT [PK_tblBCPRestauracionInfraestructura]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdBCPRestauracionInfraestructura] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdRestauracionMobiliario] in table 'tblBCPRestauracionMobiliario'
ALTER TABLE [dbo].[tblBCPRestauracionMobiliario]
ADD CONSTRAINT [PK_tblBCPRestauracionMobiliario]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdRestauracionMobiliario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBCP], [IdBCPRestauracionOtro] in table 'tblBCPRestauracionOtro'
ALTER TABLE [dbo].[tblBCPRestauracionOtro]
ADD CONSTRAINT [PK_tblBCPRestauracionOtro]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBCP], [IdBCPRestauracionOtro] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdProceso], [IdAmenaza] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [PK_tblBIAAmenaza]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdProceso], [IdAmenaza] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAmenaza], [IdAmenazaEvento] in table 'tblBIAAmenazaEvento'
ALTER TABLE [dbo].[tblBIAAmenazaEvento]
ADD CONSTRAINT [PK_tblBIAAmenazaEvento]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAmenaza], [IdAmenazaEvento] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAplicacion] in table 'tblBIAAplicacion'
ALTER TABLE [dbo].[tblBIAAplicacion]
ADD CONSTRAINT [PK_tblBIAAplicacion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAplicacion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdCadenaServicio] in table 'tblBIACadenaServicio'
ALTER TABLE [dbo].[tblBIACadenaServicio]
ADD CONSTRAINT [PK_tblBIACadenaServicio]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdCadenaServicio] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdClienteProceso] in table 'tblBIAClienteProceso'
ALTER TABLE [dbo].[tblBIAClienteProceso]
ADD CONSTRAINT [PK_tblBIAClienteProceso]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdClienteProceso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBia], [IdComentario] in table 'tblBIAComentario'
ALTER TABLE [dbo].[tblBIAComentario]
ADD CONSTRAINT [PK_tblBIAComentario]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBia], [IdComentario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdDocumento] in table 'tblBIADocumentacion'
ALTER TABLE [dbo].[tblBIADocumentacion]
ADD CONSTRAINT [PK_tblBIADocumentacion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdDocumento] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA] in table 'tblBIADocumento'
ALTER TABLE [dbo].[tblBIADocumento]
ADD CONSTRAINT [PK_tblBIADocumento]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdEntrada] in table 'tblBIAEntrada'
ALTER TABLE [dbo].[tblBIAEntrada]
ADD CONSTRAINT [PK_tblBIAEntrada]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdEntrada] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAmenaza], [IdAmenazaEvento], [IdEventoControl] in table 'tblBIAEventoControl'
ALTER TABLE [dbo].[tblBIAEventoControl]
ADD CONSTRAINT [PK_tblBIAEventoControl]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAmenaza], [IdAmenazaEvento], [IdEventoControl] ASC);
GO

-- Creating primary key on [IdEventoRiesgo], [IdEmpresa] in table 'tblBIAEventoRiesgo'
ALTER TABLE [dbo].[tblBIAEventoRiesgo]
ADD CONSTRAINT [PK_tblBIAEventoRiesgo]
    PRIMARY KEY CLUSTERED ([IdEventoRiesgo], [IdEmpresa] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdGranImpacto], [IdMes] in table 'tblBIAGranImpacto'
ALTER TABLE [dbo].[tblBIAGranImpacto]
ADD CONSTRAINT [PK_tblBIAGranImpacto]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdGranImpacto], [IdMes] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdImpactoFinanciero] in table 'tblBIAImpactoFinanciero'
ALTER TABLE [dbo].[tblBIAImpactoFinanciero]
ADD CONSTRAINT [PK_tblBIAImpactoFinanciero]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdImpactoFinanciero] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdImpactoOperacional] in table 'tblBIAImpactoOperacional'
ALTER TABLE [dbo].[tblBIAImpactoOperacional]
ADD CONSTRAINT [PK_tblBIAImpactoOperacional]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdImpactoOperacional] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdInterdependencia] in table 'tblBIAInterdependencia'
ALTER TABLE [dbo].[tblBIAInterdependencia]
ADD CONSTRAINT [PK_tblBIAInterdependencia]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdInterdependencia] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdMTD] in table 'tblBIAMTD'
ALTER TABLE [dbo].[tblBIAMTD]
ADD CONSTRAINT [PK_tblBIAMTD]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdMTD] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdPersonaClave] in table 'tblBIAPersonaClave'
ALTER TABLE [dbo].[tblBIAPersonaClave]
ADD CONSTRAINT [PK_tblBIAPersonaClave]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdPersonaClave] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdPersona], [IdProceso] in table 'tblBIAPersonaRespaldoProceso'
ALTER TABLE [dbo].[tblBIAPersonaRespaldoProceso]
ADD CONSTRAINT [PK_tblBIAPersonaRespaldoProceso]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdPersona], [IdProceso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBia], [IdProceso] in table 'tblBIAProceso'
ALTER TABLE [dbo].[tblBIAProceso]
ADD CONSTRAINT [PK_tblBIAProceso]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBia], [IdProceso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdProcesoAlterno] in table 'tblBIAProcesoAlterno'
ALTER TABLE [dbo].[tblBIAProcesoAlterno]
ADD CONSTRAINT [PK_tblBIAProcesoAlterno]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdProcesoAlterno] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdProveedor] in table 'tblBIAProveedor'
ALTER TABLE [dbo].[tblBIAProveedor]
ADD CONSTRAINT [PK_tblBIAProveedor]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdProveedor] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdRespaldo] in table 'tblBIARespaldoPrimario'
ALTER TABLE [dbo].[tblBIARespaldoPrimario]
ADD CONSTRAINT [PK_tblBIARespaldoPrimario]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdRespaldo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdRespaldo] in table 'tblBIARespaldoSecundario'
ALTER TABLE [dbo].[tblBIARespaldoSecundario]
ADD CONSTRAINT [PK_tblBIARespaldoSecundario]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdRespaldo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdRPO] in table 'tblBIARPO'
ALTER TABLE [dbo].[tblBIARPO]
ADD CONSTRAINT [PK_tblBIARPO]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdRPO] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdRTO] in table 'tblBIARTO'
ALTER TABLE [dbo].[tblBIARTO]
ADD CONSTRAINT [PK_tblBIARTO]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdRTO] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdUnidadTrabajo] in table 'tblBIAUnidadTrabajo'
ALTER TABLE [dbo].[tblBIAUnidadTrabajo]
ADD CONSTRAINT [PK_tblBIAUnidadTrabajo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdUnidadTrabajo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdUnidadTrabajo], [IdUnidadTrabajoProceso], [IdUnidadPersona], [IdClienteProceso] in table 'tblBIAUnidadTrabajoPersonas'
ALTER TABLE [dbo].[tblBIAUnidadTrabajoPersonas]
ADD CONSTRAINT [PK_tblBIAUnidadTrabajoPersonas]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdUnidadTrabajo], [IdUnidadTrabajoProceso], [IdUnidadPersona], [IdClienteProceso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdUnidadTrabajo], [IdUnidadTrabajoProceso] in table 'tblBIAUnidadTrabajoProceso'
ALTER TABLE [dbo].[tblBIAUnidadTrabajoProceso]
ADD CONSTRAINT [PK_tblBIAUnidadTrabajoProceso]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdUnidadTrabajo], [IdUnidadTrabajoProceso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdWRT] in table 'tblBIAWRT'
ALTER TABLE [dbo].[tblBIAWRT]
ADD CONSTRAINT [PK_tblBIAWRT]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdWRT] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdCargo] in table 'tblCargo'
ALTER TABLE [dbo].[tblCargo]
ADD CONSTRAINT [PK_tblCargo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdCargo] ASC);
GO

-- Creating primary key on [IdPais], [IdEstado], [IdCiudad] in table 'tblCiudad'
ALTER TABLE [dbo].[tblCiudad]
ADD CONSTRAINT [PK_tblCiudad]
    PRIMARY KEY CLUSTERED ([IdPais], [IdEstado], [IdCiudad] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdControl] in table 'tblControlRiesgo'
ALTER TABLE [dbo].[tblControlRiesgo]
ADD CONSTRAINT [PK_tblControlRiesgo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdControl] ASC);
GO

-- Creating primary key on [FechaAplicacion], [IdEmpresa], [IdTipoEscala] in table 'tblCriticidad'
ALTER TABLE [dbo].[tblCriticidad]
ADD CONSTRAINT [PK_tblCriticidad]
    PRIMARY KEY CLUSTERED ([FechaAplicacion], [IdEmpresa], [IdTipoEscala] ASC);
GO

-- Creating primary key on [Culture], [IdPais], [IdEstado], [IdCiudad] in table 'tblCultura_Ciudad'
ALTER TABLE [dbo].[tblCultura_Ciudad]
ADD CONSTRAINT [PK_tblCultura_Ciudad]
    PRIMARY KEY CLUSTERED ([Culture], [IdPais], [IdEstado], [IdCiudad] ASC);
GO

-- Creating primary key on [Culture], [IdPais], [IdEstado] in table 'tblCultura_Estado'
ALTER TABLE [dbo].[tblCultura_Estado]
ADD CONSTRAINT [PK_tblCultura_Estado]
    PRIMARY KEY CLUSTERED ([Culture], [IdPais], [IdEstado] ASC);
GO

-- Creating primary key on [Culture], [IdEstadoDocumento] in table 'tblCultura_EstadoDocumento'
ALTER TABLE [dbo].[tblCultura_EstadoDocumento]
ADD CONSTRAINT [PK_tblCultura_EstadoDocumento]
    PRIMARY KEY CLUSTERED ([Culture], [IdEstadoDocumento] ASC);
GO

-- Creating primary key on [Culture], [IdEstadoEmpresa] in table 'tblCultura_EstadoEmpresa'
ALTER TABLE [dbo].[tblCultura_EstadoEmpresa]
ADD CONSTRAINT [PK_tblCultura_EstadoEmpresa]
    PRIMARY KEY CLUSTERED ([Culture], [IdEstadoEmpresa] ASC);
GO

-- Creating primary key on [Culture], [IdEstadoProceso] in table 'tblCultura_EstadoProceso'
ALTER TABLE [dbo].[tblCultura_EstadoProceso]
ADD CONSTRAINT [PK_tblCultura_EstadoProceso]
    PRIMARY KEY CLUSTERED ([Culture], [IdEstadoProceso] ASC);
GO

-- Creating primary key on [Culture], [IdEstadoUsuario] in table 'tblCultura_EstadoUsuario'
ALTER TABLE [dbo].[tblCultura_EstadoUsuario]
ADD CONSTRAINT [PK_tblCultura_EstadoUsuario]
    PRIMARY KEY CLUSTERED ([Culture], [IdEstadoUsuario] ASC);
GO

-- Creating primary key on [Culture], [IdMes] in table 'tblCultura_Mes'
ALTER TABLE [dbo].[tblCultura_Mes]
ADD CONSTRAINT [PK_tblCultura_Mes]
    PRIMARY KEY CLUSTERED ([Culture], [IdMes] ASC);
GO

-- Creating primary key on [Culture], [IdNivelImpacto] in table 'tblCultura_NivelImpacto'
ALTER TABLE [dbo].[tblCultura_NivelImpacto]
ADD CONSTRAINT [PK_tblCultura_NivelImpacto]
    PRIMARY KEY CLUSTERED ([Culture], [IdNivelImpacto] ASC);
GO

-- Creating primary key on [Culture], [IdNivelUsuario] in table 'tblCultura_NivelUsuario'
ALTER TABLE [dbo].[tblCultura_NivelUsuario]
ADD CONSTRAINT [PK_tblCultura_NivelUsuario]
    PRIMARY KEY CLUSTERED ([Culture], [IdNivelUsuario] ASC);
GO

-- Creating primary key on [Culture], [IdPais] in table 'tblCultura_Pais'
ALTER TABLE [dbo].[tblCultura_Pais]
ADD CONSTRAINT [PK_tblCultura_Pais]
    PRIMARY KEY CLUSTERED ([Culture], [IdPais] ASC);
GO

-- Creating primary key on [Cultura], [IdEstatus] in table 'tblCultura_PBEPruebaEstatus'
ALTER TABLE [dbo].[tblCultura_PBEPruebaEstatus]
ADD CONSTRAINT [PK_tblCultura_PBEPruebaEstatus]
    PRIMARY KEY CLUSTERED ([Cultura], [IdEstatus] ASC);
GO

-- Creating primary key on [Culture], [IdEstatusActividad] in table 'tblCultura_PlanTrabajoEstatus'
ALTER TABLE [dbo].[tblCultura_PlanTrabajoEstatus]
ADD CONSTRAINT [PK_tblCultura_PlanTrabajoEstatus]
    PRIMARY KEY CLUSTERED ([Culture], [IdEstatusActividad] ASC);
GO

-- Creating primary key on [Culture], [IdTipoActualizacion] in table 'tblCultura_PMTProgramacionTipoActualizacion'
ALTER TABLE [dbo].[tblCultura_PMTProgramacionTipoActualizacion]
ADD CONSTRAINT [PK_tblCultura_PMTProgramacionTipoActualizacion]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoActualizacion] ASC);
GO

-- Creating primary key on [Cultura], [IdTipoNotificacion] in table 'tblCultura_PMTProgramacionTipoNotificacion'
ALTER TABLE [dbo].[tblCultura_PMTProgramacionTipoNotificacion]
ADD CONSTRAINT [PK_tblCultura_PMTProgramacionTipoNotificacion]
    PRIMARY KEY CLUSTERED ([Cultura], [IdTipoNotificacion] ASC);
GO

-- Creating primary key on [Culture], [IdTipoCorreo] in table 'tblCultura_TipoCorreo'
ALTER TABLE [dbo].[tblCultura_TipoCorreo]
ADD CONSTRAINT [PK_tblCultura_TipoCorreo]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoCorreo] ASC);
GO

-- Creating primary key on [Culture], [IdTipoDireccion] in table 'tblCultura_TipoDireccion'
ALTER TABLE [dbo].[tblCultura_TipoDireccion]
ADD CONSTRAINT [PK_tblCultura_TipoDireccion]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoDireccion] ASC);
GO

-- Creating primary key on [Culture], [IdTipoFrecuencia] in table 'tblCultura_TipoFrecuencia'
ALTER TABLE [dbo].[tblCultura_TipoFrecuencia]
ADD CONSTRAINT [PK_tblCultura_TipoFrecuencia]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoFrecuencia] ASC);
GO

-- Creating primary key on [Culture], [IdTipoImpacto] in table 'tblCultura_TipoImpacto'
ALTER TABLE [dbo].[tblCultura_TipoImpacto]
ADD CONSTRAINT [PK_tblCultura_TipoImpacto]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoImpacto] ASC);
GO

-- Creating primary key on [Culture], [IdTipoInterdependencia] in table 'tblCultura_TipoInterdependencia'
ALTER TABLE [dbo].[tblCultura_TipoInterdependencia]
ADD CONSTRAINT [PK_tblCultura_TipoInterdependencia]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoInterdependencia] ASC);
GO

-- Creating primary key on [Culture], [IdTipoRespaldo] in table 'tblCultura_TipoRespaldo'
ALTER TABLE [dbo].[tblCultura_TipoRespaldo]
ADD CONSTRAINT [PK_tblCultura_TipoRespaldo]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoRespaldo] ASC);
GO

-- Creating primary key on [Culture], [IdTipoResultadoPrueba] in table 'tblCultura_TipoResultadoPrueba'
ALTER TABLE [dbo].[tblCultura_TipoResultadoPrueba]
ADD CONSTRAINT [PK_tblCultura_TipoResultadoPrueba]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoResultadoPrueba] ASC);
GO

-- Creating primary key on [Culture], [IdTipoTablaContenido] in table 'tblCultura_TipoTablaContenido'
ALTER TABLE [dbo].[tblCultura_TipoTablaContenido]
ADD CONSTRAINT [PK_tblCultura_TipoTablaContenido]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoTablaContenido] ASC);
GO

-- Creating primary key on [Culture], [IdTipoTelefono] in table 'tblCultura_TipoTelefono'
ALTER TABLE [dbo].[tblCultura_TipoTelefono]
ADD CONSTRAINT [PK_tblCultura_TipoTelefono]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoTelefono] ASC);
GO

-- Creating primary key on [Culture], [IdTipoUbicacionInformacion] in table 'tblCultura_TipoUbicacionInformacion'
ALTER TABLE [dbo].[tblCultura_TipoUbicacionInformacion]
ADD CONSTRAINT [PK_tblCultura_TipoUbicacionInformacion]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoUbicacionInformacion] ASC);
GO

-- Creating primary key on [Culture], [IdFuenteIncidente] in table 'tblCulture_FuenteIncidente'
ALTER TABLE [dbo].[tblCulture_FuenteIncidente]
ADD CONSTRAINT [PK_tblCulture_FuenteIncidente]
    PRIMARY KEY CLUSTERED ([Culture], [IdFuenteIncidente] ASC);
GO

-- Creating primary key on [Culture], [IdNaturalezaIncidente] in table 'tblCulture_NaturalezaIncidente'
ALTER TABLE [dbo].[tblCulture_NaturalezaIncidente]
ADD CONSTRAINT [PK_tblCulture_NaturalezaIncidente]
    PRIMARY KEY CLUSTERED ([Culture], [IdNaturalezaIncidente] ASC);
GO

-- Creating primary key on [Culture], [IdTipoIncidente] in table 'tblCulture_TipoIncidente'
ALTER TABLE [dbo].[tblCulture_TipoIncidente]
ADD CONSTRAINT [PK_tblCulture_TipoIncidente]
    PRIMARY KEY CLUSTERED ([Culture], [IdTipoIncidente] ASC);
GO

-- Creating primary key on [IdDispositivo] in table 'tblDispositivo'
ALTER TABLE [dbo].[tblDispositivo]
ADD CONSTRAINT [PK_tblDispositivo]
    PRIMARY KEY CLUSTERED ([IdDispositivo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDispositivo], [IdUsuario], [fechaConexion] in table 'tblDispositivoConexion'
ALTER TABLE [dbo].[tblDispositivoConexion]
ADD CONSTRAINT [PK_tblDispositivoConexion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDispositivo], [IdUsuario], [fechaConexion] ASC);
GO

-- Creating primary key on [IdDispositivo], [IdSubModulo] in table 'tblDispositivoEnvio'
ALTER TABLE [dbo].[tblDispositivoEnvio]
ADD CONSTRAINT [PK_tblDispositivoEnvio]
    PRIMARY KEY CLUSTERED ([IdDispositivo], [IdSubModulo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblDocumento'
ALTER TABLE [dbo].[tblDocumento]
ADD CONSTRAINT [PK_tblDocumento]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdAnexo] in table 'tblDocumentoAnexo'
ALTER TABLE [dbo].[tblDocumentoAnexo]
ADD CONSTRAINT [PK_tblDocumentoAnexo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdAnexo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdAprobacion] in table 'tblDocumentoAprobacion'
ALTER TABLE [dbo].[tblDocumentoAprobacion]
ADD CONSTRAINT [PK_tblDocumentoAprobacion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdAprobacion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdCertificacion] in table 'tblDocumentoCertificacion'
ALTER TABLE [dbo].[tblDocumentoCertificacion]
ADD CONSTRAINT [PK_tblDocumentoCertificacion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdCertificacion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdSubModulo] in table 'tblDocumentoContenido'
ALTER TABLE [dbo].[tblDocumentoContenido]
ADD CONSTRAINT [PK_tblDocumentoContenido]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdSubModulo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdEntrevista] in table 'tblDocumentoEntrevista'
ALTER TABLE [dbo].[tblDocumentoEntrevista]
ADD CONSTRAINT [PK_tblDocumentoEntrevista]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdEntrevista] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdEntrevista], [IdPersonaEntrevista] in table 'tblDocumentoEntrevistaPersona'
ALTER TABLE [dbo].[tblDocumentoEntrevistaPersona]
ADD CONSTRAINT [PK_tblDocumentoEntrevistaPersona]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdEntrevista], [IdPersonaEntrevista] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdPersona] in table 'tblDocumentoPersonaClave'
ALTER TABLE [dbo].[tblDocumentoPersonaClave]
ADD CONSTRAINT [PK_tblDocumentoPersonaClave]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdPersona] ASC);
GO

-- Creating primary key on [IdEmpresa] in table 'tblEmpresa'
ALTER TABLE [dbo].[tblEmpresa]
ADD CONSTRAINT [PK_tblEmpresa]
    PRIMARY KEY CLUSTERED ([IdEmpresa] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdModulo] in table 'tblEmpresaModulo'
ALTER TABLE [dbo].[tblEmpresaModulo]
ADD CONSTRAINT [PK_tblEmpresaModulo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdModulo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdUsuario] in table 'tblEmpresaUsuario'
ALTER TABLE [dbo].[tblEmpresaUsuario]
ADD CONSTRAINT [PK_tblEmpresaUsuario]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdUsuario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdEscala] in table 'tblEscala'
ALTER TABLE [dbo].[tblEscala]
ADD CONSTRAINT [PK_tblEscala]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdEscala] ASC);
GO

-- Creating primary key on [IdPais], [IdEstado] in table 'tblEstado'
ALTER TABLE [dbo].[tblEstado]
ADD CONSTRAINT [PK_tblEstado]
    PRIMARY KEY CLUSTERED ([IdPais], [IdEstado] ASC);
GO

-- Creating primary key on [IdEstadoDocumento] in table 'tblEstadoDocumento'
ALTER TABLE [dbo].[tblEstadoDocumento]
ADD CONSTRAINT [PK_tblEstadoDocumento]
    PRIMARY KEY CLUSTERED ([IdEstadoDocumento] ASC);
GO

-- Creating primary key on [IdEstadoEmpresa] in table 'tblEstadoEmpresa'
ALTER TABLE [dbo].[tblEstadoEmpresa]
ADD CONSTRAINT [PK_tblEstadoEmpresa]
    PRIMARY KEY CLUSTERED ([IdEstadoEmpresa] ASC);
GO

-- Creating primary key on [IdEstadoProceso] in table 'tblEstadoProceso'
ALTER TABLE [dbo].[tblEstadoProceso]
ADD CONSTRAINT [PK_tblEstadoProceso]
    PRIMARY KEY CLUSTERED ([IdEstadoProceso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdEstadoRiesgo] in table 'tblEstadoRiesgo'
ALTER TABLE [dbo].[tblEstadoRiesgo]
ADD CONSTRAINT [PK_tblEstadoRiesgo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdEstadoRiesgo] ASC);
GO

-- Creating primary key on [IdEstadoUsuario] in table 'tblEstadoUsuario'
ALTER TABLE [dbo].[tblEstadoUsuario]
ADD CONSTRAINT [PK_tblEstadoUsuario]
    PRIMARY KEY CLUSTERED ([IdEstadoUsuario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdCodigoModulo], [IdCorreo] in table 'tblFormatosEmail'
ALTER TABLE [dbo].[tblFormatosEmail]
ADD CONSTRAINT [PK_tblFormatosEmail]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdCodigoModulo], [IdCorreo] ASC);
GO

-- Creating primary key on [IdEmpresa], [CodigoFuente] in table 'tblFuenteRiesgo'
ALTER TABLE [dbo].[tblFuenteRiesgo]
ADD CONSTRAINT [PK_tblFuenteRiesgo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [CodigoFuente] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdImpacto] in table 'tblImpactoRiesgo'
ALTER TABLE [dbo].[tblImpactoRiesgo]
ADD CONSTRAINT [PK_tblImpactoRiesgo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdImpacto] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPrioridad] in table 'tblIniciativaPrioridad'
ALTER TABLE [dbo].[tblIniciativaPrioridad]
ADD CONSTRAINT [PK_tblIniciativaPrioridad]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPrioridad] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdResponsable] in table 'tblIniciativaResponsable'
ALTER TABLE [dbo].[tblIniciativaResponsable]
ADD CONSTRAINT [PK_tblIniciativaResponsable]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdResponsable] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdIniciativa] in table 'tblIniciativas'
ALTER TABLE [dbo].[tblIniciativas]
ADD CONSTRAINT [PK_tblIniciativas]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdIniciativa] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdIniciativa], [IdAnexo] in table 'tblIniciativas_Anexo'
ALTER TABLE [dbo].[tblIniciativas_Anexo]
ADD CONSTRAINT [PK_tblIniciativas_Anexo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdIniciativa], [IdAnexo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdLocalidad] in table 'tblLocalidad'
ALTER TABLE [dbo].[tblLocalidad]
ADD CONSTRAINT [PK_tblLocalidad]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdLocalidad] ASC);
GO

-- Creating primary key on [IdMes] in table 'tblMes'
ALTER TABLE [dbo].[tblMes]
ADD CONSTRAINT [PK_tblMes]
    PRIMARY KEY CLUSTERED ([IdMes] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdModulo] in table 'tblModulo'
ALTER TABLE [dbo].[tblModulo]
ADD CONSTRAINT [PK_tblModulo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdModulo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdNivelUsuario], [IdModulo] in table 'tblModulo_NivelUsuario'
ALTER TABLE [dbo].[tblModulo_NivelUsuario]
ADD CONSTRAINT [PK_tblModulo_NivelUsuario]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdNivelUsuario], [IdModulo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdModulo], [IdUsuario] in table 'tblModulo_Usuario'
ALTER TABLE [dbo].[tblModulo_Usuario]
ADD CONSTRAINT [PK_tblModulo_Usuario]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdModulo], [IdUsuario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdModulo], [Negocios], [IdAnexo] in table 'tblModuloAnexo'
ALTER TABLE [dbo].[tblModuloAnexo]
ADD CONSTRAINT [PK_tblModuloAnexo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdModulo], [Negocios], [IdAnexo] ASC);
GO

-- Creating primary key on [IdNivelImpacto] in table 'tblNivelImpacto'
ALTER TABLE [dbo].[tblNivelImpacto]
ADD CONSTRAINT [PK_tblNivelImpacto]
    PRIMARY KEY CLUSTERED ([IdNivelImpacto] ASC);
GO

-- Creating primary key on [IdNivelUsuario] in table 'tblNivelUsuario'
ALTER TABLE [dbo].[tblNivelUsuario]
ADD CONSTRAINT [PK_tblNivelUsuario]
    PRIMARY KEY CLUSTERED ([IdNivelUsuario] ASC);
GO

-- Creating primary key on [IdPais] in table 'tblPais'
ALTER TABLE [dbo].[tblPais]
ADD CONSTRAINT [PK_tblPais]
    PRIMARY KEY CLUSTERED ([IdPais] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaEjecucion'
ALTER TABLE [dbo].[tblPBEPruebaEjecucion]
ADD CONSTRAINT [PK_tblPBEPruebaEjecucion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdEjercicio] in table 'tblPBEPruebaEjecucionEjercicio'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicio]
ADD CONSTRAINT [PK_tblPBEPruebaEjecucionEjercicio]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdEjercicio] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdEjercicio], [IdParticipante] in table 'tblPBEPruebaEjecucionEjercicioParticipante'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicioParticipante]
ADD CONSTRAINT [PK_tblPBEPruebaEjecucionEjercicioParticipante]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdEjercicio], [IdParticipante] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdEjercicio], [IdRecurso] in table 'tblPBEPruebaEjecucionEjercicioRecurso'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicioRecurso]
ADD CONSTRAINT [PK_tblPBEPruebaEjecucionEjercicioRecurso]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdEjercicio], [IdRecurso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdParticipante] in table 'tblPBEPruebaEjecucionParticipante'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionParticipante]
ADD CONSTRAINT [PK_tblPBEPruebaEjecucionParticipante]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdParticipante] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdContenido] in table 'tblPBEPruebaEjecucionResultado'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionResultado]
ADD CONSTRAINT [PK_tblPBEPruebaEjecucionResultado]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdContenido] ASC);
GO

-- Creating primary key on [IdEstatus] in table 'tblPBEPruebaEstatus'
ALTER TABLE [dbo].[tblPBEPruebaEstatus]
ADD CONSTRAINT [PK_tblPBEPruebaEstatus]
    PRIMARY KEY CLUSTERED ([IdEstatus] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaPlanificacion'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacion]
ADD CONSTRAINT [PK_tblPBEPruebaPlanificacion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdEjercicio] in table 'tblPBEPruebaPlanificacionEjercicio'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicio]
ADD CONSTRAINT [PK_tblPBEPruebaPlanificacionEjercicio]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdEjercicio] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdEjercicio], [IdParticipante] in table 'tblPBEPruebaPlanificacionEjercicioParticipante'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioParticipante]
ADD CONSTRAINT [PK_tblPBEPruebaPlanificacionEjercicioParticipante]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdEjercicio], [IdParticipante] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdEjercicio], [IdRecurso] in table 'tblPBEPruebaPlanificacionEjercicioRecurso'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioRecurso]
ADD CONSTRAINT [PK_tblPBEPruebaPlanificacionEjercicioRecurso]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdEjercicio], [IdRecurso] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanificacion], [IdParticipante] in table 'tblPBEPruebaPlanificacionParticipante'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionParticipante]
ADD CONSTRAINT [PK_tblPBEPruebaPlanificacionParticipante]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanificacion], [IdParticipante] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPersona] in table 'tblPersona'
ALTER TABLE [dbo].[tblPersona]
ADD CONSTRAINT [PK_tblPersona]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPersona] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPersona], [IdPersonaCorreo] in table 'tblPersonaCorreo'
ALTER TABLE [dbo].[tblPersonaCorreo]
ADD CONSTRAINT [PK_tblPersonaCorreo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPersona], [IdPersonaCorreo] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPersona], [IdPersonaDireccion] in table 'tblPersonaDireccion'
ALTER TABLE [dbo].[tblPersonaDireccion]
ADD CONSTRAINT [PK_tblPersonaDireccion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPersona], [IdPersonaDireccion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPersona], [IdPersonaTelefono] in table 'tblPersonaTelefono'
ALTER TABLE [dbo].[tblPersonaTelefono]
ADD CONSTRAINT [PK_tblPersonaTelefono]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPersona], [IdPersonaTelefono] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdAccion], [IdActividad] in table 'tblPlanTrabajo'
ALTER TABLE [dbo].[tblPlanTrabajo]
ADD CONSTRAINT [PK_tblPlanTrabajo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdAccion], [IdActividad] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdPlanAccion] in table 'tblPlanTrabajoAccion'
ALTER TABLE [dbo].[tblPlanTrabajoAccion]
ADD CONSTRAINT [PK_tblPlanTrabajoAccion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdPlanAccion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdAccion], [IdActividad], [IdCambioEstado] in table 'tblPlanTrabajoAuditoria'
ALTER TABLE [dbo].[tblPlanTrabajoAuditoria]
ADD CONSTRAINT [PK_tblPlanTrabajoAuditoria]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdAccion], [IdActividad], [IdCambioEstado] ASC);
GO

-- Creating primary key on [IdEstatusActividad] in table 'tblPlanTrabajoEstatus'
ALTER TABLE [dbo].[tblPlanTrabajoEstatus]
ADD CONSTRAINT [PK_tblPlanTrabajoEstatus]
    PRIMARY KEY CLUSTERED ([IdEstatusActividad] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdMensaje], [IdModulo] in table 'tblPMTMensajeActualizacion'
ALTER TABLE [dbo].[tblPMTMensajeActualizacion]
ADD CONSTRAINT [PK_tblPMTMensajeActualizacion]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdMensaje], [IdModulo] ASC);
GO

-- Creating primary key on [IdPMTProgramacion] in table 'tblPMTProgramacion'
ALTER TABLE [dbo].[tblPMTProgramacion]
ADD CONSTRAINT [PK_tblPMTProgramacion]
    PRIMARY KEY CLUSTERED ([IdPMTProgramacion] ASC);
GO

-- Creating primary key on [IdPMTProgramacion], [IdEmpresa], [IdModulo], [IdDocumento] in table 'tblPMTProgramacionDocumentos'
ALTER TABLE [dbo].[tblPMTProgramacionDocumentos]
ADD CONSTRAINT [PK_tblPMTProgramacionDocumentos]
    PRIMARY KEY CLUSTERED ([IdPMTProgramacion], [IdEmpresa], [IdModulo], [IdDocumento] ASC);
GO

-- Creating primary key on [IdTipoActualizacion] in table 'tblPMTProgramacionTipoActualizacion'
ALTER TABLE [dbo].[tblPMTProgramacionTipoActualizacion]
ADD CONSTRAINT [PK_tblPMTProgramacionTipoActualizacion]
    PRIMARY KEY CLUSTERED ([IdTipoActualizacion] ASC);
GO

-- Creating primary key on [IdTipoNotificacion] in table 'tblPMTProgramacionTipoNotificacion'
ALTER TABLE [dbo].[tblPMTProgramacionTipoNotificacion]
ADD CONSTRAINT [PK_tblPMTProgramacionTipoNotificacion]
    PRIMARY KEY CLUSTERED ([IdTipoNotificacion] ASC);
GO

-- Creating primary key on [IdPMTProgramacion], [IdUsuario] in table 'tblPMTProgramacionUsuario'
ALTER TABLE [dbo].[tblPMTProgramacionUsuario]
ADD CONSTRAINT [PK_tblPMTProgramacionUsuario]
    PRIMARY KEY CLUSTERED ([IdPMTProgramacion], [IdUsuario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdModulo], [IdMensaje] in table 'tblPMTResponsableUpdate'
ALTER TABLE [dbo].[tblPMTResponsableUpdate]
ADD CONSTRAINT [PK_tblPMTResponsableUpdate]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdModulo], [IdMensaje] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdModulo], [IdMensaje], [IdUsuario] in table 'tblPMTResponsableUpdate_Correo'
ALTER TABLE [dbo].[tblPMTResponsableUpdate_Correo]
ADD CONSTRAINT [PK_tblPMTResponsableUpdate_Correo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdModulo], [IdMensaje], [IdUsuario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdPPEFrecuencia] in table 'tblPPEFrecuencia'
ALTER TABLE [dbo].[tblPPEFrecuencia]
ADD CONSTRAINT [PK_tblPPEFrecuencia]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdPPEFrecuencia] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdProbabilidad] in table 'tblProbabilidadRiesgo'
ALTER TABLE [dbo].[tblProbabilidadRiesgo]
ADD CONSTRAINT [PK_tblProbabilidadRiesgo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdProbabilidad] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdProducto] in table 'tblProducto'
ALTER TABLE [dbo].[tblProducto]
ADD CONSTRAINT [PK_tblProducto]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdProducto] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdProveedor] in table 'tblProveedor'
ALTER TABLE [dbo].[tblProveedor]
ADD CONSTRAINT [PK_tblProveedor]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdProveedor] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdSeveridad] in table 'tblSeveridadRiesgo'
ALTER TABLE [dbo].[tblSeveridadRiesgo]
ADD CONSTRAINT [PK_tblSeveridadRiesgo]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdSeveridad] ASC);
GO

-- Creating primary key on [IdTipoCorreo] in table 'tblTipoCorreo'
ALTER TABLE [dbo].[tblTipoCorreo]
ADD CONSTRAINT [PK_tblTipoCorreo]
    PRIMARY KEY CLUSTERED ([IdTipoCorreo] ASC);
GO

-- Creating primary key on [IdTipoDireccion] in table 'tblTipoDireccion'
ALTER TABLE [dbo].[tblTipoDireccion]
ADD CONSTRAINT [PK_tblTipoDireccion]
    PRIMARY KEY CLUSTERED ([IdTipoDireccion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdTipoEscala] in table 'tblTipoEscala'
ALTER TABLE [dbo].[tblTipoEscala]
ADD CONSTRAINT [PK_tblTipoEscala]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdTipoEscala] ASC);
GO

-- Creating primary key on [IdTipoFrecuencia] in table 'tblTipoFrecuencia'
ALTER TABLE [dbo].[tblTipoFrecuencia]
ADD CONSTRAINT [PK_tblTipoFrecuencia]
    PRIMARY KEY CLUSTERED ([IdTipoFrecuencia] ASC);
GO

-- Creating primary key on [IdTipoImpacto] in table 'tblTipoImpacto'
ALTER TABLE [dbo].[tblTipoImpacto]
ADD CONSTRAINT [PK_tblTipoImpacto]
    PRIMARY KEY CLUSTERED ([IdTipoImpacto] ASC);
GO

-- Creating primary key on [IdTipoInterdependencia] in table 'tblTipoInterdependencia'
ALTER TABLE [dbo].[tblTipoInterdependencia]
ADD CONSTRAINT [PK_tblTipoInterdependencia]
    PRIMARY KEY CLUSTERED ([IdTipoInterdependencia] ASC);
GO

-- Creating primary key on [IdTipoRespaldo] in table 'tblTipoRespaldo'
ALTER TABLE [dbo].[tblTipoRespaldo]
ADD CONSTRAINT [PK_tblTipoRespaldo]
    PRIMARY KEY CLUSTERED ([IdTipoRespaldo] ASC);
GO

-- Creating primary key on [IdTipoResultadoPrueba] in table 'tblTipoResultadoPrueba'
ALTER TABLE [dbo].[tblTipoResultadoPrueba]
ADD CONSTRAINT [PK_tblTipoResultadoPrueba]
    PRIMARY KEY CLUSTERED ([IdTipoResultadoPrueba] ASC);
GO

-- Creating primary key on [IdTipoTablaContenido] in table 'tblTipoTablaContenido'
ALTER TABLE [dbo].[tblTipoTablaContenido]
ADD CONSTRAINT [PK_tblTipoTablaContenido]
    PRIMARY KEY CLUSTERED ([IdTipoTablaContenido] ASC);
GO

-- Creating primary key on [IdTipoTelefono] in table 'tblTipoTelefono'
ALTER TABLE [dbo].[tblTipoTelefono]
ADD CONSTRAINT [PK_tblTipoTelefono]
    PRIMARY KEY CLUSTERED ([IdTipoTelefono] ASC);
GO

-- Creating primary key on [IdTipoUbicacionInformacion] in table 'tblTipoUbicacionInformacion'
ALTER TABLE [dbo].[tblTipoUbicacionInformacion]
ADD CONSTRAINT [PK_tblTipoUbicacionInformacion]
    PRIMARY KEY CLUSTERED ([IdTipoUbicacionInformacion] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdUnidadOrganizativa] in table 'tblUnidadOrganizativa'
ALTER TABLE [dbo].[tblUnidadOrganizativa]
ADD CONSTRAINT [PK_tblUnidadOrganizativa]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdUnidadOrganizativa] ASC);
GO

-- Creating primary key on [IdUsuario] in table 'tblUsuario'
ALTER TABLE [dbo].[tblUsuario]
ADD CONSTRAINT [PK_tblUsuario]
    PRIMARY KEY CLUSTERED ([IdUsuario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdUnidadOrganizativa], [IdUsuario] in table 'tblUsuarioUnidadOrganizativa'
ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa]
ADD CONSTRAINT [PK_tblUsuarioUnidadOrganizativa]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdUnidadOrganizativa], [IdUsuario] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdVicepresidencia] in table 'tblVicepresidencia'
ALTER TABLE [dbo].[tblVicepresidencia]
ADD CONSTRAINT [PK_tblVicepresidencia]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdVicepresidencia] ASC);
GO

-- Creating primary key on [IdDispositivo] in table 'tblDispositivo1'
ALTER TABLE [dbo].[tblDispositivo1]
ADD CONSTRAINT [PK_tblDispositivo1]
    PRIMARY KEY CLUSTERED ([IdDispositivo] ASC);
GO

-- Creating primary key on [IdDispositivo], [IdUsuario], [IdEmpresa], [IdSubModulo], [IdUsuarioEnvia], [FechaEnvio] in table 'tblDispositivoEnvio1'
ALTER TABLE [dbo].[tblDispositivoEnvio1]
ADD CONSTRAINT [PK_tblDispositivoEnvio1]
    PRIMARY KEY CLUSTERED ([IdDispositivo], [IdUsuario], [IdEmpresa], [IdSubModulo], [IdUsuarioEnvia], [FechaEnvio] ASC);
GO

-- Creating primary key on [IdFuenteIncidente] in table 'tblFuenteIncidente'
ALTER TABLE [dbo].[tblFuenteIncidente]
ADD CONSTRAINT [PK_tblFuenteIncidente]
    PRIMARY KEY CLUSTERED ([IdFuenteIncidente] ASC);
GO

-- Creating primary key on [IdEmpresa], [IdIncidente] in table 'tblIncidentes'
ALTER TABLE [dbo].[tblIncidentes]
ADD CONSTRAINT [PK_tblIncidentes]
    PRIMARY KEY CLUSTERED ([IdEmpresa], [IdIncidente] ASC);
GO

-- Creating primary key on [IdNaturalezaIncidente] in table 'tblNaturalezaIncidente'
ALTER TABLE [dbo].[tblNaturalezaIncidente]
ADD CONSTRAINT [PK_tblNaturalezaIncidente]
    PRIMARY KEY CLUSTERED ([IdNaturalezaIncidente] ASC);
GO

-- Creating primary key on [IdTipoIncidente] in table 'tblTipoIncidente'
ALTER TABLE [dbo].[tblTipoIncidente]
ADD CONSTRAINT [PK_tblTipoIncidente]
    PRIMARY KEY CLUSTERED ([IdTipoIncidente] ASC);
GO

-- Creating primary key on [IdDispositivo], [IdUsuario], [FechaConexion] in table 'tblDispositivoConexion1'
ALTER TABLE [dbo].[tblDispositivoConexion1]
ADD CONSTRAINT [PK_tblDispositivoConexion1]
    PRIMARY KEY CLUSTERED ([IdDispositivo], [IdUsuario], [FechaConexion] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblAuditoria'
ALTER TABLE [dbo].[tblAuditoria]
ADD CONSTRAINT [FK_tblAuditoria_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblAuditoria_tblDocumento'
CREATE INDEX [IX_FK_tblAuditoria_tblDocumento]
ON [dbo].[tblAuditoria]
    ([IdEmpresa], [IdDocumento], [IdTipoDocumento]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblAuditoria'
ALTER TABLE [dbo].[tblAuditoria]
ADD CONSTRAINT [FK_tblEmpresa_tblAuditoria_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblEmpresa_tblAuditoria_FK1'
CREATE INDEX [IX_FK_tblEmpresa_tblAuditoria_FK1]
ON [dbo].[tblAuditoria]
    ([IdEmpresa]);
GO

-- Creating foreign key on [IdUsuario] in table 'tblAuditoria'
ALTER TABLE [dbo].[tblAuditoria]
ADD CONSTRAINT [FK_tblUsuario_tblAuditoria_FK1]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblUsuario_tblAuditoria_FK1'
CREATE INDEX [IX_FK_tblUsuario_tblAuditoria_FK1]
ON [dbo].[tblAuditoria]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblAuditoriaProcesoCritico'
ALTER TABLE [dbo].[tblAuditoriaProcesoCritico]
ADD CONSTRAINT [FK_tblAuditoriaProcesoCritico_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblAuditoriaProcesoCritico_tblEmpresa'
CREATE INDEX [IX_FK_tblAuditoriaProcesoCritico_tblEmpresa]
ON [dbo].[tblAuditoriaProcesoCritico]
    ([IdEmpresa]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPReanudacionPersonaClave'
ALTER TABLE [dbo].[tblBCPReanudacionPersonaClave]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPReanudacionPersonaClave_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPReanudacionTarea'
ALTER TABLE [dbo].[tblBCPReanudacionTarea]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPReanudacionTarea_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPRecuperacionPersonaClave'
ALTER TABLE [dbo].[tblBCPRecuperacionPersonaClave]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPRecuperacionPersonaClave_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPRecuperacionRecurso'
ALTER TABLE [dbo].[tblBCPRecuperacionRecurso]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPRecuperacionRecurso_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPRespuestaRecurso'
ALTER TABLE [dbo].[tblBCPRespuestaRecurso]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPRespuestaRecurso_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPRestauracionEquipo'
ALTER TABLE [dbo].[tblBCPRestauracionEquipo]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPRestauracionEquipo_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPRestauracionInfraestructura'
ALTER TABLE [dbo].[tblBCPRestauracionInfraestructura]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPRestauracionInfraestructura_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPRestauracionMobiliario'
ALTER TABLE [dbo].[tblBCPRestauracionMobiliario]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPRestauracionMobiliario_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPRestauracionOtro'
ALTER TABLE [dbo].[tblBCPRestauracionOtro]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBCPRestauracionOtro_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBCPDocumento'
ALTER TABLE [dbo].[tblBCPDocumento]
ADD CONSTRAINT [FK_tblBCPDocumento_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBCPDocumento_tblBIAProceso'
CREATE INDEX [IX_FK_tblBCPDocumento_tblBIAProceso]
ON [dbo].[tblBCPDocumento]
    ([IdEmpresa], [IdDocumentoBIA], [IdProceso]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblBCPDocumento'
ALTER TABLE [dbo].[tblBCPDocumento]
ADD CONSTRAINT [FK_tblBCPDocumento_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBCPDocumento_tblDocumento'
CREATE INDEX [IX_FK_tblBCPDocumento_tblDocumento]
ON [dbo].[tblBCPDocumento]
    ([IdEmpresa], [IdDocumento], [IdTipoDocumento]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP] in table 'tblBCPRespuestaAccion'
ALTER TABLE [dbo].[tblBCPRespuestaAccion]
ADD CONSTRAINT [FK_tblBCPRespuestaAccion_tblBCPDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP])
    REFERENCES [dbo].[tblBCPDocumento]
        ([IdEmpresa], [IdDocumentoBCP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblBCPDocumento'
ALTER TABLE [dbo].[tblBCPDocumento]
ADD CONSTRAINT [FK_tblEmpresa_tblBCPDocumento_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPersonaClavePrincipal] in table 'tblBCPReanudacionPersonaClave'
ALTER TABLE [dbo].[tblBCPReanudacionPersonaClave]
ADD CONSTRAINT [FK_tblPersona_tblReanudacionPersonaClave_FK1]
    FOREIGN KEY ([IdEmpresa], [IdPersonaClavePrincipal])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersona_tblReanudacionPersonaClave_FK1'
CREATE INDEX [IX_FK_tblPersona_tblReanudacionPersonaClave_FK1]
ON [dbo].[tblBCPReanudacionPersonaClave]
    ([IdEmpresa], [IdPersonaClavePrincipal]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBCP], [IdBCPReanudacionTarea] in table 'tblBCPReanudacionTareaActividad'
ALTER TABLE [dbo].[tblBCPReanudacionTareaActividad]
ADD CONSTRAINT [FK_tblBCPReanudacionTarea_tblBCPReanudacionTareaActividad_FK1]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBCP], [IdBCPReanudacionTarea])
    REFERENCES [dbo].[tblBCPReanudacionTarea]
        ([IdEmpresa], [IdDocumentoBCP], [IdBCPReanudacionTarea])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPersonaClavePrincipal] in table 'tblBCPRecuperacionPersonaClave'
ALTER TABLE [dbo].[tblBCPRecuperacionPersonaClave]
ADD CONSTRAINT [FK_tblPersona_tblBCPRecuperacionPersonaClave_FK1]
    FOREIGN KEY ([IdEmpresa], [IdPersonaClavePrincipal])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersona_tblBCPRecuperacionPersonaClave_FK1'
CREATE INDEX [IX_FK_tblPersona_tblBCPRecuperacionPersonaClave_FK1]
ON [dbo].[tblBCPRecuperacionPersonaClave]
    ([IdEmpresa], [IdPersonaClavePrincipal]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [FK_tblBIAAmenaza_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAAmenaza_tblBIAProceso'
CREATE INDEX [IX_FK_tblBIAAmenaza_tblBIAProceso]
ON [dbo].[tblBIAAmenaza]
    ([IdEmpresa], [IdDocumentoBIA], [IdProceso]);
GO

-- Creating foreign key on [IdEmpresa], [Control] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [FK_tblBIAAmenaza_tblControlRiesgo]
    FOREIGN KEY ([IdEmpresa], [Control])
    REFERENCES [dbo].[tblControlRiesgo]
        ([IdEmpresa], [IdControl])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAAmenaza_tblControlRiesgo'
CREATE INDEX [IX_FK_tblBIAAmenaza_tblControlRiesgo]
ON [dbo].[tblBIAAmenaza]
    ([IdEmpresa], [Control]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [FK_tblBIAAmenaza_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAAmenaza_tblDocumento'
CREATE INDEX [IX_FK_tblBIAAmenaza_tblDocumento]
ON [dbo].[tblBIAAmenaza]
    ([IdEmpresa], [IdDocumento], [IdTipoDocumento]);
GO

-- Creating foreign key on [IdEmpresa], [Estado] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [FK_tblBIAAmenaza_tblEstadoRiesgo]
    FOREIGN KEY ([IdEmpresa], [Estado])
    REFERENCES [dbo].[tblEstadoRiesgo]
        ([IdEmpresa], [IdEstadoRiesgo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAAmenaza_tblEstadoRiesgo'
CREATE INDEX [IX_FK_tblBIAAmenaza_tblEstadoRiesgo]
ON [dbo].[tblBIAAmenaza]
    ([IdEmpresa], [Estado]);
GO

-- Creating foreign key on [IdEmpresa], [Fuente] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [FK_tblBIAAmenaza_tblFuenteRiesgo]
    FOREIGN KEY ([IdEmpresa], [Fuente])
    REFERENCES [dbo].[tblFuenteRiesgo]
        ([IdEmpresa], [CodigoFuente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAAmenaza_tblFuenteRiesgo'
CREATE INDEX [IX_FK_tblBIAAmenaza_tblFuenteRiesgo]
ON [dbo].[tblBIAAmenaza]
    ([IdEmpresa], [Fuente]);
GO

-- Creating foreign key on [IdEmpresa], [Impacto] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [FK_tblBIAAmenaza_tblImpactoRiesgo]
    FOREIGN KEY ([IdEmpresa], [Impacto])
    REFERENCES [dbo].[tblImpactoRiesgo]
        ([IdEmpresa], [IdImpacto])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAAmenaza_tblImpactoRiesgo'
CREATE INDEX [IX_FK_tblBIAAmenaza_tblImpactoRiesgo]
ON [dbo].[tblBIAAmenaza]
    ([IdEmpresa], [Impacto]);
GO

-- Creating foreign key on [IdEmpresa], [Probabilidad] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [FK_tblBIAAmenaza_tblProbabilidadRiesgo]
    FOREIGN KEY ([IdEmpresa], [Probabilidad])
    REFERENCES [dbo].[tblProbabilidadRiesgo]
        ([IdEmpresa], [IdProbabilidad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAAmenaza_tblProbabilidadRiesgo'
CREATE INDEX [IX_FK_tblBIAAmenaza_tblProbabilidadRiesgo]
ON [dbo].[tblBIAAmenaza]
    ([IdEmpresa], [Probabilidad]);
GO

-- Creating foreign key on [IdEmpresa], [Severidad] in table 'tblBIAAmenaza'
ALTER TABLE [dbo].[tblBIAAmenaza]
ADD CONSTRAINT [FK_tblBIAAmenaza_tblSeveridadRiesgo]
    FOREIGN KEY ([IdEmpresa], [Severidad])
    REFERENCES [dbo].[tblSeveridadRiesgo]
        ([IdEmpresa], [IdSeveridad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAAmenaza_tblSeveridadRiesgo'
CREATE INDEX [IX_FK_tblBIAAmenaza_tblSeveridadRiesgo]
ON [dbo].[tblBIAAmenaza]
    ([IdEmpresa], [Severidad]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAmenaza], [IdAmenazaEvento] in table 'tblBIAEventoControl'
ALTER TABLE [dbo].[tblBIAEventoControl]
ADD CONSTRAINT [FK_tblBIAEventoControl_tblBIAAmenazaEvento]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAmenaza], [IdAmenazaEvento])
    REFERENCES [dbo].[tblBIAAmenazaEvento]
        ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdAmenaza], [IdAmenazaEvento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAAplicacion'
ALTER TABLE [dbo].[tblBIAAplicacion]
ADD CONSTRAINT [FK_tblBIAAplicacion_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblBIACadenaServicio'
ALTER TABLE [dbo].[tblBIACadenaServicio]
ADD CONSTRAINT [FK_tblBIACadenaServicio_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAClienteProceso'
ALTER TABLE [dbo].[tblBIAClienteProceso]
ADD CONSTRAINT [FK_tblBIAClienteProducto_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdClienteProceso] in table 'tblBIAUnidadTrabajoPersonas'
ALTER TABLE [dbo].[tblBIAUnidadTrabajoPersonas]
ADD CONSTRAINT [FK_tblBIAUnidadTrabajoPersonas_tblBIAClienteProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdClienteProceso])
    REFERENCES [dbo].[tblBIAClienteProceso]
        ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdClienteProceso])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAUnidadTrabajoPersonas_tblBIAClienteProceso'
CREATE INDEX [IX_FK_tblBIAUnidadTrabajoPersonas_tblBIAClienteProceso]
ON [dbo].[tblBIAUnidadTrabajoPersonas]
    ([IdEmpresa], [IdDocumentoBIA], [IdProceso], [IdClienteProceso]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBia] in table 'tblBIAComentario'
ALTER TABLE [dbo].[tblBIAComentario]
ADD CONSTRAINT [FK_tblBIAComentario_tblBIADocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBia])
    REFERENCES [dbo].[tblBIADocumento]
        ([IdEmpresa], [IdDocumentoBIA])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIADocumentacion'
ALTER TABLE [dbo].[tblBIADocumentacion]
ADD CONSTRAINT [FK_tblBIADocumentacion_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdUnidadOrganizativa] in table 'tblBIADocumento'
ALTER TABLE [dbo].[tblBIADocumento]
ADD CONSTRAINT [FK_tblBIADocumento_tblUnidadOrganizativa]
    FOREIGN KEY ([IdEmpresa], [IdUnidadOrganizativa])
    REFERENCES [dbo].[tblUnidadOrganizativa]
        ([IdEmpresa], [IdUnidadOrganizativa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIADocumento_tblUnidadOrganizativa'
CREATE INDEX [IX_FK_tblBIADocumento_tblUnidadOrganizativa]
ON [dbo].[tblBIADocumento]
    ([IdEmpresa], [IdUnidadOrganizativa]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBia] in table 'tblBIAProceso'
ALTER TABLE [dbo].[tblBIAProceso]
ADD CONSTRAINT [FK_tblBIAProceso_tblBIADocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBia])
    REFERENCES [dbo].[tblBIADocumento]
        ([IdEmpresa], [IdDocumentoBIA])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblBIADocumento'
ALTER TABLE [dbo].[tblBIADocumento]
ADD CONSTRAINT [FK_tblDocumento_tblBIADocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDocumento_tblBIADocumento'
CREATE INDEX [IX_FK_tblDocumento_tblBIADocumento]
ON [dbo].[tblBIADocumento]
    ([IdEmpresa], [IdDocumento], [IdTipoDocumento]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblBIADocumento'
ALTER TABLE [dbo].[tblBIADocumento]
ADD CONSTRAINT [FK_tblEmpresa_tblBIADocumento_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAEntrada'
ALTER TABLE [dbo].[tblBIAEntrada]
ADD CONSTRAINT [FK_tblBIAEntrada_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAGranImpacto'
ALTER TABLE [dbo].[tblBIAGranImpacto]
ADD CONSTRAINT [FK_tblBIAGranImpacto_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdMes] in table 'tblBIAGranImpacto'
ALTER TABLE [dbo].[tblBIAGranImpacto]
ADD CONSTRAINT [FK_tblBIAGranImpacto_tblMes]
    FOREIGN KEY ([IdMes])
    REFERENCES [dbo].[tblMes]
        ([IdMes])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAGranImpacto_tblMes'
CREATE INDEX [IX_FK_tblBIAGranImpacto_tblMes]
ON [dbo].[tblBIAGranImpacto]
    ([IdMes]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAImpactoFinanciero'
ALTER TABLE [dbo].[tblBIAImpactoFinanciero]
ADD CONSTRAINT [FK_tblBIAImpactoFinanciero_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdEscala] in table 'tblBIAImpactoFinanciero'
ALTER TABLE [dbo].[tblBIAImpactoFinanciero]
ADD CONSTRAINT [FK_tblBIAImpactoFinanciero_tblEscala]
    FOREIGN KEY ([IdEmpresa], [IdEscala])
    REFERENCES [dbo].[tblEscala]
        ([IdEmpresa], [IdEscala])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAImpactoFinanciero_tblEscala'
CREATE INDEX [IX_FK_tblBIAImpactoFinanciero_tblEscala]
ON [dbo].[tblBIAImpactoFinanciero]
    ([IdEmpresa], [IdEscala]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblBIAImpactoFinanciero'
ALTER TABLE [dbo].[tblBIAImpactoFinanciero]
ADD CONSTRAINT [FK_tblBIAImpactoFinanciero_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAImpactoFinanciero_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblBIAImpactoFinanciero_tblTipoFrecuencia]
ON [dbo].[tblBIAImpactoFinanciero]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAImpactoOperacional'
ALTER TABLE [dbo].[tblBIAImpactoOperacional]
ADD CONSTRAINT [FK_tblBIAImpactoOperacional_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdEscala] in table 'tblBIAImpactoOperacional'
ALTER TABLE [dbo].[tblBIAImpactoOperacional]
ADD CONSTRAINT [FK_tblBIAImpactoOperacional_tblEscala]
    FOREIGN KEY ([IdEmpresa], [IdEscala])
    REFERENCES [dbo].[tblEscala]
        ([IdEmpresa], [IdEscala])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAImpactoOperacional_tblEscala'
CREATE INDEX [IX_FK_tblBIAImpactoOperacional_tblEscala]
ON [dbo].[tblBIAImpactoOperacional]
    ([IdEmpresa], [IdEscala]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblBIAImpactoOperacional'
ALTER TABLE [dbo].[tblBIAImpactoOperacional]
ADD CONSTRAINT [FK_tblBIAImpactoOperacional_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAImpactoOperacional_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblBIAImpactoOperacional_tblTipoFrecuencia]
ON [dbo].[tblBIAImpactoOperacional]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAInterdependencia'
ALTER TABLE [dbo].[tblBIAInterdependencia]
ADD CONSTRAINT [FK_tblBIAInterdependencia_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAMTD'
ALTER TABLE [dbo].[tblBIAMTD]
ADD CONSTRAINT [FK_tblBIAMTD_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdEscala] in table 'tblBIAMTD'
ALTER TABLE [dbo].[tblBIAMTD]
ADD CONSTRAINT [FK_tblBIAMTD_tblEscala]
    FOREIGN KEY ([IdEmpresa], [IdEscala])
    REFERENCES [dbo].[tblEscala]
        ([IdEmpresa], [IdEscala])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAMTD_tblEscala'
CREATE INDEX [IX_FK_tblBIAMTD_tblEscala]
ON [dbo].[tblBIAMTD]
    ([IdEmpresa], [IdEscala]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblBIAMTD'
ALTER TABLE [dbo].[tblBIAMTD]
ADD CONSTRAINT [FK_tblBIAMTD_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAMTD_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblBIAMTD_tblTipoFrecuencia]
ON [dbo].[tblBIAMTD]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAPersonaClave'
ALTER TABLE [dbo].[tblBIAPersonaClave]
ADD CONSTRAINT [FK_tblBIAPersonaClave_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblBIAPersonaClave'
ALTER TABLE [dbo].[tblBIAPersonaClave]
ADD CONSTRAINT [FK_tblBIAPersonaClave_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAPersonaClave_tblDocumento'
CREATE INDEX [IX_FK_tblBIAPersonaClave_tblDocumento]
ON [dbo].[tblBIAPersonaClave]
    ([IdEmpresa], [IdDocumento], [IdTipoDocumento]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdPersonaClave] in table 'tblBIAPersonaClave'
ALTER TABLE [dbo].[tblBIAPersonaClave]
ADD CONSTRAINT [FK_tblBIAPersonaClave_tblDocumentoPersonaClave]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdPersonaClave])
    REFERENCES [dbo].[tblDocumentoPersonaClave]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdPersona])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAPersonaClave_tblDocumentoPersonaClave'
CREATE INDEX [IX_FK_tblBIAPersonaClave_tblDocumentoPersonaClave]
ON [dbo].[tblBIAPersonaClave]
    ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdPersonaClave]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAPersonaRespaldoProceso'
ALTER TABLE [dbo].[tblBIAPersonaRespaldoProceso]
ADD CONSTRAINT [FK_tblBIAPersonaRespaldoProceso_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAPersonaRespaldoProceso_tblBIAProceso'
CREATE INDEX [IX_FK_tblBIAPersonaRespaldoProceso_tblBIAProceso]
ON [dbo].[tblBIAPersonaRespaldoProceso]
    ([IdEmpresa], [IdDocumentoBIA], [IdProceso]);
GO

-- Creating foreign key on [IdEmpresa], [IdPersona] in table 'tblBIAPersonaRespaldoProceso'
ALTER TABLE [dbo].[tblBIAPersonaRespaldoProceso]
ADD CONSTRAINT [FK_tblBIAPersonaRespaldoProceso_tblPersona]
    FOREIGN KEY ([IdEmpresa], [IdPersona])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAPersonaRespaldoProceso_tblPersona'
CREATE INDEX [IX_FK_tblBIAPersonaRespaldoProceso_tblPersona]
ON [dbo].[tblBIAPersonaRespaldoProceso]
    ([IdEmpresa], [IdPersona]);
GO

-- Creating foreign key on [IdEstadoProceso] in table 'tblBIAProceso'
ALTER TABLE [dbo].[tblBIAProceso]
ADD CONSTRAINT [FK_tblBIAProceso_tblEstadoProceso]
    FOREIGN KEY ([IdEstadoProceso])
    REFERENCES [dbo].[tblEstadoProceso]
        ([IdEstadoProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAProceso_tblEstadoProceso'
CREATE INDEX [IX_FK_tblBIAProceso_tblEstadoProceso]
ON [dbo].[tblBIAProceso]
    ([IdEstadoProceso]);
GO

-- Creating foreign key on [IdEmpresa], [IdUnidadOrganizativa] in table 'tblBIAProceso'
ALTER TABLE [dbo].[tblBIAProceso]
ADD CONSTRAINT [FK_tblBIAProceso_tblUnidadOrganizativa]
    FOREIGN KEY ([IdEmpresa], [IdUnidadOrganizativa])
    REFERENCES [dbo].[tblUnidadOrganizativa]
        ([IdEmpresa], [IdUnidadOrganizativa])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAProceso_tblUnidadOrganizativa'
CREATE INDEX [IX_FK_tblBIAProceso_tblUnidadOrganizativa]
ON [dbo].[tblBIAProceso]
    ([IdEmpresa], [IdUnidadOrganizativa]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAProcesoAlterno'
ALTER TABLE [dbo].[tblBIAProcesoAlterno]
ADD CONSTRAINT [FK_tblBIAProcesoAlterno_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAProveedor'
ALTER TABLE [dbo].[tblBIAProveedor]
ADD CONSTRAINT [FK_tblBIAProveedor_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIARespaldoPrimario'
ALTER TABLE [dbo].[tblBIARespaldoPrimario]
ADD CONSTRAINT [FK_tblBIARespaldoPrimario_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIARespaldoSecundario'
ALTER TABLE [dbo].[tblBIARespaldoSecundario]
ADD CONSTRAINT [FK_tblBIARespaldoSecundario_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIARPO'
ALTER TABLE [dbo].[tblBIARPO]
ADD CONSTRAINT [FK_tblBIARPO_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIARTO'
ALTER TABLE [dbo].[tblBIARTO]
ADD CONSTRAINT [FK_tblBIARTO_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAUnidadTrabajoProceso'
ALTER TABLE [dbo].[tblBIAUnidadTrabajoProceso]
ADD CONSTRAINT [FK_tblBIAUnidadTrabajoProceso_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAUnidadTrabajoProceso_tblBIAProceso'
CREATE INDEX [IX_FK_tblBIAUnidadTrabajoProceso_tblBIAProceso]
ON [dbo].[tblBIAUnidadTrabajoProceso]
    ([IdEmpresa], [IdDocumentoBIA], [IdProceso]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumentoBIA], [IdProceso] in table 'tblBIAWRT'
ALTER TABLE [dbo].[tblBIAWRT]
ADD CONSTRAINT [FK_tblBIAWRT_tblBIAProceso]
    FOREIGN KEY ([IdEmpresa], [IdDocumentoBIA], [IdProceso])
    REFERENCES [dbo].[tblBIAProceso]
        ([IdEmpresa], [IdDocumentoBia], [IdProceso])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdEscala] in table 'tblBIARPO'
ALTER TABLE [dbo].[tblBIARPO]
ADD CONSTRAINT [FK_tblBIARPO_tblEscala]
    FOREIGN KEY ([IdEmpresa], [IdEscala])
    REFERENCES [dbo].[tblEscala]
        ([IdEmpresa], [IdEscala])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIARPO_tblEscala'
CREATE INDEX [IX_FK_tblBIARPO_tblEscala]
ON [dbo].[tblBIARPO]
    ([IdEmpresa], [IdEscala]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblBIARPO'
ALTER TABLE [dbo].[tblBIARPO]
ADD CONSTRAINT [FK_tblBIARPO_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIARPO_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblBIARPO_tblTipoFrecuencia]
ON [dbo].[tblBIARPO]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdEmpresa], [IdEscala] in table 'tblBIARTO'
ALTER TABLE [dbo].[tblBIARTO]
ADD CONSTRAINT [FK_tblBIARTO_tblEscala]
    FOREIGN KEY ([IdEmpresa], [IdEscala])
    REFERENCES [dbo].[tblEscala]
        ([IdEmpresa], [IdEscala])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIARTO_tblEscala'
CREATE INDEX [IX_FK_tblBIARTO_tblEscala]
ON [dbo].[tblBIARTO]
    ([IdEmpresa], [IdEscala]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblBIARTO'
ALTER TABLE [dbo].[tblBIARTO]
ADD CONSTRAINT [FK_tblBIARTO_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIARTO_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblBIARTO_tblTipoFrecuencia]
ON [dbo].[tblBIARTO]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblBIAUnidadTrabajo'
ALTER TABLE [dbo].[tblBIAUnidadTrabajo]
ADD CONSTRAINT [FK_tblBIAUnidadTrabajo_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdUnidadOrganizativa] in table 'tblBIAUnidadTrabajo'
ALTER TABLE [dbo].[tblBIAUnidadTrabajo]
ADD CONSTRAINT [FK_tblBIAUnidadTrabajo_tblUnidadOrganizativa]
    FOREIGN KEY ([IdEmpresa], [IdUnidadOrganizativa])
    REFERENCES [dbo].[tblUnidadOrganizativa]
        ([IdEmpresa], [IdUnidadOrganizativa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAUnidadTrabajo_tblUnidadOrganizativa'
CREATE INDEX [IX_FK_tblBIAUnidadTrabajo_tblUnidadOrganizativa]
ON [dbo].[tblBIAUnidadTrabajo]
    ([IdEmpresa], [IdUnidadOrganizativa]);
GO

-- Creating foreign key on [IdEmpresa], [IdUnidadTrabajo] in table 'tblBIAUnidadTrabajoProceso'
ALTER TABLE [dbo].[tblBIAUnidadTrabajoProceso]
ADD CONSTRAINT [FK_tblBIAUnidadTrabajoProceso_tblBIAUnidadTrabajo]
    FOREIGN KEY ([IdEmpresa], [IdUnidadTrabajo])
    REFERENCES [dbo].[tblBIAUnidadTrabajo]
        ([IdEmpresa], [IdUnidadTrabajo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdUnidadTrabajo], [IdUnidadTrabajoProceso] in table 'tblBIAUnidadTrabajoPersonas'
ALTER TABLE [dbo].[tblBIAUnidadTrabajoPersonas]
ADD CONSTRAINT [FK_tblBIAUnidadTrabajoPersonas_tblBIAUnidadTrabajoProceso]
    FOREIGN KEY ([IdEmpresa], [IdUnidadTrabajo], [IdUnidadTrabajoProceso])
    REFERENCES [dbo].[tblBIAUnidadTrabajoProceso]
        ([IdEmpresa], [IdUnidadTrabajo], [IdUnidadTrabajoProceso])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdEscala] in table 'tblBIAWRT'
ALTER TABLE [dbo].[tblBIAWRT]
ADD CONSTRAINT [FK_tblBIAWRT_tblEscala]
    FOREIGN KEY ([IdEmpresa], [IdEscala])
    REFERENCES [dbo].[tblEscala]
        ([IdEmpresa], [IdEscala])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAWRT_tblEscala'
CREATE INDEX [IX_FK_tblBIAWRT_tblEscala]
ON [dbo].[tblBIAWRT]
    ([IdEmpresa], [IdEscala]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblBIAWRT'
ALTER TABLE [dbo].[tblBIAWRT]
ADD CONSTRAINT [FK_tblBIAWRT_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblBIAWRT_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblBIAWRT_tblTipoFrecuencia]
ON [dbo].[tblBIAWRT]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdEmpresa], [IdCargo] in table 'tblPersona'
ALTER TABLE [dbo].[tblPersona]
ADD CONSTRAINT [FK_tblCargo_tblPersona_FK1]
    FOREIGN KEY ([IdEmpresa], [IdCargo])
    REFERENCES [dbo].[tblCargo]
        ([IdEmpresa], [IdCargo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCargo_tblPersona_FK1'
CREATE INDEX [IX_FK_tblCargo_tblPersona_FK1]
ON [dbo].[tblPersona]
    ([IdEmpresa], [IdCargo]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblCargo'
ALTER TABLE [dbo].[tblCargo]
ADD CONSTRAINT [FK_tblEmpresa_tblCargo_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdPais], [IdEstado] in table 'tblCiudad'
ALTER TABLE [dbo].[tblCiudad]
ADD CONSTRAINT [FK_tblCiudad_tblEstado]
    FOREIGN KEY ([IdPais], [IdEstado])
    REFERENCES [dbo].[tblEstado]
        ([IdPais], [IdEstado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdPais] in table 'tblCiudad'
ALTER TABLE [dbo].[tblCiudad]
ADD CONSTRAINT [FK_tblCiudad_tblPais]
    FOREIGN KEY ([IdPais])
    REFERENCES [dbo].[tblPais]
        ([IdPais])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdPais], [IdEstado], [IdCiudad] in table 'tblCultura_Ciudad'
ALTER TABLE [dbo].[tblCultura_Ciudad]
ADD CONSTRAINT [FK_tblCulture_Ciudad_tblCiudad]
    FOREIGN KEY ([IdPais], [IdEstado], [IdCiudad])
    REFERENCES [dbo].[tblCiudad]
        ([IdPais], [IdEstado], [IdCiudad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCulture_Ciudad_tblCiudad'
CREATE INDEX [IX_FK_tblCulture_Ciudad_tblCiudad]
ON [dbo].[tblCultura_Ciudad]
    ([IdPais], [IdEstado], [IdCiudad]);
GO

-- Creating foreign key on [IdPais], [IdPaisEstado], [IdPaisEstadoCiudad] in table 'tblEmpresa'
ALTER TABLE [dbo].[tblEmpresa]
ADD CONSTRAINT [FK_tblEmpresa_tblCiudad]
    FOREIGN KEY ([IdPais], [IdPaisEstado], [IdPaisEstadoCiudad])
    REFERENCES [dbo].[tblCiudad]
        ([IdPais], [IdEstado], [IdCiudad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblEmpresa_tblCiudad'
CREATE INDEX [IX_FK_tblEmpresa_tblCiudad]
ON [dbo].[tblEmpresa]
    ([IdPais], [IdPaisEstado], [IdPaisEstadoCiudad]);
GO

-- Creating foreign key on [IdPais], [IdEstado], [IdCiudad] in table 'tblLocalidad'
ALTER TABLE [dbo].[tblLocalidad]
ADD CONSTRAINT [FK_tblLocalidad_tblCiudad]
    FOREIGN KEY ([IdPais], [IdEstado], [IdCiudad])
    REFERENCES [dbo].[tblCiudad]
        ([IdPais], [IdEstado], [IdCiudad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblLocalidad_tblCiudad'
CREATE INDEX [IX_FK_tblLocalidad_tblCiudad]
ON [dbo].[tblLocalidad]
    ([IdPais], [IdEstado], [IdCiudad]);
GO

-- Creating foreign key on [IdPais], [IdEstado], [IdCiudad] in table 'tblPersonaDireccion'
ALTER TABLE [dbo].[tblPersonaDireccion]
ADD CONSTRAINT [FK_tblPersonaDireccion_tblCiudad]
    FOREIGN KEY ([IdPais], [IdEstado], [IdCiudad])
    REFERENCES [dbo].[tblCiudad]
        ([IdPais], [IdEstado], [IdCiudad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersonaDireccion_tblCiudad'
CREATE INDEX [IX_FK_tblPersonaDireccion_tblCiudad]
ON [dbo].[tblPersonaDireccion]
    ([IdPais], [IdEstado], [IdCiudad]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblCriticidad'
ALTER TABLE [dbo].[tblCriticidad]
ADD CONSTRAINT [FK_tblCriticidad_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCriticidad_tblEmpresa'
CREATE INDEX [IX_FK_tblCriticidad_tblEmpresa]
ON [dbo].[tblCriticidad]
    ([IdEmpresa]);
GO

-- Creating foreign key on [IdEmpresa], [IdTipoEscala] in table 'tblCriticidad'
ALTER TABLE [dbo].[tblCriticidad]
ADD CONSTRAINT [FK_tblCriticidad_tblTipoEscala]
    FOREIGN KEY ([IdEmpresa], [IdTipoEscala])
    REFERENCES [dbo].[tblEscala]
        ([IdEmpresa], [IdEscala])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCriticidad_tblTipoEscala'
CREATE INDEX [IX_FK_tblCriticidad_tblTipoEscala]
ON [dbo].[tblCriticidad]
    ([IdEmpresa], [IdTipoEscala]);
GO

-- Creating foreign key on [IdPais], [IdEstado] in table 'tblCultura_Estado'
ALTER TABLE [dbo].[tblCultura_Estado]
ADD CONSTRAINT [FK_tblCultura_Estado_tblEstado]
    FOREIGN KEY ([IdPais], [IdEstado])
    REFERENCES [dbo].[tblEstado]
        ([IdPais], [IdEstado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_Estado_tblEstado'
CREATE INDEX [IX_FK_tblCultura_Estado_tblEstado]
ON [dbo].[tblCultura_Estado]
    ([IdPais], [IdEstado]);
GO

-- Creating foreign key on [IdEstadoDocumento] in table 'tblCultura_EstadoDocumento'
ALTER TABLE [dbo].[tblCultura_EstadoDocumento]
ADD CONSTRAINT [FK_tblCultura_EstadoDocumento_tblEstadoDocumento]
    FOREIGN KEY ([IdEstadoDocumento])
    REFERENCES [dbo].[tblEstadoDocumento]
        ([IdEstadoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_EstadoDocumento_tblEstadoDocumento'
CREATE INDEX [IX_FK_tblCultura_EstadoDocumento_tblEstadoDocumento]
ON [dbo].[tblCultura_EstadoDocumento]
    ([IdEstadoDocumento]);
GO

-- Creating foreign key on [IdEstadoEmpresa] in table 'tblCultura_EstadoEmpresa'
ALTER TABLE [dbo].[tblCultura_EstadoEmpresa]
ADD CONSTRAINT [FK_tblCultura_EstadoEmpresa_tblEstadoEmpresa]
    FOREIGN KEY ([IdEstadoEmpresa])
    REFERENCES [dbo].[tblEstadoEmpresa]
        ([IdEstadoEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_EstadoEmpresa_tblEstadoEmpresa'
CREATE INDEX [IX_FK_tblCultura_EstadoEmpresa_tblEstadoEmpresa]
ON [dbo].[tblCultura_EstadoEmpresa]
    ([IdEstadoEmpresa]);
GO

-- Creating foreign key on [IdEstadoProceso] in table 'tblCultura_EstadoProceso'
ALTER TABLE [dbo].[tblCultura_EstadoProceso]
ADD CONSTRAINT [FK_tblCultura_EstadoProceso_tblEstadoProceso]
    FOREIGN KEY ([IdEstadoProceso])
    REFERENCES [dbo].[tblEstadoProceso]
        ([IdEstadoProceso])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_EstadoProceso_tblEstadoProceso'
CREATE INDEX [IX_FK_tblCultura_EstadoProceso_tblEstadoProceso]
ON [dbo].[tblCultura_EstadoProceso]
    ([IdEstadoProceso]);
GO

-- Creating foreign key on [IdEstadoUsuario] in table 'tblCultura_EstadoUsuario'
ALTER TABLE [dbo].[tblCultura_EstadoUsuario]
ADD CONSTRAINT [FK_tblCultura_EstadoUsuario_tblEstadoUsuario]
    FOREIGN KEY ([IdEstadoUsuario])
    REFERENCES [dbo].[tblEstadoUsuario]
        ([IdEstadoUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_EstadoUsuario_tblEstadoUsuario'
CREATE INDEX [IX_FK_tblCultura_EstadoUsuario_tblEstadoUsuario]
ON [dbo].[tblCultura_EstadoUsuario]
    ([IdEstadoUsuario]);
GO

-- Creating foreign key on [IdMes] in table 'tblCultura_Mes'
ALTER TABLE [dbo].[tblCultura_Mes]
ADD CONSTRAINT [FK_tblCultura_Mes_tblMes]
    FOREIGN KEY ([IdMes])
    REFERENCES [dbo].[tblMes]
        ([IdMes])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_Mes_tblMes'
CREATE INDEX [IX_FK_tblCultura_Mes_tblMes]
ON [dbo].[tblCultura_Mes]
    ([IdMes]);
GO

-- Creating foreign key on [IdNivelImpacto] in table 'tblCultura_NivelImpacto'
ALTER TABLE [dbo].[tblCultura_NivelImpacto]
ADD CONSTRAINT [FK_tblCultura_NivelImpacto_tblNivelImpacto]
    FOREIGN KEY ([IdNivelImpacto])
    REFERENCES [dbo].[tblNivelImpacto]
        ([IdNivelImpacto])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_NivelImpacto_tblNivelImpacto'
CREATE INDEX [IX_FK_tblCultura_NivelImpacto_tblNivelImpacto]
ON [dbo].[tblCultura_NivelImpacto]
    ([IdNivelImpacto]);
GO

-- Creating foreign key on [IdNivelUsuario] in table 'tblCultura_NivelUsuario'
ALTER TABLE [dbo].[tblCultura_NivelUsuario]
ADD CONSTRAINT [FK_tblCulture_NivelUsuario_tblNivelUsuario]
    FOREIGN KEY ([IdNivelUsuario])
    REFERENCES [dbo].[tblNivelUsuario]
        ([IdNivelUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCulture_NivelUsuario_tblNivelUsuario'
CREATE INDEX [IX_FK_tblCulture_NivelUsuario_tblNivelUsuario]
ON [dbo].[tblCultura_NivelUsuario]
    ([IdNivelUsuario]);
GO

-- Creating foreign key on [IdPais] in table 'tblCultura_Pais'
ALTER TABLE [dbo].[tblCultura_Pais]
ADD CONSTRAINT [FK_tblCulture_Pais_tblPais]
    FOREIGN KEY ([IdPais])
    REFERENCES [dbo].[tblPais]
        ([IdPais])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCulture_Pais_tblPais'
CREATE INDEX [IX_FK_tblCulture_Pais_tblPais]
ON [dbo].[tblCultura_Pais]
    ([IdPais]);
GO

-- Creating foreign key on [IdEstatus] in table 'tblCultura_PBEPruebaEstatus'
ALTER TABLE [dbo].[tblCultura_PBEPruebaEstatus]
ADD CONSTRAINT [FK_tblCultura_PBEPruebaEjecucionEstatus_tblPBEPruebaEjecucionEstatus]
    FOREIGN KEY ([IdEstatus])
    REFERENCES [dbo].[tblPBEPruebaEstatus]
        ([IdEstatus])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_PBEPruebaEjecucionEstatus_tblPBEPruebaEjecucionEstatus'
CREATE INDEX [IX_FK_tblCultura_PBEPruebaEjecucionEstatus_tblPBEPruebaEjecucionEstatus]
ON [dbo].[tblCultura_PBEPruebaEstatus]
    ([IdEstatus]);
GO

-- Creating foreign key on [IdEstatusActividad] in table 'tblCultura_PlanTrabajoEstatus'
ALTER TABLE [dbo].[tblCultura_PlanTrabajoEstatus]
ADD CONSTRAINT [FK_tblCultura_PlanTrabajoEstatus_tblPlanTrabajoEstatus]
    FOREIGN KEY ([IdEstatusActividad])
    REFERENCES [dbo].[tblPlanTrabajoEstatus]
        ([IdEstatusActividad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_PlanTrabajoEstatus_tblPlanTrabajoEstatus'
CREATE INDEX [IX_FK_tblCultura_PlanTrabajoEstatus_tblPlanTrabajoEstatus]
ON [dbo].[tblCultura_PlanTrabajoEstatus]
    ([IdEstatusActividad]);
GO

-- Creating foreign key on [IdTipoActualizacion] in table 'tblCultura_PMTProgramacionTipoActualizacion'
ALTER TABLE [dbo].[tblCultura_PMTProgramacionTipoActualizacion]
ADD CONSTRAINT [FK_tblCultura_PMTProgramacionTipoActualizacion_tblPMTProgramacionTipoActualizacion]
    FOREIGN KEY ([IdTipoActualizacion])
    REFERENCES [dbo].[tblPMTProgramacionTipoActualizacion]
        ([IdTipoActualizacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_PMTProgramacionTipoActualizacion_tblPMTProgramacionTipoActualizacion'
CREATE INDEX [IX_FK_tblCultura_PMTProgramacionTipoActualizacion_tblPMTProgramacionTipoActualizacion]
ON [dbo].[tblCultura_PMTProgramacionTipoActualizacion]
    ([IdTipoActualizacion]);
GO

-- Creating foreign key on [IdTipoNotificacion] in table 'tblCultura_PMTProgramacionTipoNotificacion'
ALTER TABLE [dbo].[tblCultura_PMTProgramacionTipoNotificacion]
ADD CONSTRAINT [FK_tblCultura_PMTProgramacionTipoNotificacion_tblPMTProgramacionTipoNotificacion]
    FOREIGN KEY ([IdTipoNotificacion])
    REFERENCES [dbo].[tblPMTProgramacionTipoNotificacion]
        ([IdTipoNotificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_PMTProgramacionTipoNotificacion_tblPMTProgramacionTipoNotificacion'
CREATE INDEX [IX_FK_tblCultura_PMTProgramacionTipoNotificacion_tblPMTProgramacionTipoNotificacion]
ON [dbo].[tblCultura_PMTProgramacionTipoNotificacion]
    ([IdTipoNotificacion]);
GO

-- Creating foreign key on [IdTipoCorreo] in table 'tblCultura_TipoCorreo'
ALTER TABLE [dbo].[tblCultura_TipoCorreo]
ADD CONSTRAINT [FK_tblCultura_TipoCorreo_tblTipoCorreo]
    FOREIGN KEY ([IdTipoCorreo])
    REFERENCES [dbo].[tblTipoCorreo]
        ([IdTipoCorreo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoCorreo_tblTipoCorreo'
CREATE INDEX [IX_FK_tblCultura_TipoCorreo_tblTipoCorreo]
ON [dbo].[tblCultura_TipoCorreo]
    ([IdTipoCorreo]);
GO

-- Creating foreign key on [IdTipoDireccion] in table 'tblCultura_TipoDireccion'
ALTER TABLE [dbo].[tblCultura_TipoDireccion]
ADD CONSTRAINT [FK_tblCultura_TipoDireccion_tblTipoDireccion]
    FOREIGN KEY ([IdTipoDireccion])
    REFERENCES [dbo].[tblTipoDireccion]
        ([IdTipoDireccion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoDireccion_tblTipoDireccion'
CREATE INDEX [IX_FK_tblCultura_TipoDireccion_tblTipoDireccion]
ON [dbo].[tblCultura_TipoDireccion]
    ([IdTipoDireccion]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblCultura_TipoFrecuencia'
ALTER TABLE [dbo].[tblCultura_TipoFrecuencia]
ADD CONSTRAINT [FK_tblCultura_TipoFrecuencia_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoFrecuencia_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblCultura_TipoFrecuencia_tblTipoFrecuencia]
ON [dbo].[tblCultura_TipoFrecuencia]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdTipoImpacto] in table 'tblCultura_TipoImpacto'
ALTER TABLE [dbo].[tblCultura_TipoImpacto]
ADD CONSTRAINT [FK_tblCultura_TipoImpacto_tblTipoImpacto]
    FOREIGN KEY ([IdTipoImpacto])
    REFERENCES [dbo].[tblTipoImpacto]
        ([IdTipoImpacto])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoImpacto_tblTipoImpacto'
CREATE INDEX [IX_FK_tblCultura_TipoImpacto_tblTipoImpacto]
ON [dbo].[tblCultura_TipoImpacto]
    ([IdTipoImpacto]);
GO

-- Creating foreign key on [IdTipoInterdependencia] in table 'tblCultura_TipoInterdependencia'
ALTER TABLE [dbo].[tblCultura_TipoInterdependencia]
ADD CONSTRAINT [FK_tblCultura_TipoInterdependencia_tblTipoInterdependencia]
    FOREIGN KEY ([IdTipoInterdependencia])
    REFERENCES [dbo].[tblTipoInterdependencia]
        ([IdTipoInterdependencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoInterdependencia_tblTipoInterdependencia'
CREATE INDEX [IX_FK_tblCultura_TipoInterdependencia_tblTipoInterdependencia]
ON [dbo].[tblCultura_TipoInterdependencia]
    ([IdTipoInterdependencia]);
GO

-- Creating foreign key on [IdTipoRespaldo] in table 'tblCultura_TipoRespaldo'
ALTER TABLE [dbo].[tblCultura_TipoRespaldo]
ADD CONSTRAINT [FK_tblCultura_TipoRespaldo_tblTipoRespaldo]
    FOREIGN KEY ([IdTipoRespaldo])
    REFERENCES [dbo].[tblTipoRespaldo]
        ([IdTipoRespaldo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoRespaldo_tblTipoRespaldo'
CREATE INDEX [IX_FK_tblCultura_TipoRespaldo_tblTipoRespaldo]
ON [dbo].[tblCultura_TipoRespaldo]
    ([IdTipoRespaldo]);
GO

-- Creating foreign key on [IdTipoResultadoPrueba] in table 'tblCultura_TipoResultadoPrueba'
ALTER TABLE [dbo].[tblCultura_TipoResultadoPrueba]
ADD CONSTRAINT [FK_tblCultura_TipoResultadoPrueba_tblTipoResultadoPrueba]
    FOREIGN KEY ([IdTipoResultadoPrueba])
    REFERENCES [dbo].[tblTipoResultadoPrueba]
        ([IdTipoResultadoPrueba])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoResultadoPrueba_tblTipoResultadoPrueba'
CREATE INDEX [IX_FK_tblCultura_TipoResultadoPrueba_tblTipoResultadoPrueba]
ON [dbo].[tblCultura_TipoResultadoPrueba]
    ([IdTipoResultadoPrueba]);
GO

-- Creating foreign key on [IdTipoTablaContenido] in table 'tblCultura_TipoTablaContenido'
ALTER TABLE [dbo].[tblCultura_TipoTablaContenido]
ADD CONSTRAINT [FK_tblCultura_TipoTablaContenido_tblTipoTablaContenido]
    FOREIGN KEY ([IdTipoTablaContenido])
    REFERENCES [dbo].[tblTipoTablaContenido]
        ([IdTipoTablaContenido])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoTablaContenido_tblTipoTablaContenido'
CREATE INDEX [IX_FK_tblCultura_TipoTablaContenido_tblTipoTablaContenido]
ON [dbo].[tblCultura_TipoTablaContenido]
    ([IdTipoTablaContenido]);
GO

-- Creating foreign key on [IdTipoTelefono] in table 'tblCultura_TipoTelefono'
ALTER TABLE [dbo].[tblCultura_TipoTelefono]
ADD CONSTRAINT [FK_tblCultura_TipoTelefono_tblTipoTelefono]
    FOREIGN KEY ([IdTipoTelefono])
    REFERENCES [dbo].[tblTipoTelefono]
        ([IdTipoTelefono])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoTelefono_tblTipoTelefono'
CREATE INDEX [IX_FK_tblCultura_TipoTelefono_tblTipoTelefono]
ON [dbo].[tblCultura_TipoTelefono]
    ([IdTipoTelefono]);
GO

-- Creating foreign key on [IdTipoUbicacionInformacion] in table 'tblCultura_TipoUbicacionInformacion'
ALTER TABLE [dbo].[tblCultura_TipoUbicacionInformacion]
ADD CONSTRAINT [FK_tblCultura_TipoUbicacionInformacion_tblTipoUbicacionInformacion]
    FOREIGN KEY ([IdTipoUbicacionInformacion])
    REFERENCES [dbo].[tblTipoUbicacionInformacion]
        ([IdTipoUbicacionInformacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCultura_TipoUbicacionInformacion_tblTipoUbicacionInformacion'
CREATE INDEX [IX_FK_tblCultura_TipoUbicacionInformacion_tblTipoUbicacionInformacion]
ON [dbo].[tblCultura_TipoUbicacionInformacion]
    ([IdTipoUbicacionInformacion]);
GO

-- Creating foreign key on [IdFuenteIncidente] in table 'tblCulture_FuenteIncidente'
ALTER TABLE [dbo].[tblCulture_FuenteIncidente]
ADD CONSTRAINT [FK_tblCulture_FuenteIncidente_tblFuenteIncidente]
    FOREIGN KEY ([IdFuenteIncidente])
    REFERENCES [dbo].[tblFuenteIncidente]
        ([IdFuenteIncidente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCulture_FuenteIncidente_tblFuenteIncidente'
CREATE INDEX [IX_FK_tblCulture_FuenteIncidente_tblFuenteIncidente]
ON [dbo].[tblCulture_FuenteIncidente]
    ([IdFuenteIncidente]);
GO

-- Creating foreign key on [IdNaturalezaIncidente] in table 'tblCulture_NaturalezaIncidente'
ALTER TABLE [dbo].[tblCulture_NaturalezaIncidente]
ADD CONSTRAINT [FK_tblCulture_NaturalezaIncidente_tblNaturalezaIncidente]
    FOREIGN KEY ([IdNaturalezaIncidente])
    REFERENCES [dbo].[tblNaturalezaIncidente]
        ([IdNaturalezaIncidente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCulture_NaturalezaIncidente_tblNaturalezaIncidente'
CREATE INDEX [IX_FK_tblCulture_NaturalezaIncidente_tblNaturalezaIncidente]
ON [dbo].[tblCulture_NaturalezaIncidente]
    ([IdNaturalezaIncidente]);
GO

-- Creating foreign key on [IdTipoIncidente] in table 'tblCulture_TipoIncidente'
ALTER TABLE [dbo].[tblCulture_TipoIncidente]
ADD CONSTRAINT [FK_tblCulture_TipoIncidente_tblTipoIncidente]
    FOREIGN KEY ([IdTipoIncidente])
    REFERENCES [dbo].[tblTipoIncidente]
        ([IdTipoIncidente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCulture_TipoIncidente_tblTipoIncidente'
CREATE INDEX [IX_FK_tblCulture_TipoIncidente_tblTipoIncidente]
ON [dbo].[tblCulture_TipoIncidente]
    ([IdTipoIncidente]);
GO

-- Creating foreign key on [IdDispositivo] in table 'tblDispositivoConexion'
ALTER TABLE [dbo].[tblDispositivoConexion]
ADD CONSTRAINT [FK_tblDispositivoConexion_tblUsuarioDispositivo]
    FOREIGN KEY ([IdDispositivo])
    REFERENCES [dbo].[tblDispositivo]
        ([IdDispositivo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDispositivoConexion_tblUsuarioDispositivo'
CREATE INDEX [IX_FK_tblDispositivoConexion_tblUsuarioDispositivo]
ON [dbo].[tblDispositivoConexion]
    ([IdDispositivo]);
GO

-- Creating foreign key on [IdDispositivo] in table 'tblDispositivoEnvio'
ALTER TABLE [dbo].[tblDispositivoEnvio]
ADD CONSTRAINT [FK_tblDispositivoEnvio_tblDispositivo]
    FOREIGN KEY ([IdDispositivo])
    REFERENCES [dbo].[tblDispositivo]
        ([IdDispositivo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblDispositivoConexion'
ALTER TABLE [dbo].[tblDispositivoConexion]
ADD CONSTRAINT [FK_tblDispositivoConexion_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdUsuario] in table 'tblDispositivoConexion'
ALTER TABLE [dbo].[tblDispositivoConexion]
ADD CONSTRAINT [FK_tblDispositivoConexion_tblUsuario]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDispositivoConexion_tblUsuario'
CREATE INDEX [IX_FK_tblDispositivoConexion_tblUsuario]
ON [dbo].[tblDispositivoConexion]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblDocumentoAprobacion'
ALTER TABLE [dbo].[tblDocumentoAprobacion]
ADD CONSTRAINT [FK_tblAprobacion_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblDocumentoCertificacion'
ALTER TABLE [dbo].[tblDocumentoCertificacion]
ADD CONSTRAINT [FK_tblCertificacion_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblDocumento'
ALTER TABLE [dbo].[tblDocumento]
ADD CONSTRAINT [FK_tblDocumento_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEstadoDocumento] in table 'tblDocumento'
ALTER TABLE [dbo].[tblDocumento]
ADD CONSTRAINT [FK_tblDocumento_tblEstadoDocumento]
    FOREIGN KEY ([IdEstadoDocumento])
    REFERENCES [dbo].[tblEstadoDocumento]
        ([IdEstadoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDocumento_tblEstadoDocumento'
CREATE INDEX [IX_FK_tblDocumento_tblEstadoDocumento]
ON [dbo].[tblDocumento]
    ([IdEstadoDocumento]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblDocumentoAnexo'
ALTER TABLE [dbo].[tblDocumentoAnexo]
ADD CONSTRAINT [FK_tblDocumentoAnexo_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblDocumentoContenido'
ALTER TABLE [dbo].[tblDocumentoContenido]
ADD CONSTRAINT [FK_tblDocumentoContenido_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblDocumentoEntrevista'
ALTER TABLE [dbo].[tblDocumentoEntrevista]
ADD CONSTRAINT [FK_tblDocumentoEntrevista_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblDocumentoPersonaClave'
ALTER TABLE [dbo].[tblDocumentoPersonaClave]
ADD CONSTRAINT [FK_tblDocumentoPersonaClave_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblPMTProgramacionDocumentos'
ALTER TABLE [dbo].[tblPMTProgramacionDocumentos]
ADD CONSTRAINT [FK_tblPMTProgramacionDocumentos_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTProgramacionDocumentos_tblDocumento'
CREATE INDEX [IX_FK_tblPMTProgramacionDocumentos_tblDocumento]
ON [dbo].[tblPMTProgramacionDocumentos]
    ([IdEmpresa], [IdDocumento], [IdTipoDocumento]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento] in table 'tblPPEFrecuencia'
ALTER TABLE [dbo].[tblPPEFrecuencia]
ADD CONSTRAINT [FK_tblPPEFrecuencia_tblDocumento]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    REFERENCES [dbo].[tblDocumento]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPersona] in table 'tblDocumentoAprobacion'
ALTER TABLE [dbo].[tblDocumentoAprobacion]
ADD CONSTRAINT [FK_tblDocumentoAprobacion_tblPersona]
    FOREIGN KEY ([IdEmpresa], [IdPersona])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDocumentoAprobacion_tblPersona'
CREATE INDEX [IX_FK_tblDocumentoAprobacion_tblPersona]
ON [dbo].[tblDocumentoAprobacion]
    ([IdEmpresa], [IdPersona]);
GO

-- Creating foreign key on [IdEmpresa], [IdPersona] in table 'tblDocumentoCertificacion'
ALTER TABLE [dbo].[tblDocumentoCertificacion]
ADD CONSTRAINT [FK_tblCertificacion_tblPersona]
    FOREIGN KEY ([IdEmpresa], [IdPersona])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblCertificacion_tblPersona'
CREATE INDEX [IX_FK_tblCertificacion_tblPersona]
ON [dbo].[tblDocumentoCertificacion]
    ([IdEmpresa], [IdPersona]);
GO

-- Creating foreign key on [IdEmpresa], [IdSubModulo] in table 'tblDocumentoContenido'
ALTER TABLE [dbo].[tblDocumentoContenido]
ADD CONSTRAINT [FK_tblDocumentoContenido_tblModulo]
    FOREIGN KEY ([IdEmpresa], [IdSubModulo])
    REFERENCES [dbo].[tblModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDocumentoContenido_tblModulo'
CREATE INDEX [IX_FK_tblDocumentoContenido_tblModulo]
ON [dbo].[tblDocumentoContenido]
    ([IdEmpresa], [IdSubModulo]);
GO

-- Creating foreign key on [IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdEntrevista] in table 'tblDocumentoEntrevistaPersona'
ALTER TABLE [dbo].[tblDocumentoEntrevistaPersona]
ADD CONSTRAINT [FK_tblDocumentoEntrevistaPersona_tblDocumentoEntrevista]
    FOREIGN KEY ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdEntrevista])
    REFERENCES [dbo].[tblDocumentoEntrevista]
        ([IdEmpresa], [IdDocumento], [IdTipoDocumento], [IdEntrevista])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPersona] in table 'tblDocumentoPersonaClave'
ALTER TABLE [dbo].[tblDocumentoPersonaClave]
ADD CONSTRAINT [FK_tblDocumentoPersonaClave_tblPersona]
    FOREIGN KEY ([IdEmpresa], [IdPersona])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDocumentoPersonaClave_tblPersona'
CREATE INDEX [IX_FK_tblDocumentoPersonaClave_tblPersona]
ON [dbo].[tblDocumentoPersonaClave]
    ([IdEmpresa], [IdPersona]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblDocumentoPersonaClave'
ALTER TABLE [dbo].[tblDocumentoPersonaClave]
ADD CONSTRAINT [FK_tblPersonaClave_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblProbabilidadRiesgo'
ALTER TABLE [dbo].[tblProbabilidadRiesgo]
ADD CONSTRAINT [FK_tblBIAProbabilidadRiesgo_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblEscala'
ALTER TABLE [dbo].[tblEscala]
ADD CONSTRAINT [FK_tblEmpresa_tblEscala_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdPais], [IdPaisEstado] in table 'tblEmpresa'
ALTER TABLE [dbo].[tblEmpresa]
ADD CONSTRAINT [FK_tblEmpresa_tblEstado]
    FOREIGN KEY ([IdPais], [IdPaisEstado])
    REFERENCES [dbo].[tblEstado]
        ([IdPais], [IdEstado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblEmpresa_tblEstado'
CREATE INDEX [IX_FK_tblEmpresa_tblEstado]
ON [dbo].[tblEmpresa]
    ([IdPais], [IdPaisEstado]);
GO

-- Creating foreign key on [IdEstado] in table 'tblEmpresa'
ALTER TABLE [dbo].[tblEmpresa]
ADD CONSTRAINT [FK_tblEmpresa_tblEstadoEmpresa]
    FOREIGN KEY ([IdEstado])
    REFERENCES [dbo].[tblEstadoEmpresa]
        ([IdEstadoEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblEmpresa_tblEstadoEmpresa'
CREATE INDEX [IX_FK_tblEmpresa_tblEstadoEmpresa]
ON [dbo].[tblEmpresa]
    ([IdEstado]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblLocalidad'
ALTER TABLE [dbo].[tblLocalidad]
ADD CONSTRAINT [FK_tblEmpresa_tblLocalidad_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdPais] in table 'tblEmpresa'
ALTER TABLE [dbo].[tblEmpresa]
ADD CONSTRAINT [FK_tblEmpresa_tblPais]
    FOREIGN KEY ([IdPais])
    REFERENCES [dbo].[tblPais]
        ([IdPais])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblEmpresa_tblPais'
CREATE INDEX [IX_FK_tblEmpresa_tblPais]
ON [dbo].[tblEmpresa]
    ([IdPais]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPersona'
ALTER TABLE [dbo].[tblPersona]
ADD CONSTRAINT [FK_tblEmpresa_tblPersona_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblProveedor'
ALTER TABLE [dbo].[tblProveedor]
ADD CONSTRAINT [FK_tblEmpresa_tblProveedor_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblUnidadOrganizativa'
ALTER TABLE [dbo].[tblUnidadOrganizativa]
ADD CONSTRAINT [FK_tblEmpresa_tblUnidadOrganizativa_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblVicepresidencia'
ALTER TABLE [dbo].[tblVicepresidencia]
ADD CONSTRAINT [FK_tblEmpresa_tblVicepresidencia_FK1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblEmpresaModulo'
ALTER TABLE [dbo].[tblEmpresaModulo]
ADD CONSTRAINT [FK_tblEmpresaModulo_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblEmpresaUsuario'
ALTER TABLE [dbo].[tblEmpresaUsuario]
ADD CONSTRAINT [FK_tblEmpresaUsuario_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblFormatosEmail'
ALTER TABLE [dbo].[tblFormatosEmail]
ADD CONSTRAINT [FK_tblFormatosEmail_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblIncidentes'
ALTER TABLE [dbo].[tblIncidentes]
ADD CONSTRAINT [FK_tblIncidentes_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblIniciativas_Anexo'
ALTER TABLE [dbo].[tblIniciativas_Anexo]
ADD CONSTRAINT [FK_tblIniciativas_Anexo_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblIniciativas'
ALTER TABLE [dbo].[tblIniciativas]
ADD CONSTRAINT [FK_tblIniciativas_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblModulo_NivelUsuario'
ALTER TABLE [dbo].[tblModulo_NivelUsuario]
ADD CONSTRAINT [FK_tblModulo_NivelUsuario_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblModulo'
ALTER TABLE [dbo].[tblModulo]
ADD CONSTRAINT [FK_tblModulo_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblModuloAnexo'
ALTER TABLE [dbo].[tblModuloAnexo]
ADD CONSTRAINT [FK_tblModuloAnexo_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPBEPruebaEjecucion'
ALTER TABLE [dbo].[tblPBEPruebaEjecucion]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucion_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPBEPruebaEjecucionEjercicio'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicio]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicio_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPBEPruebaPlanificacion'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacion]
ADD CONSTRAINT [FK_tblPBEPruebaPlanificacion_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPlanTrabajo'
ALTER TABLE [dbo].[tblPlanTrabajo]
ADD CONSTRAINT [FK_tblPlanTrabajo_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPlanTrabajoAccion'
ALTER TABLE [dbo].[tblPlanTrabajoAccion]
ADD CONSTRAINT [FK_tblPlanTrabajoAccion_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPMTMensajeActualizacion'
ALTER TABLE [dbo].[tblPMTMensajeActualizacion]
ADD CONSTRAINT [FK_tblPMTMensajeActualizacion_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPMTProgramacion'
ALTER TABLE [dbo].[tblPMTProgramacion]
ADD CONSTRAINT [FK_tblPMTProgramacion_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTProgramacion_tblEmpresa'
CREATE INDEX [IX_FK_tblPMTProgramacion_tblEmpresa]
ON [dbo].[tblPMTProgramacion]
    ([IdEmpresa]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblPMTResponsableUpdate'
ALTER TABLE [dbo].[tblPMTResponsableUpdate]
ADD CONSTRAINT [FK_tblPMTResponsableUpdate_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblProducto'
ALTER TABLE [dbo].[tblProducto]
ADD CONSTRAINT [FK_tblProducto_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa] in table 'tblUsuarioUnidadOrganizativa'
ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa]
ADD CONSTRAINT [FK_tblUsuarioUnidadOrganizativa_tblEmpresa]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdSubModulo] in table 'tblDispositivoEnvio1'
ALTER TABLE [dbo].[tblDispositivoEnvio1]
ADD CONSTRAINT [FK_tblDispositivoEnvio_tblEmpresaModulo]
    FOREIGN KEY ([IdEmpresa], [IdSubModulo])
    REFERENCES [dbo].[tblEmpresaModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDispositivoEnvio_tblEmpresaModulo'
CREATE INDEX [IX_FK_tblDispositivoEnvio_tblEmpresaModulo]
ON [dbo].[tblDispositivoEnvio1]
    ([IdEmpresa], [IdSubModulo]);
GO

-- Creating foreign key on [IdEmpresa], [IdModulo] in table 'tblEmpresaModulo'
ALTER TABLE [dbo].[tblEmpresaModulo]
ADD CONSTRAINT [FK_tblEmpresaModulo_tblModulo]
    FOREIGN KEY ([IdEmpresa], [IdModulo])
    REFERENCES [dbo].[tblModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdNivelUsuario] in table 'tblEmpresaUsuario'
ALTER TABLE [dbo].[tblEmpresaUsuario]
ADD CONSTRAINT [FK_tblEmpresaUsuario_tblNivelUsuario]
    FOREIGN KEY ([IdNivelUsuario])
    REFERENCES [dbo].[tblNivelUsuario]
        ([IdNivelUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblEmpresaUsuario_tblNivelUsuario'
CREATE INDEX [IX_FK_tblEmpresaUsuario_tblNivelUsuario]
ON [dbo].[tblEmpresaUsuario]
    ([IdNivelUsuario]);
GO

-- Creating foreign key on [IdUsuario] in table 'tblEmpresaUsuario'
ALTER TABLE [dbo].[tblEmpresaUsuario]
ADD CONSTRAINT [FK_tblEmpresaUsuario_tblUsuario]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblEmpresaUsuario_tblUsuario'
CREATE INDEX [IX_FK_tblEmpresaUsuario_tblUsuario]
ON [dbo].[tblEmpresaUsuario]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdEmpresa], [IdTipoEscala] in table 'tblEscala'
ALTER TABLE [dbo].[tblEscala]
ADD CONSTRAINT [FK_tblEscala_tblTipoEscala]
    FOREIGN KEY ([IdEmpresa], [IdTipoEscala])
    REFERENCES [dbo].[tblTipoEscala]
        ([IdEmpresa], [IdTipoEscala])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblEscala_tblTipoEscala'
CREATE INDEX [IX_FK_tblEscala_tblTipoEscala]
ON [dbo].[tblEscala]
    ([IdEmpresa], [IdTipoEscala]);
GO

-- Creating foreign key on [IdPais], [IdEstado] in table 'tblLocalidad'
ALTER TABLE [dbo].[tblLocalidad]
ADD CONSTRAINT [FK_tblLocalidad_tblEstado]
    FOREIGN KEY ([IdPais], [IdEstado])
    REFERENCES [dbo].[tblEstado]
        ([IdPais], [IdEstado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblLocalidad_tblEstado'
CREATE INDEX [IX_FK_tblLocalidad_tblEstado]
ON [dbo].[tblLocalidad]
    ([IdPais], [IdEstado]);
GO

-- Creating foreign key on [IdPais] in table 'tblEstado'
ALTER TABLE [dbo].[tblEstado]
ADD CONSTRAINT [FK_tblPais_tblEstado_FK1]
    FOREIGN KEY ([IdPais])
    REFERENCES [dbo].[tblPais]
        ([IdPais])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdPais], [IdEstado] in table 'tblPersonaDireccion'
ALTER TABLE [dbo].[tblPersonaDireccion]
ADD CONSTRAINT [FK_tblPersonaDireccion_tblEstado]
    FOREIGN KEY ([IdPais], [IdEstado])
    REFERENCES [dbo].[tblEstado]
        ([IdPais], [IdEstado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersonaDireccion_tblEstado'
CREATE INDEX [IX_FK_tblPersonaDireccion_tblEstado]
ON [dbo].[tblPersonaDireccion]
    ([IdPais], [IdEstado]);
GO

-- Creating foreign key on [EstadoUsuario] in table 'tblUsuario'
ALTER TABLE [dbo].[tblUsuario]
ADD CONSTRAINT [FK_tblUsuario_tblEstadoUsuario]
    FOREIGN KEY ([EstadoUsuario])
    REFERENCES [dbo].[tblEstadoUsuario]
        ([IdEstadoUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblUsuario_tblEstadoUsuario'
CREATE INDEX [IX_FK_tblUsuario_tblEstadoUsuario]
ON [dbo].[tblUsuario]
    ([EstadoUsuario]);
GO

-- Creating foreign key on [IdEmpresa], [IdPrioridad] in table 'tblIniciativas'
ALTER TABLE [dbo].[tblIniciativas]
ADD CONSTRAINT [FK_tblIniciativas_tblIniciativaPrioridad]
    FOREIGN KEY ([IdEmpresa], [IdPrioridad])
    REFERENCES [dbo].[tblIniciativaPrioridad]
        ([IdEmpresa], [IdPrioridad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblIniciativas_tblIniciativaPrioridad'
CREATE INDEX [IX_FK_tblIniciativas_tblIniciativaPrioridad]
ON [dbo].[tblIniciativas]
    ([IdEmpresa], [IdPrioridad]);
GO

-- Creating foreign key on [IdEstatusIniciativa] in table 'tblIniciativas'
ALTER TABLE [dbo].[tblIniciativas]
ADD CONSTRAINT [FK_tblIniciativas_tblPlanTrabajoEstatus]
    FOREIGN KEY ([IdEstatusIniciativa])
    REFERENCES [dbo].[tblPlanTrabajoEstatus]
        ([IdEstatusActividad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblIniciativas_tblPlanTrabajoEstatus'
CREATE INDEX [IX_FK_tblIniciativas_tblPlanTrabajoEstatus]
ON [dbo].[tblIniciativas]
    ([IdEstatusIniciativa]);
GO

-- Creating foreign key on [IdPais] in table 'tblLocalidad'
ALTER TABLE [dbo].[tblLocalidad]
ADD CONSTRAINT [FK_tblLocalidad_tblPais]
    FOREIGN KEY ([IdPais])
    REFERENCES [dbo].[tblPais]
        ([IdPais])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblLocalidad_tblPais'
CREATE INDEX [IX_FK_tblLocalidad_tblPais]
ON [dbo].[tblLocalidad]
    ([IdPais]);
GO

-- Creating foreign key on [IdEmpresa], [IdModulo] in table 'tblModulo_NivelUsuario'
ALTER TABLE [dbo].[tblModulo_NivelUsuario]
ADD CONSTRAINT [FK_tblModulo_NivelUsuario_tblModulo]
    FOREIGN KEY ([IdEmpresa], [IdModulo])
    REFERENCES [dbo].[tblModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblModulo_NivelUsuario_tblModulo'
CREATE INDEX [IX_FK_tblModulo_NivelUsuario_tblModulo]
ON [dbo].[tblModulo_NivelUsuario]
    ([IdEmpresa], [IdModulo]);
GO

-- Creating foreign key on [IdEmpresa], [IdModulo] in table 'tblModulo_Usuario'
ALTER TABLE [dbo].[tblModulo_Usuario]
ADD CONSTRAINT [FK_tblModulo_Usuario_tblModulo]
    FOREIGN KEY ([IdEmpresa], [IdModulo])
    REFERENCES [dbo].[tblModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdModulo] in table 'tblPMTMensajeActualizacion'
ALTER TABLE [dbo].[tblPMTMensajeActualizacion]
ADD CONSTRAINT [FK_tblPMTMensajeActualizacion_tblModulo]
    FOREIGN KEY ([IdEmpresa], [IdModulo])
    REFERENCES [dbo].[tblModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTMensajeActualizacion_tblModulo'
CREATE INDEX [IX_FK_tblPMTMensajeActualizacion_tblModulo]
ON [dbo].[tblPMTMensajeActualizacion]
    ([IdEmpresa], [IdModulo]);
GO

-- Creating foreign key on [IdEmpresa], [IdModulo] in table 'tblPMTProgramacion'
ALTER TABLE [dbo].[tblPMTProgramacion]
ADD CONSTRAINT [FK_tblPMTProgramacion_tblModulo]
    FOREIGN KEY ([IdEmpresa], [IdModulo])
    REFERENCES [dbo].[tblModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTProgramacion_tblModulo'
CREATE INDEX [IX_FK_tblPMTProgramacion_tblModulo]
ON [dbo].[tblPMTProgramacion]
    ([IdEmpresa], [IdModulo]);
GO

-- Creating foreign key on [IdEmpresa], [IdModulo] in table 'tblPMTProgramacionDocumentos'
ALTER TABLE [dbo].[tblPMTProgramacionDocumentos]
ADD CONSTRAINT [FK_tblPMTProgramacionDocumentos_tblModulo]
    FOREIGN KEY ([IdEmpresa], [IdModulo])
    REFERENCES [dbo].[tblModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTProgramacionDocumentos_tblModulo'
CREATE INDEX [IX_FK_tblPMTProgramacionDocumentos_tblModulo]
ON [dbo].[tblPMTProgramacionDocumentos]
    ([IdEmpresa], [IdModulo]);
GO

-- Creating foreign key on [IdEmpresa], [IdModulo] in table 'tblPMTResponsableUpdate'
ALTER TABLE [dbo].[tblPMTResponsableUpdate]
ADD CONSTRAINT [FK_tblPMTResponsableUpdate_tblModulo]
    FOREIGN KEY ([IdEmpresa], [IdModulo])
    REFERENCES [dbo].[tblModulo]
        ([IdEmpresa], [IdModulo])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdNivelUsuario] in table 'tblModulo_NivelUsuario'
ALTER TABLE [dbo].[tblModulo_NivelUsuario]
ADD CONSTRAINT [FK_tblModulo_NivelUsuario_tblNivelUsuario]
    FOREIGN KEY ([IdNivelUsuario])
    REFERENCES [dbo].[tblNivelUsuario]
        ([IdNivelUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblModulo_NivelUsuario_tblNivelUsuario'
CREATE INDEX [IX_FK_tblModulo_NivelUsuario_tblNivelUsuario]
ON [dbo].[tblModulo_NivelUsuario]
    ([IdNivelUsuario]);
GO

-- Creating foreign key on [IdUsuario] in table 'tblModulo_Usuario'
ALTER TABLE [dbo].[tblModulo_Usuario]
ADD CONSTRAINT [FK_tblModulo_Usuario_tblUsuario]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblModulo_Usuario_tblUsuario'
CREATE INDEX [IX_FK_tblModulo_Usuario_tblUsuario]
ON [dbo].[tblModulo_Usuario]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdNivelUsuario] in table 'tblUsuarioUnidadOrganizativa'
ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa]
ADD CONSTRAINT [FK_tblUsuarioUnidadOrganizativa_tblNivelUsuario]
    FOREIGN KEY ([IdNivelUsuario])
    REFERENCES [dbo].[tblNivelUsuario]
        ([IdNivelUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblUsuarioUnidadOrganizativa_tblNivelUsuario'
CREATE INDEX [IX_FK_tblUsuarioUnidadOrganizativa_tblNivelUsuario]
ON [dbo].[tblUsuarioUnidadOrganizativa]
    ([IdNivelUsuario]);
GO

-- Creating foreign key on [IdPais] in table 'tblPersonaDireccion'
ALTER TABLE [dbo].[tblPersonaDireccion]
ADD CONSTRAINT [FK_tblPersonaDireccion_tblPais]
    FOREIGN KEY ([IdPais])
    REFERENCES [dbo].[tblPais]
        ([IdPais])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersonaDireccion_tblPais'
CREATE INDEX [IX_FK_tblPersonaDireccion_tblPais]
ON [dbo].[tblPersonaDireccion]
    ([IdPais]);
GO

-- Creating foreign key on [IdPais] in table 'tblVicepresidencia'
ALTER TABLE [dbo].[tblVicepresidencia]
ADD CONSTRAINT [FK_tblVicepresidencia_tblPais]
    FOREIGN KEY ([IdPais])
    REFERENCES [dbo].[tblPais]
        ([IdPais])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblVicepresidencia_tblPais'
CREATE INDEX [IX_FK_tblVicepresidencia_tblPais]
ON [dbo].[tblVicepresidencia]
    ([IdPais]);
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaEjecucion'
ALTER TABLE [dbo].[tblPBEPruebaEjecucion]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucion_tblPBEPruebaPlanificacion]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion])
    REFERENCES [dbo].[tblPBEPruebaPlanificacion]
        ([IdEmpresa], [IdPlanificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaEjecucionEjercicio'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicio]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicio_tblPBEPruebaEjecucion]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion])
    REFERENCES [dbo].[tblPBEPruebaEjecucion]
        ([IdEmpresa], [IdPlanificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaEjecucionParticipante'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionParticipante]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucionParticipante_tblPBEPruebaEjecucion]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion])
    REFERENCES [dbo].[tblPBEPruebaEjecucion]
        ([IdEmpresa], [IdPlanificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaEjecucionResultado'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionResultado]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucionResultado_tblPBEPruebaEjecucion]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion])
    REFERENCES [dbo].[tblPBEPruebaEjecucion]
        ([IdEmpresa], [IdPlanificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEstatus] in table 'tblPBEPruebaEjecucionEjercicio'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicio]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicio_tblPBEPruebaEjecucionEstatus]
    FOREIGN KEY ([IdEstatus])
    REFERENCES [dbo].[tblPBEPruebaEstatus]
        ([IdEstatus])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPBEPruebaEjecucionEjercicio_tblPBEPruebaEjecucionEstatus'
CREATE INDEX [IX_FK_tblPBEPruebaEjecucionEjercicio_tblPBEPruebaEjecucionEstatus]
ON [dbo].[tblPBEPruebaEjecucionEjercicio]
    ([IdEstatus]);
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion], [IdEjercicio] in table 'tblPBEPruebaEjecucionEjercicioParticipante'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicioParticipante]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicioParticipante_tblPBEPruebaEjecucionEjercicio]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion], [IdEjercicio])
    REFERENCES [dbo].[tblPBEPruebaEjecucionEjercicio]
        ([IdEmpresa], [IdPlanificacion], [IdEjercicio])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion], [IdEjercicio] in table 'tblPBEPruebaEjecucionEjercicioRecurso'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicioRecurso]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicioRecurso_tblPBEPruebaEjecucionEjercicio]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion], [IdEjercicio])
    REFERENCES [dbo].[tblPBEPruebaEjecucionEjercicio]
        ([IdEmpresa], [IdPlanificacion], [IdEjercicio])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion], [IdParticipante] in table 'tblPBEPruebaEjecucionEjercicioParticipante'
ALTER TABLE [dbo].[tblPBEPruebaEjecucionEjercicioParticipante]
ADD CONSTRAINT [FK_tblPBEPruebaEjecucionEjercicioParticipante_tblPBEPruebaEjecucionParticipante]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion], [IdParticipante])
    REFERENCES [dbo].[tblPBEPruebaEjecucionParticipante]
        ([IdEmpresa], [IdPlanificacion], [IdParticipante])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPBEPruebaEjecucionEjercicioParticipante_tblPBEPruebaEjecucionParticipante'
CREATE INDEX [IX_FK_tblPBEPruebaEjecucionEjercicioParticipante_tblPBEPruebaEjecucionParticipante]
ON [dbo].[tblPBEPruebaEjecucionEjercicioParticipante]
    ([IdEmpresa], [IdPlanificacion], [IdParticipante]);
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaPlanificacionEjercicio'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicio]
ADD CONSTRAINT [FK_tblPBEPruebaPlanificacionEjercicio_tblPBEPruebaPlanificacion]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion])
    REFERENCES [dbo].[tblPBEPruebaPlanificacion]
        ([IdEmpresa], [IdPlanificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaPlanificacionParticipante'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionParticipante]
ADD CONSTRAINT [FK_tblPBEPruebaPlanificacionParticipante_tblPBEPruebaPlanificacion]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion])
    REFERENCES [dbo].[tblPBEPruebaPlanificacion]
        ([IdEmpresa], [IdPlanificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion] in table 'tblPBEPruebaPlanificacionEjercicioRecurso'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioRecurso]
ADD CONSTRAINT [FK_tblPBEPruebaPlanificacionRecurso_tblPBEPruebaPlanificacion]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion])
    REFERENCES [dbo].[tblPBEPruebaPlanificacion]
        ([IdEmpresa], [IdPlanificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion], [IdEjercicio] in table 'tblPBEPruebaPlanificacionEjercicioParticipante'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioParticipante]
ADD CONSTRAINT [FK_tblPBEPruebaPlanificacionEjercicioParticipante_tblPBEPruebaPlanificacionEjercicio]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion], [IdEjercicio])
    REFERENCES [dbo].[tblPBEPruebaPlanificacionEjercicio]
        ([IdEmpresa], [IdPlanificacion], [IdEjercicio])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion], [IdEjercicio] in table 'tblPBEPruebaPlanificacionEjercicioRecurso'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioRecurso]
ADD CONSTRAINT [FK_tblPBEPruebaPlanificacionEjercicioRecurso_tblPBEPruebaPlanificacionEjercicio]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion], [IdEjercicio])
    REFERENCES [dbo].[tblPBEPruebaPlanificacionEjercicio]
        ([IdEmpresa], [IdPlanificacion], [IdEjercicio])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPlanificacion], [IdParticipante] in table 'tblPBEPruebaPlanificacionEjercicioParticipante'
ALTER TABLE [dbo].[tblPBEPruebaPlanificacionEjercicioParticipante]
ADD CONSTRAINT [FK_tblPBEPruebaPlanificacionEjercicioParticipante_tblPBEPruebaPlanificacionParticipante]
    FOREIGN KEY ([IdEmpresa], [IdPlanificacion], [IdParticipante])
    REFERENCES [dbo].[tblPBEPruebaPlanificacionParticipante]
        ([IdEmpresa], [IdPlanificacion], [IdParticipante])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPBEPruebaPlanificacionEjercicioParticipante_tblPBEPruebaPlanificacionParticipante'
CREATE INDEX [IX_FK_tblPBEPruebaPlanificacionEjercicioParticipante_tblPBEPruebaPlanificacionParticipante]
ON [dbo].[tblPBEPruebaPlanificacionEjercicioParticipante]
    ([IdEmpresa], [IdPlanificacion], [IdParticipante]);
GO

-- Creating foreign key on [IdEmpresa], [IdPersona] in table 'tblPersonaDireccion'
ALTER TABLE [dbo].[tblPersonaDireccion]
ADD CONSTRAINT [FK_tblPersona_tblPersonaDireccion_FK1]
    FOREIGN KEY ([IdEmpresa], [IdPersona])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdPersona] in table 'tblPersonaTelefono'
ALTER TABLE [dbo].[tblPersonaTelefono]
ADD CONSTRAINT [FK_tblPersona_tblPersonaTelefono_FK1]
    FOREIGN KEY ([IdEmpresa], [IdPersona])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdUnidadOrganizativa] in table 'tblPersona'
ALTER TABLE [dbo].[tblPersona]
ADD CONSTRAINT [FK_tblPersona_tblUnidadOrganizativa]
    FOREIGN KEY ([IdEmpresa], [IdUnidadOrganizativa])
    REFERENCES [dbo].[tblUnidadOrganizativa]
        ([IdEmpresa], [IdUnidadOrganizativa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersona_tblUnidadOrganizativa'
CREATE INDEX [IX_FK_tblPersona_tblUnidadOrganizativa]
ON [dbo].[tblPersona]
    ([IdEmpresa], [IdUnidadOrganizativa]);
GO

-- Creating foreign key on [IdUsuario] in table 'tblPersona'
ALTER TABLE [dbo].[tblPersona]
ADD CONSTRAINT [FK_tblPersona_tblUsuario]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersona_tblUsuario'
CREATE INDEX [IX_FK_tblPersona_tblUsuario]
ON [dbo].[tblPersona]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdEmpresa], [IdPersona] in table 'tblPersonaCorreo'
ALTER TABLE [dbo].[tblPersonaCorreo]
ADD CONSTRAINT [FK_tblPersonaCorreo_tblPersona]
    FOREIGN KEY ([IdEmpresa], [IdPersona])
    REFERENCES [dbo].[tblPersona]
        ([IdEmpresa], [IdPersona])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdTipoCorreo] in table 'tblPersonaCorreo'
ALTER TABLE [dbo].[tblPersonaCorreo]
ADD CONSTRAINT [FK_tblPersonaCorreo_tblTipoCorreo]
    FOREIGN KEY ([IdTipoCorreo])
    REFERENCES [dbo].[tblTipoCorreo]
        ([IdTipoCorreo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersonaCorreo_tblTipoCorreo'
CREATE INDEX [IX_FK_tblPersonaCorreo_tblTipoCorreo]
ON [dbo].[tblPersonaCorreo]
    ([IdTipoCorreo]);
GO

-- Creating foreign key on [IdTipoDireccion] in table 'tblPersonaDireccion'
ALTER TABLE [dbo].[tblPersonaDireccion]
ADD CONSTRAINT [FK_tblPersonaDireccion_tblTipoDireccion]
    FOREIGN KEY ([IdTipoDireccion])
    REFERENCES [dbo].[tblTipoDireccion]
        ([IdTipoDireccion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersonaDireccion_tblTipoDireccion'
CREATE INDEX [IX_FK_tblPersonaDireccion_tblTipoDireccion]
ON [dbo].[tblPersonaDireccion]
    ([IdTipoDireccion]);
GO

-- Creating foreign key on [IdTipoTelefono] in table 'tblPersonaTelefono'
ALTER TABLE [dbo].[tblPersonaTelefono]
ADD CONSTRAINT [FK_tblPersonaTelefono_tblTipoTelefono]
    FOREIGN KEY ([IdTipoTelefono])
    REFERENCES [dbo].[tblTipoTelefono]
        ([IdTipoTelefono])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPersonaTelefono_tblTipoTelefono'
CREATE INDEX [IX_FK_tblPersonaTelefono_tblTipoTelefono]
ON [dbo].[tblPersonaTelefono]
    ([IdTipoTelefono]);
GO

-- Creating foreign key on [IdEmpresa], [IdAccion] in table 'tblPlanTrabajo'
ALTER TABLE [dbo].[tblPlanTrabajo]
ADD CONSTRAINT [FK_tblPlanTrabajo_tblPlanTrabajoAccion]
    FOREIGN KEY ([IdEmpresa], [IdAccion])
    REFERENCES [dbo].[tblPlanTrabajoAccion]
        ([IdEmpresa], [IdPlanAccion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Ejecutada] in table 'tblPlanTrabajo'
ALTER TABLE [dbo].[tblPlanTrabajo]
ADD CONSTRAINT [FK_tblPlanTrabajo_tblPlanTrabajoEstatus]
    FOREIGN KEY ([Ejecutada])
    REFERENCES [dbo].[tblPlanTrabajoEstatus]
        ([IdEstatusActividad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPlanTrabajo_tblPlanTrabajoEstatus'
CREATE INDEX [IX_FK_tblPlanTrabajo_tblPlanTrabajoEstatus]
ON [dbo].[tblPlanTrabajo]
    ([Ejecutada]);
GO

-- Creating foreign key on [IdEmpresa], [IdAccion], [IdActividad] in table 'tblPlanTrabajoAuditoria'
ALTER TABLE [dbo].[tblPlanTrabajoAuditoria]
ADD CONSTRAINT [FK_tblPlanTrabajoAuditoria_tblPlanTrabajo]
    FOREIGN KEY ([IdEmpresa], [IdAccion], [IdActividad])
    REFERENCES [dbo].[tblPlanTrabajo]
        ([IdEmpresa], [IdAccion], [IdActividad])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdEmpresa], [IdAccion] in table 'tblPlanTrabajoAuditoria'
ALTER TABLE [dbo].[tblPlanTrabajoAuditoria]
ADD CONSTRAINT [FK_tblPlanTrabajoAuditoria_tblPlanTrabajoAccion]
    FOREIGN KEY ([IdEmpresa], [IdAccion])
    REFERENCES [dbo].[tblPlanTrabajoAccion]
        ([IdEmpresa], [IdPlanAccion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Estado] in table 'tblPlanTrabajoAuditoria'
ALTER TABLE [dbo].[tblPlanTrabajoAuditoria]
ADD CONSTRAINT [FK_tblPlanTrabajoAuditoria_tblPlanTrabajoEstatus]
    FOREIGN KEY ([Estado])
    REFERENCES [dbo].[tblPlanTrabajoEstatus]
        ([IdEstatusActividad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPlanTrabajoAuditoria_tblPlanTrabajoEstatus'
CREATE INDEX [IX_FK_tblPlanTrabajoAuditoria_tblPlanTrabajoEstatus]
ON [dbo].[tblPlanTrabajoAuditoria]
    ([Estado]);
GO

-- Creating foreign key on [IdUsuarioCambioEstado] in table 'tblPlanTrabajoAuditoria'
ALTER TABLE [dbo].[tblPlanTrabajoAuditoria]
ADD CONSTRAINT [FK_tblPlanTrabajoAuditoria_tblUsuario]
    FOREIGN KEY ([IdUsuarioCambioEstado])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPlanTrabajoAuditoria_tblUsuario'
CREATE INDEX [IX_FK_tblPlanTrabajoAuditoria_tblUsuario]
ON [dbo].[tblPlanTrabajoAuditoria]
    ([IdUsuarioCambioEstado]);
GO

-- Creating foreign key on [IdTipoActualizacion] in table 'tblPMTProgramacion'
ALTER TABLE [dbo].[tblPMTProgramacion]
ADD CONSTRAINT [FK_tblPMTProgramacion_tblPMTProgramacionTipoActualizacion]
    FOREIGN KEY ([IdTipoActualizacion])
    REFERENCES [dbo].[tblPMTProgramacionTipoActualizacion]
        ([IdTipoActualizacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTProgramacion_tblPMTProgramacionTipoActualizacion'
CREATE INDEX [IX_FK_tblPMTProgramacion_tblPMTProgramacionTipoActualizacion]
ON [dbo].[tblPMTProgramacion]
    ([IdTipoActualizacion]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblPMTProgramacion'
ALTER TABLE [dbo].[tblPMTProgramacion]
ADD CONSTRAINT [FK_tblPMTProgramacion_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTProgramacion_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblPMTProgramacion_tblTipoFrecuencia]
ON [dbo].[tblPMTProgramacion]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdPMTProgramacion] in table 'tblPMTProgramacionDocumentos'
ALTER TABLE [dbo].[tblPMTProgramacionDocumentos]
ADD CONSTRAINT [FK_tblPMTProgramacionDocumentos_tblPMTProgramacion]
    FOREIGN KEY ([IdPMTProgramacion])
    REFERENCES [dbo].[tblPMTProgramacion]
        ([IdPMTProgramacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdPMTProgramacion] in table 'tblPMTProgramacionUsuario'
ALTER TABLE [dbo].[tblPMTProgramacionUsuario]
ADD CONSTRAINT [FK_tblPMTProgramacionUsuario_tblPMTProgramacion]
    FOREIGN KEY ([IdPMTProgramacion])
    REFERENCES [dbo].[tblPMTProgramacion]
        ([IdPMTProgramacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdTipoNotificacion] in table 'tblPMTProgramacionUsuario'
ALTER TABLE [dbo].[tblPMTProgramacionUsuario]
ADD CONSTRAINT [FK_tblPMTProgramacionUsuario_tblPMTProgramacionTipoNotificacion]
    FOREIGN KEY ([IdTipoNotificacion])
    REFERENCES [dbo].[tblPMTProgramacionTipoNotificacion]
        ([IdTipoNotificacion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTProgramacionUsuario_tblPMTProgramacionTipoNotificacion'
CREATE INDEX [IX_FK_tblPMTProgramacionUsuario_tblPMTProgramacionTipoNotificacion]
ON [dbo].[tblPMTProgramacionUsuario]
    ([IdTipoNotificacion]);
GO

-- Creating foreign key on [IdUsuario] in table 'tblPMTProgramacionUsuario'
ALTER TABLE [dbo].[tblPMTProgramacionUsuario]
ADD CONSTRAINT [FK_tblPMTProgramacionUsuario_tblUsuario]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTProgramacionUsuario_tblUsuario'
CREATE INDEX [IX_FK_tblPMTProgramacionUsuario_tblUsuario]
ON [dbo].[tblPMTProgramacionUsuario]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdEmpresa], [IdModulo], [IdMensaje] in table 'tblPMTResponsableUpdate_Correo'
ALTER TABLE [dbo].[tblPMTResponsableUpdate_Correo]
ADD CONSTRAINT [FK_tblPMTResponsableUpdate_Correo_tblPMTResponsableUpdate]
    FOREIGN KEY ([IdEmpresa], [IdModulo], [IdMensaje])
    REFERENCES [dbo].[tblPMTResponsableUpdate]
        ([IdEmpresa], [IdModulo], [IdMensaje])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdUsuario] in table 'tblPMTResponsableUpdate_Correo'
ALTER TABLE [dbo].[tblPMTResponsableUpdate_Correo]
ADD CONSTRAINT [FK_tblPMTResponsableUpdate_Correo_tblUsuario]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPMTResponsableUpdate_Correo_tblUsuario'
CREATE INDEX [IX_FK_tblPMTResponsableUpdate_Correo_tblUsuario]
ON [dbo].[tblPMTResponsableUpdate_Correo]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdTipoFrecuencia] in table 'tblPPEFrecuencia'
ALTER TABLE [dbo].[tblPPEFrecuencia]
ADD CONSTRAINT [FK_tblPPEFrecuencia_tblTipoFrecuencia]
    FOREIGN KEY ([IdTipoFrecuencia])
    REFERENCES [dbo].[tblTipoFrecuencia]
        ([IdTipoFrecuencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblPPEFrecuencia_tblTipoFrecuencia'
CREATE INDEX [IX_FK_tblPPEFrecuencia_tblTipoFrecuencia]
ON [dbo].[tblPPEFrecuencia]
    ([IdTipoFrecuencia]);
GO

-- Creating foreign key on [IdEmpresa], [IdUnidadOrganizativa] in table 'tblUsuarioUnidadOrganizativa'
ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa]
ADD CONSTRAINT [FK_tblUsuarioUnidadOrganizativa_tblUnidadOrganizativa]
    FOREIGN KEY ([IdEmpresa], [IdUnidadOrganizativa])
    REFERENCES [dbo].[tblUnidadOrganizativa]
        ([IdEmpresa], [IdUnidadOrganizativa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdUsuario] in table 'tblDispositivo1'
ALTER TABLE [dbo].[tblDispositivo1]
ADD CONSTRAINT [FK_tblDispositivo_tblUsuario]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDispositivo_tblUsuario'
CREATE INDEX [IX_FK_tblDispositivo_tblUsuario]
ON [dbo].[tblDispositivo1]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdUsuarioEnvia] in table 'tblDispositivoEnvio1'
ALTER TABLE [dbo].[tblDispositivoEnvio1]
ADD CONSTRAINT [FK_tblDispositivoEnvio_tblUsuarioEnvia]
    FOREIGN KEY ([IdUsuarioEnvia])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDispositivoEnvio_tblUsuarioEnvia'
CREATE INDEX [IX_FK_tblDispositivoEnvio_tblUsuarioEnvia]
ON [dbo].[tblDispositivoEnvio1]
    ([IdUsuarioEnvia]);
GO

-- Creating foreign key on [IdUsuario] in table 'tblDispositivoEnvio1'
ALTER TABLE [dbo].[tblDispositivoEnvio1]
ADD CONSTRAINT [FK_tblDispositivoEnvio_tblUsuarioRecibe]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDispositivoEnvio_tblUsuarioRecibe'
CREATE INDEX [IX_FK_tblDispositivoEnvio_tblUsuarioRecibe]
ON [dbo].[tblDispositivoEnvio1]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdUsuario] in table 'tblUsuarioUnidadOrganizativa'
ALTER TABLE [dbo].[tblUsuarioUnidadOrganizativa]
ADD CONSTRAINT [FK_tblUsuarioUnidadOrganizativa_tblUsuario]
    FOREIGN KEY ([IdUsuario])
    REFERENCES [dbo].[tblUsuario]
        ([IdUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblUsuarioUnidadOrganizativa_tblUsuario'
CREATE INDEX [IX_FK_tblUsuarioUnidadOrganizativa_tblUsuario]
ON [dbo].[tblUsuarioUnidadOrganizativa]
    ([IdUsuario]);
GO

-- Creating foreign key on [IdDispositivo] in table 'tblDispositivoEnvio1'
ALTER TABLE [dbo].[tblDispositivoEnvio1]
ADD CONSTRAINT [FK_tblDispositivoEnvio_tblDispositivo1]
    FOREIGN KEY ([IdDispositivo])
    REFERENCES [dbo].[tblDispositivo1]
        ([IdDispositivo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdFuenteIncidente] in table 'tblIncidentes'
ALTER TABLE [dbo].[tblIncidentes]
ADD CONSTRAINT [FK_tblIncidentes_tblFuenteIncidente]
    FOREIGN KEY ([IdFuenteIncidente])
    REFERENCES [dbo].[tblFuenteIncidente]
        ([IdFuenteIncidente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblIncidentes_tblFuenteIncidente'
CREATE INDEX [IX_FK_tblIncidentes_tblFuenteIncidente]
ON [dbo].[tblIncidentes]
    ([IdFuenteIncidente]);
GO

-- Creating foreign key on [IdNaturalezaIncidente] in table 'tblIncidentes'
ALTER TABLE [dbo].[tblIncidentes]
ADD CONSTRAINT [FK_tblIncidentes_tblNaturalezaIncidente]
    FOREIGN KEY ([IdNaturalezaIncidente])
    REFERENCES [dbo].[tblNaturalezaIncidente]
        ([IdNaturalezaIncidente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblIncidentes_tblNaturalezaIncidente'
CREATE INDEX [IX_FK_tblIncidentes_tblNaturalezaIncidente]
ON [dbo].[tblIncidentes]
    ([IdNaturalezaIncidente]);
GO

-- Creating foreign key on [IdTipoIncidente] in table 'tblIncidentes'
ALTER TABLE [dbo].[tblIncidentes]
ADD CONSTRAINT [FK_tblIncidentes_tblTipoIncidente]
    FOREIGN KEY ([IdTipoIncidente])
    REFERENCES [dbo].[tblTipoIncidente]
        ([IdTipoIncidente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblIncidentes_tblTipoIncidente'
CREATE INDEX [IX_FK_tblIncidentes_tblTipoIncidente]
ON [dbo].[tblIncidentes]
    ([IdTipoIncidente]);
GO

-- Creating foreign key on [IdEmpresa] in table 'tblDispositivoConexion1'
ALTER TABLE [dbo].[tblDispositivoConexion1]
ADD CONSTRAINT [FK_tblDispositivoConexion_tblEmpresa1]
    FOREIGN KEY ([IdEmpresa])
    REFERENCES [dbo].[tblEmpresa]
        ([IdEmpresa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tblDispositivoConexion_tblEmpresa1'
CREATE INDEX [IX_FK_tblDispositivoConexion_tblEmpresa1]
ON [dbo].[tblDispositivoConexion1]
    ([IdEmpresa]);
GO

-- Creating foreign key on [IdDispositivo] in table 'tblDispositivoConexion1'
ALTER TABLE [dbo].[tblDispositivoConexion1]
ADD CONSTRAINT [FK_tblDispositivoConexion_tblDispositivo]
    FOREIGN KEY ([IdDispositivo])
    REFERENCES [dbo].[tblDispositivo1]
        ([IdDispositivo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------