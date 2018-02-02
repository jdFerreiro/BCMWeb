using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BCMWeb.Data.EF;

namespace BCMWeb.APIController
{
    public class tblModuloController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/tblModulo
        public IQueryable<tblModulo> GettblModulo()
        {
            return db.tblModulo;
        }

        // GET: api/tblModulo/5
        [ResponseType(typeof(tblModulo))]
        public async Task<IHttpActionResult> GettblModulo(long id)
        {
            tblModulo tblModulo = await db.tblModulo.FindAsync(id);
            if (tblModulo == null)
            {
                return NotFound();
            }

            return Ok(tblModulo);
        }

        // PUT: api/tblModulo/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PuttblModulo(long id, tblModulo tblModulo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblModulo.IdEmpresa)
            {
                return BadRequest();
            }

            db.Entry(tblModulo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblModuloExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tblModulo
        [ResponseType(typeof(tblModulo))]
        public async Task<IHttpActionResult> PosttblModulo(tblModulo tblModulo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tblModulo.Add(tblModulo);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tblModuloExists(tblModulo.IdEmpresa))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tblModulo.IdEmpresa }, tblModulo);
        }

        // DELETE: api/tblModulo/5
        [ResponseType(typeof(tblModulo))]
        public async Task<IHttpActionResult> DeletetblModulo(long id)
        {
            tblModulo tblModulo = await db.tblModulo.FindAsync(id);
            if (tblModulo == null)
            {
                return NotFound();
            }

            db.tblModulo.Remove(tblModulo);
            await db.SaveChangesAsync();

            return Ok(tblModulo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tblModuloExists(long id)
        {
            return db.tblModulo.Count(e => e.IdEmpresa == id) > 0;
        }
    }
}