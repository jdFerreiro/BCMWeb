using System.Collections.Generic;

namespace BCMWebApp
{
    public static class GlobalVars
    {
        public const string _serverUrl = "https://www.bcmweb.net";
        //public const string _serverUrl = "http://localhost:39641";

        public static long UserId { get; set; }
        public static string UserName { get; set; }
        public static string Usertoken { get; set; }
        public static string UserCode { get; set; }
        public static string UserPassw { get; set; }
        public static long IdEmpresaSelected { get; set; }
        public static List<Empresa> BotonEmpresa { get; set; }
        public static void Clear()
        {
            UserId = 0;
            UserName = string.Empty;
            Usertoken = string.Empty;
            UserCode = string.Empty;
            UserPassw = string.Empty;
            IdEmpresaSelected = 0;
            BotonEmpresa = new List<Empresa>();
        }
    }
}
