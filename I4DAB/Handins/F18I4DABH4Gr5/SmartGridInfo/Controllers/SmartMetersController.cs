﻿using System;
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
using SmartGridInfo.Models;

namespace SmartGridInfo.Controllers
{
    public class SmartMetersController : ApiController
    {
        private SmartGridInfoContext db = new SmartGridInfoContext();

        // GET: api/SmartMeters
        public IQueryable<SmartMeter> GetSmartMeters()
        {
            return db.SmartMeters;
        }

        // GET: api/SmartMeters/5
        [ResponseType(typeof(SmartMeter))]
        public async Task<IHttpActionResult> GetSmartMeter(string id)
        {
            SmartMeter smartMeter = await db.SmartMeters.FindAsync(id);
            if (smartMeter == null)
            {
                return NotFound();
            }

            return Ok(smartMeter);
        }

        // PUT: api/SmartMeters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSmartMeter(string id, SmartMeter smartMeter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != smartMeter.SerialNumber)
            {
                return BadRequest();
            }

            db.Entry(smartMeter).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmartMeterExists(id))
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

        // POST: api/SmartMeters
        [ResponseType(typeof(SmartMeter))]
        public async Task<IHttpActionResult> PostSmartMeter(SmartMeter smartMeter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SmartMeters.Add(smartMeter);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SmartMeterExists(smartMeter.SerialNumber))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = smartMeter.SerialNumber }, smartMeter);
        }

        // DELETE: api/SmartMeters/5
        [ResponseType(typeof(SmartMeter))]
        public async Task<IHttpActionResult> DeleteSmartMeter(string id)
        {
            SmartMeter smartMeter = await db.SmartMeters.FindAsync(id);
            if (smartMeter == null)
            {
                return NotFound();
            }

            db.SmartMeters.Remove(smartMeter);
            await db.SaveChangesAsync();

            return Ok(smartMeter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SmartMeterExists(string id)
        {
            return db.SmartMeters.Count(e => e.SerialNumber == id) > 0;
        }
    }
}