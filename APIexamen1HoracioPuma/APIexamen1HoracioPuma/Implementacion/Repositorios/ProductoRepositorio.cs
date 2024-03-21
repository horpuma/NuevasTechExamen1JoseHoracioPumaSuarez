using APIexamen1HoracioPuma.Contratos.Repositorios;
using APIexamen1HoracioPuma.Modelo.Implementacion;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIexamen1HoracioPuma.Implementacion.Repositorios
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tablaNombre;
        private readonly IConfiguration configuration;
        public ProductoRepositorio(IConfiguration conf)
        {
            this.configuration = conf;
            this.cadenaConexion = configuration.GetSection("CadenaConexion").Value;
            this.tablaNombre = "Productos";
        }

        public async Task<bool> Create(Producto objeto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(objeto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string partitionKey, string rowkey)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionKey, rowkey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Producto> Get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Producto' and RowKey eq '{id}'";
            await foreach (Producto linea in tablaCliente.QueryAsync<Producto>(filter: filtro))
            {
                return linea;
            }
            return null;
        }

        public async Task<List<Producto>> GetAll()
        {
            List<Producto> lista = new List<Producto>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Producto'";
            await foreach (Producto linea in tablaCliente.QueryAsync<Producto>(filter: filtro))
            {
                lista.Add(linea);
            }
            return lista;
        }

        public async Task<bool> Update(Producto objeto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(objeto, objeto.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
