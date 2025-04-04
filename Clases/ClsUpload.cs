using Examen2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace Examen2.Clases
{
    public class ClsUpload
    {
        public string datos { get; set; }
        public string proceso { get; set; }
        public HttpRequestMessage request { get; set; }
        private DBexamenEntities DBexamen = new DBexamenEntities();
        private string Archivos { get; set; }
        public async Task<HttpResponseMessage> GrabarArchivo(int id, string placa)
        {
            if (!request.Content.IsMimeMultipartContent())
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "No se envio un archivo para procesar");
            }
            string root = HttpContext.Current.Server.MapPath("~/Archivos");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                bool existe = false;
                await request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileData.Count > 0)
                {
                    
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        //lee el contenido del archivo
                        
                        string nombreArchivo = file.Headers.ContentDisposition.FileName;
                        if (nombreArchivo.StartsWith("\"") && nombreArchivo.EndsWith("\""))
                        {
                            nombreArchivo = nombreArchivo.Trim('"');
                        }
                        if (nombreArchivo.Contains(@"/") || nombreArchivo.Contains(@"\"))
                        {
                            nombreArchivo = Path.GetFileName(nombreArchivo);
                        }
                        if (File.Exists(Path.Combine(root, nombreArchivo)))
                        {
                            //el archivo ya existe en el servidor, no se va a cargar, se va a cargar al temporal y se devolvera un error
                            File.Delete(Path.Combine(root, file.LocalFileName));
                            existe = true;
                            //return request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "el archivo ya existe");
                        }
                        else
                        {
                            existe = false;
                            //agrego en una lista el nombre de los archivos
                            Archivos=nombreArchivo;
                            GrabarImagenPesaje(id, placa, Archivos);

                            //renombra el archivo temporal
                            File.Move(file.LocalFileName, Path.Combine(root, nombreArchivo));
                        }
                    }
                    if (!existe)
                    {
                        //se genera el proceso de gestion en la base de datos
                       
                        //terminar el ciclo, responde que se cargo el archivo correctamente
                        return request.CreateResponse(System.Net.HttpStatusCode.OK, "Archivo cargado correctamente");
                    }
                    else
                    {
                        return request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "el archivo ya existe");
                    }
                }
                else
                {
                    return request.CreateErrorResponse(System.Net.HttpStatusCode.Conflict, "No se envio un archivo para rocesar");
                }
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "Error al procesar el archivo: " + ex.Message);
            }
        }
        public string GrabarImagenPesaje(int id, string placa, string nom)
        {
            try
            {
                FotoPesaje foto = new FotoPesaje();

                foto.ImagenVehiculo = nom;
                foto.idFotoPesaje = id;
                foto.idPesaje = DBexamen.Pesajes.FirstOrDefault(x => x.PlacaCamion == placa).id;
                DBexamen.FotoPesajes.Add(foto);
                DBexamen.SaveChanges();

                return "Imagenes guardadas correctamente en la base de datos";
            }
            catch (Exception e)
            {
                return "Error al guardar la imagen: " + e.Message;
            }

        }
    }
}
