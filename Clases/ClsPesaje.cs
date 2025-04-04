using Examen2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examen2.Clases
{
	public class ClsPesaje
	{
        private DBexamenEntities DBexamen = new DBexamenEntities();
        public Pesaje pesaje { get; set; }

        public string insertar(int id, string placa, float peso, string estacion)
        {
            try
            {
                pesaje = new Pesaje();
                pesaje.id = id;
                pesaje.FechaPesaje = DateTime.Now;
                pesaje.PlacaCamion = placa;
                pesaje.Peso = peso;
                pesaje.Estacion = estacion;
                DBexamen.Pesajes.Add(pesaje);
                DBexamen.SaveChanges();
                return "Pesaje registrado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al registrar el pesaje: " + ex.Message;
            }
        }
        public IQueryable consultarXPlaca(string placa)
        {
            return from c in DBexamen.Set<Camion>()
                       join p in DBexamen.Set<Pesaje>() on c.Placa equals p.PlacaCamion
                       //join i in DBexamen.Set<FotoPesaje>() on p.id equals i.idFotoPesaje
                   where p.PlacaCamion == placa
                   select new
                   {
                       c.Placa,
                       c.Marca,
                       c.NumeroEjes,
                       p.FechaPesaje,
                       p.Peso,
                       //i.ImagenVehiculo
                   };
        }
    }
}