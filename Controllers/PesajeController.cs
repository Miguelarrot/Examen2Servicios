using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<HttpResponseMessage> insertar(HttpRequestMessage request, string Datos,string placa, string marca, int numeroEjes, int id, float peso, string estacion, int idFoto)
        {          
            ClsVehiculo camion = new ClsVehiculo();
            if (!camion.consultar(placa))
            {
                camion.insertar(placa, marca, numeroEjes);
                Ok(new { Mensaje = "Camion ingresado por primera vez con exito"});
            }
            ClsPesaje pesaje = new ClsPesaje();          
            pesaje.insertar(id, placa, peso, estacion);
            Ok(new { Mensaje = "Camion ingresado por primera vez con exito" });
            ClsUpload upload = new ClsUpload();
            upload.datos = Datos;
            upload.request = request;
            return await upload.GrabarArchivo(idFoto, placa);
        }
    }
}