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
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;
        private readonly IProductoRepositorio repos;

        public ProductoFunction(ILogger<ProductoFunction> logger, IProductoRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarProducto")]
        [OpenApiOperation("Insertarspec", "InsertarProducto", Description = " Sirve para ingresar un Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto),
            Description = "Ingresar Producto nuevo")]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
            bodyType: typeof(List<Producto>), 
            Description = "Mostrara una lista de Productos")]*/
        public async Task<HttpResponseData> InsertarProducto([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingreesar una Producto con todos los datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                registro.PartitionKey = "Producto";
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

        [Function("ListarProducto")]
        [OpenApiOperation("Listarspec", "ListarProducto", Description = " Sirve para listar todas los Producto")]

        public async Task<HttpResponseData> ListarProducto([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
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
        [Function("ListarProductoId")]
        [OpenApiOperation("Listaridspec", "ListarProductoId", Description = " Sirve para listar una Producto por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de los Producto", Description = "El RowKey de los Producto a obtener", Visibility = OpenApiVisibilityType.Important)]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<Producto>),
            Description = "Mostrara una lista de Productos")]*/
        public async Task<HttpResponseData> ListarProductoId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarProductos/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var ProductoId = repos.Get(id);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(ProductoId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        [Function("ModificarProducto")]
        [OpenApiOperation("Modificarspec", "ModificarProducto", Description = " Sirve para editar una Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto),
            Description = "editar Producto")]
        public async Task<HttpResponseData> ModificarProducto(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarProducto")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            _logger.LogInformation($"Ejecutando azure function para modificar Producto.");
            try
            {
                var editar = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar una Producto con todos sus datos");
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


        [Function("EliminarProducto")]
        [OpenApiOperation("Eliminarspec", "DeleteProducto", Description = " Sirve para eliminar un Estudio")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey del Estudio", Description = "El PartitionKey del Estudio a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey del Estudio", Description = "El RowKey del Estudio a borrar", Visibility = OpenApiVisibilityType.Important)]

        /*[OpenApiRequestBody("application/json", typeof(Producto),
            Description = "Ingresar Producto nueva")]*/
        public async Task<HttpResponseData> EliminarProducto(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarProducto/{partitionKey},{rowkey}")] HttpRequestData req,
        string partitionKey, string rowkey)
        {
            HttpResponseData respuesta;
            _logger.LogInformation($"Ejecutando azure function para eliminar Producto con rowkey: {rowkey} y partitionkey {partitionKey}.");
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
