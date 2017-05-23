﻿using BCMWeb.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{
    public class CriticoModel : ModulosUserModel
    {
        public string ImpactoFinancieroSelected  { get; set; }
        public string ImpactoOperacionalSelected { get; set; }
        public string MTDSelected { get; set; }
        public string RTOSelected { get; set; }
        public string RPOSelected { get; set; }
        public string WRTSelected { get; set; }
        public IList<DocumentoProcesoModel> Procesos { get; set; }
    }
    public class ProcesoImpactoModel : ModulosUserModel
    {
        public long IdProceso { get; set; }
        public long IdDocumentoBIA { get; set; }
        public long IdImpacto { get; set; }
        public int NroProceso { get; set; }
        public string NombreProceso { get; set; }
        public string Impacto { get; set; }
        public long IdTipoEscala { get; set; }
    }
}