using BCMWeb.APIModels;
using BCMWeb.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BCMWeb.APIController
{
    [Authorize]
    [RoutePrefix("api/evento")]
    public class EventosController : ApiController
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

        public EventosController()
        {
            string[] _userLanguages = HttpContext.Current.Request.UserLanguages;
            Culture = (_userLanguages == null || _userLanguages.Count() == 0 ? "es-VE" : _userLanguages[0]);
        }

        [HttpGet]
        [Route("getdispositivos")]
        [ResponseType(typeof(List<DispositivoRegistradoModel>))]
        public async Task<IHttpActionResult> GetDispositivos()
        {

            List<DispositivoRegistradoModel> data = await db.tblDispositivo1.Select(x => new DispositivoRegistradoModel
            {
                Fabricante = x.Fabricante,
                FechaRegistro = x.FechaRegistro,
                Id = x.IdDispositivo,
                IMEI_Dispositivo = x.IMEI_Dispositivo,
                Modelo = x.Modelo,
                Nombre = x.NombreDispositivo,
                Plataforma = x.Plataforma,
                Tipo = x.TipoDispositivo,
                Version = x.Version
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

            tblDispositivo1 dispositivo = await db.tblDispositivo1
                .FirstOrDefaultAsync(x => x.IMEI_Dispositivo == deviceId);

            if (dispositivo == null)
            {
                return 0;
            }

            return dispositivo.IdDispositivo;
        }
        [Route("GetEnviosPendientes/{deviceId:int}")]
        [HttpGet]
        public async Task<List<EnvioPendienteModel>> GetDocsDevice(long deviceId)
        {

            List<tblDispositivoEnvio1> data = await db.tblDispositivoEnvio1
                .Where(x => x.IdDispositivo == deviceId && x.Descargado == false).ToListAsync();

            List<EnvioPendienteModel> envios = data
                .Select(x => new EnvioPendienteModel
                {
                    IdDocumento = x.IdSubModulo,
                    IdEmpresa = x.IdEmpresa,
                    NombreDocumento = GetNombreDocumento(x.IdSubModulo),
                    RutaDocumento = GetRutaPDF(x.IdEmpresa, x.IdSubModulo)
                }).ToList();

            return envios ?? new List<EnvioPendienteModel>();
        }
        [Route("addDevice")]
        [HttpPost]
        [ResponseType(typeof(DispositivoRegistradoModel))]
        public async Task<IHttpActionResult> PostDispositivo(DispositivoRegistradoModel reg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (reg.IdUsuario != null)
                {
                    tblDispositivo1 _tblDispositivo = new tblDispositivo1
                    {
                        Fabricante = reg.Fabricante,
                        FechaRegistro = DateTime.Now,
                        IdDispositivo = reg.Id,
                        IMEI_Dispositivo = reg.IMEI_Dispositivo,
                        Modelo = reg.Modelo,
                        NombreDispositivo = reg.Nombre,
                        Plataforma = reg.Plataforma,
                        TipoDispositivo = reg.Tipo,
                        Version = reg.Version,
                        IdUsuario = (long)reg.IdUsuario,
                        TokenDispositivo = reg.Token,
                    };

                    db.tblDispositivo1.Add(_tblDispositivo);
                    await db.SaveChangesAsync();
                    reg.Id =_tblDispositivo.IdDispositivo;
                }
                else
                {
                    tblDispositivo _tblDispositivo = new tblDispositivo
                    {
                        fabricante = reg.Fabricante,
                        fechaRegistro = DateTime.Now,
                        IdDispositivo = reg.Id,
                        IdUnicoDispositivo = reg.IMEI_Dispositivo,
                        modelo = reg.Modelo,
                        nombre = reg.Nombre,
                        plataforma = reg.Plataforma,
                        tipo = reg.Tipo,
                        version = reg.Version,
                        token = reg.Token,
                    };
                    await db.SaveChangesAsync();
                    reg.Id = _tblDispositivo.IdDispositivo;
                }
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
        [ResponseType(typeof(DispositivoRegistradoConexion))]
        public async Task<IHttpActionResult> PostConexion(DispositivoRegistradoConexion reg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (reg.IdEmpresa != null)
                {
                    tblDispositivoConexion tblDispositivoConexion = new tblDispositivoConexion
                    {
                        DireccionIP = reg.DireccionIP,
                        fechaConexion = DateTime.Now,
                        IdDispositivo = reg.IdDispositivo,
                        IdEmpresa = reg.IdEmpresa ?? 0,
                        IdUsuario = reg.IdUsuario,
                    };
                    db.tblDispositivoConexion.Add(tblDispositivoConexion);
                }
                else
                {
                    tblDispositivoConexion1 tblDispositivoConexion = new tblDispositivoConexion1
                    {
                        DirecciónIP = reg.DireccionIP,
                        FechaConexion = DateTime.Now,
                        IdDispositivo = reg.IdDispositivo,
                        IdEmpresa = reg.IdEmpresa,
                        IdUsuario = reg.IdUsuario,
                        IdSubModulo = reg.IdSubModulo
                    };
                    db.tblDispositivoConexion1.Add(tblDispositivoConexion);
                }

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
