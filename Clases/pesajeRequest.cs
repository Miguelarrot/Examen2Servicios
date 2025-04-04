using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examen2.Clases
{
	public class pesajeRequest
	{
		public string placa { get; set; }
		public string marca { get; set; }
		public int numeroEjes { get; set; }
		public int id { get; set; }
        public float peso { get; set; }
        public string estacion { get; set; }
    }
}