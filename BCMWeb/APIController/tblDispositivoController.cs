using BCMWeb.APIModels;
using BCMWeb.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BCMWeb.APIController
{
    [Authorize]
    [RoutePrefix("api/Device")]
    public class tblDispositivoController : ApiController
    {
        private Entities db = new Entities();
        private string Culture;

        private string GetRutaPDF(long IdEmpresa, long idSubModulo)
        {
            string _ruta;

            long IdDocumento = db.tblDocumento
                .Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoDocumento == 6 && x.IdEstadoDocumento == 6)
                .OrderByDescending(x => x.FechaEstadoDocumento).FirstOrDefaultAsync().Id;

            string orden = ((idSubModulo - 6050200) + 6).ToString("00");
            _ruta = string.Format("https://www.bcmweb.net/pdfdocs/PMI_{0}_{1}_{2}_{3}.pdf"
                , IdEmpresa.ToString("000"), orden, IdDocumento.ToString("000"), idSubModulo.ToString("00000000"));

            return _ruta;
        }
        private string GetNombreDocumento(long idSubModulo)
        {
            string Nombre = string.Empty;

            Nombre = string.Format("Que hacer en caso de {0}", db.tblModulo.FirstOrDefault(x => x.IdModulo == idSubModulo).Nombre);

            return Nombre;
        }

        public tblDispositivoController()
        {
            string[] _userLanguages = HttpContext.Current.Request.UserLanguages;
            Culture = (_userLanguages == null || _userLanguages.Count() == 0 ? "es-VE" : _userLanguages[0]);
        }

        [HttpGet]
        [Route("getall")]
        [ResponseType(typeof(List<DispositivoModel>))]
        public async Task<IHttpActionResult> GetDispositivos()
        {

            List<DispositivoModel> data = await db.tblDispositivo.Select(x => new DispositivoModel
            {
                fabricante = x.fabricante,
                FechaRegistro = x.fechaRegistro,
                Id = x.IdDispositivo,
                IdUnicoDispositivo = x.IdUnicoDispositivo,
                modelo = x.modelo,
                nombre = x.nombre,
                plataforma = x.plataforma,
                tipo = x.Tipo,
                version = x.version
            }).ToListAsync();


            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [Route("ExistDevice/{deviceId}")]
        [HttpGet]
        public async Task<long> ExistDevice(string deviceId)
        {

            tblDispositivo dispositivo = await db.tblDispositivo
                .FirstOrDefaultAsync(x => x.IdUnicoDispositivo == deviceId);

            if (dispositivo == null)
            {
                return 0;
            }

            return dispositivo.IdDispositivo;
        }
        [Route("GetDocsPendientes/{deviceId:int}")]
        [HttpGet]
        public async Task<List<DocumentosPendientes>> GetDocsDevice(long deviceId)
        {

            List<tblDispositivoEnvio> data = await db.tblDispositivoEnvio
                .Where(x => x.IdDispositivo == deviceId && (x.Descargado ?? false) == false).ToListAsync();

            List<DocumentosPendientes> envios = data
                .Select(x => new DocumentosPendientes
                {
                    IdDocumento = x.IdSubModulo,
                    IdEmpresa = x.IdEmpresa ?? 0,
                    NombreDocumento = GetNombreDocumento(x.IdSubModulo),
                    RutaDocumento = GetRutaPDF(x.IdEmpresa ?? 0, x.IdSubModulo)
                }).ToList();

            return envios ?? new List<DocumentosPendientes>();
        }
        [Route("addDevice")]
        [HttpPost]
        [ResponseType(typeof(DispositivoModel))]
        public async Task<IHttpActionResult> PostDispositivo(DispositivoModel reg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tblDispositivo tblDispositivo = new tblDispositivo
            {
                fabricante = reg.fabricante,
                fechaRegistro = DateTime.Now,
                IdDispositivo = reg.Id,
                IdUnicoDispositivo = reg.IdUnicoDispositivo,
                modelo = reg.modelo,
                nombre = reg.nombre,
                plataforma = reg.plataforma,
                Tipo = reg.tipo,
                version = reg.version,
            };

            try
            {
                db.tblDispositivo.Add(tblDispositivo);
                await db.SaveChangesAsync();
                reg.Id = tblDispositivo.IdDispositivo;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest(msg);
            }
            return Ok(reg);
        }
        [Route("addConexion")]
        [HttpPost]
        [ResponseType(typeof(DispositivoConexion))]
        public async Task<IHttpActionResult> PostConexion(DispositivoConexion reg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tblDispositivoConexion tblDispositivoConexion = new tblDispositivoConexion
            {
                DireccionIP = reg.DireccionIP,
                fechaConexion = DateTime.Now,
                IdDispositivo = reg.IdDispositivo,
                IdEmpresa = reg.IdEmpresa,
                IdUsuario = reg.IdUsuario,
            };

            try
            {
                db.tblDispositivoConexion.Add(tblDispositivoConexion);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest(msg);
            }
            return Ok(reg);
        }

    }
}
