using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Antlr.Runtime.Misc;
using Examen2.Clases;
using Examen2.Models;

namespace Examen2.Controllers
{
    [RoutePrefix("api/pesaje")]
    public class PesajeController : ApiController
    {
        [HttpGet]
        [Route("consultarXPlaca")]
      public IQueryable consultarXPlaca(string placa)
        {
            ClsPesaje Pesaje = new ClsPesaje();
            return Pesaje.consultarXPlaca(placa);
        }
        [HttpPost]
        [Route("insertar")]
        public string insertar([FromBody] pesajeRequest pesajeRequest)
        {
            
            ClsVehiculo camion = new ClsVehiculo();
            if (!camion.consultar(pesajeRequest.placa))
            {
                camion.insertar(pesajeRequest.placa, pesajeRequest.marca, pesajeRequest.numeroEjes);
                Ok(new { Mensaje = "Camion ingresado por primera vez con exito"});
            }
            ClsPesaje pesaje = new ClsPesaje();
            return pesaje.insertar(pesajeRequest.id, pesajeRequest.placa, pesajeRequest.peso, pesajeRequest.estacion);

        }
    }
}