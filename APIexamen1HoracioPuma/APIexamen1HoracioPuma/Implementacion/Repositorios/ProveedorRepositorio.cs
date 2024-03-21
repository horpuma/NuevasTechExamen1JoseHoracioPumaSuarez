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
    public class ProveedorRepositorio : IProveedorRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tablaNombre;
        private readonly IConfiguration configuration;
        public ProveedorRepositorio(IConfiguration conf)
        {
            this.configuration = conf;
            this.cadenaConexion = configuration.GetSection("CadenaConexion").Value;
            this.tablaNombre = "Proveedor";
        }

        public async Task<bool> Create(Proveedor objeto)
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

        public async Task<Proveedor> Get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Proveedor' and RowKey eq '{id}'";
            await foreach (Proveedor linea in tablaCliente.QueryAsync<Proveedor>(filter: filtro))
            {
                return linea;
            }
            return null;
        }

        public async Task<List<Proveedor>> GetAll()
        {
            List<Proveedor> lista = new List<Proveedor>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Proveedor'";
            await foreach (Proveedor linea in tablaCliente.QueryAsync<Proveedor>(filter: filtro))
            {
                lista.Add(linea);
            }
            return lista;
        }

        public async Task<bool> Update(Proveedor objeto)
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
