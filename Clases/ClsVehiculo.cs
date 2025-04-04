using Examen2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Examen2.Clases
{
	
	public class ClsVehiculo
	{
		private DBexamenEntities DBexamen = new DBexamenEntities();
		public Camion camion { get; set; }

		public string insertar(string placa, string marca, int num)
		{
			try
			{
                camion = new Camion();
                camion.Placa = placa;
                camion.Marca = marca;
                camion.NumeroEjes = num;
                DBexamen.Camions.Add(camion);
                DBexamen.SaveChanges();
                return "Vehiculo registrado correctamente";
            }
            catch (Exception ex)
			{
                return "Error al registrar el vehiculo: " + ex.Message;
            }
        }

		public bool consultar(string placa)
		{
			Camion camion = DBexamen.Camions.FirstOrDefault(x => x.Placa == placa);
			if(camion != null)
                return true;
            else
                return false;
        }
    }
}