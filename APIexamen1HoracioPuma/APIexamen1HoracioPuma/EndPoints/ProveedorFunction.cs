using APIexamen1HoracioPuma.Contratos.Repositorios;
using APIexamen1HoracioPuma.Modelo.Implementacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace APIexamen1HoracioPuma.EndPoints
{
    public class ProveedorFunction
    {
        private readonly ILogger<ProveedorFunction> _logger;
        private readonly IProveedorRepositorio repos;

        public ProveedorFunction(ILogger<ProveedorFunction> logger, IProveedorRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarProveedor")]
        [OpenApiOperation("Insertarspec", "InsertarProveedor", Description = " Sirve para ingresar un Proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor),
            Description = "Ingresar Proveedor nuevo")]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
            bodyType: typeof(List<Proveedor>), 
            Description = "Mostrara una lista de Proveedors")]*/
        public async Task<HttpResponseData> InsertarProveedor([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingreesar una Proveedor con todos los datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                registro.PartitionKey = "Proveedor";
                bool sw = await repos.Create(registro);
                if (sw)
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                else
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                return respuesta;

            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }

        [Function("ListarProveedor")]
        [OpenApiOperation("Listarspec", "ListarProveedor", Description = " Sirve para listar todas los Proveedor")]

        public async Task<HttpResponseData> ListarProveedor([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;

            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        [Function("ListarProveedorId")]
        [OpenApiOperation("Listaridspec", "ListarProveedorId", Description = " Sirve para listar una Proveedor por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de los Proveedor", Description = "El RowKey de los Proveedor a obtener", Visibility = OpenApiVisibilityType.Important)]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<Proveedor>),
            Description = "Mostrara una lista de Proveedors")]*/
        public async Task<HttpResponseData> ListarProveedorId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarProveedors/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var ProveedorId = repos.Get(id);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(ProveedorId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        [Function("ModificarProveedor")]
        [OpenApiOperation("Modificarspec", "ModificarProveedor", Description = " Sirve para editar una Proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor),
            Description = "editar Proveedor")]
        public async Task<HttpResponseData> ModificarProveedor(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarProveedor")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            _logger.LogInformation($"Ejecutando azure function para modificar Proveedor.");
            try
            {
                var editar = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar una Proveedor con todos sus datos");
                bool modificado = await repos.Update(editar);
                if (modificado)
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                else
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }


        [Function("EliminarProveedor")]
        [OpenApiOperation("Eliminarspec", "DeleteProveedor", Description = " Sirve para eliminar un Estudio")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey del Estudio", Description = "El PartitionKey del Estudio a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey del Estudio", Description = "El RowKey del Estudio a borrar", Visibility = OpenApiVisibilityType.Important)]

        /*[OpenApiRequestBody("application/json", typeof(Proveedor),
            Description = "Ingresar Proveedor nueva")]*/
        public async Task<HttpResponseData> EliminarProveedor(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarProveedor/{partitionKey},{rowkey}")] HttpRequestData req,
        string partitionKey, string rowkey)
        {
            HttpResponseData respuesta;
            _logger.LogInformation($"Ejecutando azure function para eliminar Proveedor con rowkey: {rowkey} y partitionkey {partitionKey}.");
            try
            {
                bool eliminado = await repos.Delete(partitionKey, rowkey);

                if (eliminado)
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                else
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
    }
}
