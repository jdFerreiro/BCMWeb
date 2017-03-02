﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BCMWeb.Data.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblAuditoria> tblAuditoria { get; set; }
        public virtual DbSet<tblAuditoriaProcesoCritico> tblAuditoriaProcesoCritico { get; set; }
        public virtual DbSet<tblBCPDocumento> tblBCPDocumento { get; set; }
        public virtual DbSet<tblBCPReanudacionPersonaClave> tblBCPReanudacionPersonaClave { get; set; }
        public virtual DbSet<tblBCPReanudacionTarea> tblBCPReanudacionTarea { get; set; }
        public virtual DbSet<tblBCPReanudacionTareaActividad> tblBCPReanudacionTareaActividad { get; set; }
        public virtual DbSet<tblBCPRecuperacionPersonaClave> tblBCPRecuperacionPersonaClave { get; set; }
        public virtual DbSet<tblBCPRecuperacionRecurso> tblBCPRecuperacionRecurso { get; set; }
        public virtual DbSet<tblBCPRespuestaAccion> tblBCPRespuestaAccion { get; set; }
        public virtual DbSet<tblBCPRespuestaRecurso> tblBCPRespuestaRecurso { get; set; }
        public virtual DbSet<tblBCPRestauracionEquipo> tblBCPRestauracionEquipo { get; set; }
        public virtual DbSet<tblBCPRestauracionInfraestructura> tblBCPRestauracionInfraestructura { get; set; }
        public virtual DbSet<tblBCPRestauracionMobiliario> tblBCPRestauracionMobiliario { get; set; }
        public virtual DbSet<tblBCPRestauracionOtro> tblBCPRestauracionOtro { get; set; }
        public virtual DbSet<tblBIAAmenaza> tblBIAAmenaza { get; set; }
        public virtual DbSet<tblBIAAmenazaEvento> tblBIAAmenazaEvento { get; set; }
        public virtual DbSet<tblBIAAplicacion> tblBIAAplicacion { get; set; }
        public virtual DbSet<tblBIACadenaServicio> tblBIACadenaServicio { get; set; }
        public virtual DbSet<tblBIAClienteProducto> tblBIAClienteProducto { get; set; }
        public virtual DbSet<tblBIAComentario> tblBIAComentario { get; set; }
        public virtual DbSet<tblBIAControlRiesgo> tblBIAControlRiesgo { get; set; }
        public virtual DbSet<tblBIADocumentacion> tblBIADocumentacion { get; set; }
        public virtual DbSet<tblBIADocumento> tblBIADocumento { get; set; }
        public virtual DbSet<tblBIAEntrada> tblBIAEntrada { get; set; }
        public virtual DbSet<tblBIAEstadoRiesgo> tblBIAEstadoRiesgo { get; set; }
        public virtual DbSet<tblBIAEventoControl> tblBIAEventoControl { get; set; }
        public virtual DbSet<tblBIAEventoRiesgo> tblBIAEventoRiesgo { get; set; }
        public virtual DbSet<tblBIAFuenteRiesgo> tblBIAFuenteRiesgo { get; set; }
        public virtual DbSet<tblBIAGranImpacto> tblBIAGranImpacto { get; set; }
        public virtual DbSet<tblBIAImpactoFinanciero> tblBIAImpactoFinanciero { get; set; }
        public virtual DbSet<tblBIAImpactoOperacional> tblBIAImpactoOperacional { get; set; }
        public virtual DbSet<tblBIAInterdependencia> tblBIAInterdependencia { get; set; }
        public virtual DbSet<tblBIAMTD> tblBIAMTD { get; set; }
        public virtual DbSet<tblBIAPersonaRespaldoProceso> tblBIAPersonaRespaldoProceso { get; set; }
        public virtual DbSet<tblBIAProbabilidadRiesgo> tblBIAProbabilidadRiesgo { get; set; }
        public virtual DbSet<tblBIAProceso> tblBIAProceso { get; set; }
        public virtual DbSet<tblBIAProcesoAlterno> tblBIAProcesoAlterno { get; set; }
        public virtual DbSet<tblBIAProveedor> tblBIAProveedor { get; set; }
        public virtual DbSet<tblBIARespaldoPrimario> tblBIARespaldoPrimario { get; set; }
        public virtual DbSet<tblBIARespaldoSecundario> tblBIARespaldoSecundario { get; set; }
        public virtual DbSet<tblBIARPO> tblBIARPO { get; set; }
        public virtual DbSet<tblBIARTO> tblBIARTO { get; set; }
        public virtual DbSet<tblBIAUnidadTrabajo> tblBIAUnidadTrabajo { get; set; }
        public virtual DbSet<tblBIAUnidadTrabajoPersonas> tblBIAUnidadTrabajoPersonas { get; set; }
        public virtual DbSet<tblBIAUnidadTrabajoProceso> tblBIAUnidadTrabajoProceso { get; set; }
        public virtual DbSet<tblCargo> tblCargo { get; set; }
        public virtual DbSet<tblCiudad> tblCiudad { get; set; }
        public virtual DbSet<tblCultura_Ciudad> tblCultura_Ciudad { get; set; }
        public virtual DbSet<tblCultura_Estado> tblCultura_Estado { get; set; }
        public virtual DbSet<tblCultura_EstadoDocumento> tblCultura_EstadoDocumento { get; set; }
        public virtual DbSet<tblCultura_EstadoEmpresa> tblCultura_EstadoEmpresa { get; set; }
        public virtual DbSet<tblCultura_EstadoProceso> tblCultura_EstadoProceso { get; set; }
        public virtual DbSet<tblCultura_EstadoUsuario> tblCultura_EstadoUsuario { get; set; }
        public virtual DbSet<tblCultura_Mes> tblCultura_Mes { get; set; }
        public virtual DbSet<tblCultura_NivelImpacto> tblCultura_NivelImpacto { get; set; }
        public virtual DbSet<tblCultura_NivelUsuario> tblCultura_NivelUsuario { get; set; }
        public virtual DbSet<tblCultura_Pais> tblCultura_Pais { get; set; }
        public virtual DbSet<tblCultura_PlanTrabajoEstatus> tblCultura_PlanTrabajoEstatus { get; set; }
        public virtual DbSet<tblCultura_TipoCorreo> tblCultura_TipoCorreo { get; set; }
        public virtual DbSet<tblCultura_TipoDireccion> tblCultura_TipoDireccion { get; set; }
        public virtual DbSet<tblCultura_TipoEscala> tblCultura_TipoEscala { get; set; }
        public virtual DbSet<tblCultura_TipoFrecuencia> tblCultura_TipoFrecuencia { get; set; }
        public virtual DbSet<tblCultura_TipoImpacto> tblCultura_TipoImpacto { get; set; }
        public virtual DbSet<tblCultura_TipoInterdependencia> tblCultura_TipoInterdependencia { get; set; }
        public virtual DbSet<tblCultura_TipoRespaldo> tblCultura_TipoRespaldo { get; set; }
        public virtual DbSet<tblCultura_TipoResultadoPrueba> tblCultura_TipoResultadoPrueba { get; set; }
        public virtual DbSet<tblCultura_TipoTablaContenido> tblCultura_TipoTablaContenido { get; set; }
        public virtual DbSet<tblCultura_TipoTelefono> tblCultura_TipoTelefono { get; set; }
        public virtual DbSet<tblCultura_TipoUbicacionInformacion> tblCultura_TipoUbicacionInformacion { get; set; }
        public virtual DbSet<tblDocumento> tblDocumento { get; set; }
        public virtual DbSet<tblDocumentoAnexo> tblDocumentoAnexo { get; set; }
        public virtual DbSet<tblDocumentoAprobacion> tblDocumentoAprobacion { get; set; }
        public virtual DbSet<tblDocumentoAuditoria> tblDocumentoAuditoria { get; set; }
        public virtual DbSet<tblDocumentoCertificacion> tblDocumentoCertificacion { get; set; }
        public virtual DbSet<tblDocumentoContenido> tblDocumentoContenido { get; set; }
        public virtual DbSet<tblDocumentoEntrevista> tblDocumentoEntrevista { get; set; }
        public virtual DbSet<tblDocumentoEntrevistaPersona> tblDocumentoEntrevistaPersona { get; set; }
        public virtual DbSet<tblDocumentoPersonaClave> tblDocumentoPersonaClave { get; set; }
        public virtual DbSet<tblEmpresa> tblEmpresa { get; set; }
        public virtual DbSet<tblEmpresaModulo> tblEmpresaModulo { get; set; }
        public virtual DbSet<tblEmpresaUsuario> tblEmpresaUsuario { get; set; }
        public virtual DbSet<tblEscala> tblEscala { get; set; }
        public virtual DbSet<tblEstado> tblEstado { get; set; }
        public virtual DbSet<tblEstadoDocumento> tblEstadoDocumento { get; set; }
        public virtual DbSet<tblEstadoEmpresa> tblEstadoEmpresa { get; set; }
        public virtual DbSet<tblEstadoProceso> tblEstadoProceso { get; set; }
        public virtual DbSet<tblEstadoUsuario> tblEstadoUsuario { get; set; }
        public virtual DbSet<tblFrecuencia> tblFrecuencia { get; set; }
        public virtual DbSet<tblImpactoRiesgo> tblImpactoRiesgo { get; set; }
        public virtual DbSet<tblLocalidad> tblLocalidad { get; set; }
        public virtual DbSet<tblMes> tblMes { get; set; }
        public virtual DbSet<tblModulo> tblModulo { get; set; }
        public virtual DbSet<tblModulo_NivelUsuario> tblModulo_NivelUsuario { get; set; }
        public virtual DbSet<tblModuloAnexo> tblModuloAnexo { get; set; }
        public virtual DbSet<tblNivelImpacto> tblNivelImpacto { get; set; }
        public virtual DbSet<tblNivelUsuario> tblNivelUsuario { get; set; }
        public virtual DbSet<tblPais> tblPais { get; set; }
        public virtual DbSet<tblPBEPruebaEjecucionEjercicio> tblPBEPruebaEjecucionEjercicio { get; set; }
        public virtual DbSet<tblPBEPruebaEjecucionParticipante> tblPBEPruebaEjecucionParticipante { get; set; }
        public virtual DbSet<tblPBEPruebaEjecucionParticipanteCorreo> tblPBEPruebaEjecucionParticipanteCorreo { get; set; }
        public virtual DbSet<tblPBEPruebaEjecucionParticipanteTelefono> tblPBEPruebaEjecucionParticipanteTelefono { get; set; }
        public virtual DbSet<tblPBEPruebaEjecucionRecurso> tblPBEPruebaEjecucionRecurso { get; set; }
        public virtual DbSet<tblPBEPruebaEjecucionResultado> tblPBEPruebaEjecucionResultado { get; set; }
        public virtual DbSet<tblPBEPruebaPlanificacion> tblPBEPruebaPlanificacion { get; set; }
        public virtual DbSet<tblPBEPruebaPlanificacionEjercicio> tblPBEPruebaPlanificacionEjercicio { get; set; }
        public virtual DbSet<tblPBEPruebaPlanificacionParticipante> tblPBEPruebaPlanificacionParticipante { get; set; }
        public virtual DbSet<tblPBEPruebaPlanificacionParticipanteCorreo> tblPBEPruebaPlanificacionParticipanteCorreo { get; set; }
        public virtual DbSet<tblPBEPruebaPlanificacionParticipanteTelefono> tblPBEPruebaPlanificacionParticipanteTelefono { get; set; }
        public virtual DbSet<tblPBEPruebaPlanificacionRecurso> tblPBEPruebaPlanificacionRecurso { get; set; }
        public virtual DbSet<tblPersona> tblPersona { get; set; }
        public virtual DbSet<tblPersonaCorreo> tblPersonaCorreo { get; set; }
        public virtual DbSet<tblPersonaDireccion> tblPersonaDireccion { get; set; }
        public virtual DbSet<tblPersonaTelefono> tblPersonaTelefono { get; set; }
        public virtual DbSet<tblPlanTrabajo> tblPlanTrabajo { get; set; }
        public virtual DbSet<tblPlanTrabajoAccion> tblPlanTrabajoAccion { get; set; }
        public virtual DbSet<tblPlanTrabajoAuditoria> tblPlanTrabajoAuditoria { get; set; }
        public virtual DbSet<tblPlanTrabajoEstatus> tblPlanTrabajoEstatus { get; set; }
        public virtual DbSet<tblPMTFrecuencia> tblPMTFrecuencia { get; set; }
        public virtual DbSet<tblPMTFrecuenciaParticipante> tblPMTFrecuenciaParticipante { get; set; }
        public virtual DbSet<tblPMTMensajeActualizacion> tblPMTMensajeActualizacion { get; set; }
        public virtual DbSet<tblPMTProgramacion> tblPMTProgramacion { get; set; }
        public virtual DbSet<tblPMTProgramacionAuditoria> tblPMTProgramacionAuditoria { get; set; }
        public virtual DbSet<tblPMTResponsableUpdate> tblPMTResponsableUpdate { get; set; }
        public virtual DbSet<tblPMTResponsableUpdate_Correo> tblPMTResponsableUpdate_Correo { get; set; }
        public virtual DbSet<tblPPEFrecuencia> tblPPEFrecuencia { get; set; }
        public virtual DbSet<tblProducto> tblProducto { get; set; }
        public virtual DbSet<tblProveedor> tblProveedor { get; set; }
        public virtual DbSet<tblTipoCorreo> tblTipoCorreo { get; set; }
        public virtual DbSet<tblTipoDireccion> tblTipoDireccion { get; set; }
        public virtual DbSet<tblTipoEscala> tblTipoEscala { get; set; }
        public virtual DbSet<tblTipoFrecuencia> tblTipoFrecuencia { get; set; }
        public virtual DbSet<tblTipoImpacto> tblTipoImpacto { get; set; }
        public virtual DbSet<tblTipoInterdependencia> tblTipoInterdependencia { get; set; }
        public virtual DbSet<tblTipoRespaldo> tblTipoRespaldo { get; set; }
        public virtual DbSet<tblTipoResultadoPrueba> tblTipoResultadoPrueba { get; set; }
        public virtual DbSet<tblTipoTablaContenido> tblTipoTablaContenido { get; set; }
        public virtual DbSet<tblTipoTelefono> tblTipoTelefono { get; set; }
        public virtual DbSet<tblTipoUbicacionInformacion> tblTipoUbicacionInformacion { get; set; }
        public virtual DbSet<tblUnidadOrganizativa> tblUnidadOrganizativa { get; set; }
        public virtual DbSet<tblUsuario> tblUsuario { get; set; }
        public virtual DbSet<tblUsuarioUnidadOrganizativa> tblUsuarioUnidadOrganizativa { get; set; }
        public virtual DbSet<tblVicepresidencia> tblVicepresidencia { get; set; }
        public virtual DbSet<tblBIAWRT> tblBIAWRT { get; set; }
    }
}
