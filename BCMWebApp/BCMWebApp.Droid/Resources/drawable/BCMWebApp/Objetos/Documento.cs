namespace BCMWebApp
{
    public class Documento
    {
        public long Id { get; set; }
        public long IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public long NroDocumento { get; set; }
        public bool Negocios { get; set; }
        public int NroVersion { get; set; }
        public int VersionOriginal { get; set; }
        public string PdfRoute { get; set; }

        public Documento()
        {

            this.IdTipoDocumento = 0;
            this.Id = 0;
            this.NroDocumento = 0;
            this.TipoDocumento = string.Empty;
            this.PdfRoute = string.Empty;
            this.Negocios = false;
            this.NroVersion = 0;
            this.VersionOriginal = 0;

        }
    }
}

