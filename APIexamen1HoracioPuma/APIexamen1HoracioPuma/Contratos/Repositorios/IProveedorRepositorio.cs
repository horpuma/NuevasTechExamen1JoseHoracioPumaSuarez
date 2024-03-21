using APIexamen1HoracioPuma.Modelo.Implementacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIexamen1HoracioPuma.Contratos.Repositorios
{
    public interface IProveedorRepositorio
    {
        public Task<bool> Create(Proveedor proveedor);
        public Task<bool> Update(Proveedor proveedor);
        public Task<bool> Delete(string partitionKey, string rowkey);
        public Task<List<Proveedor>> GetAll();
        public Task<Proveedor> Get(string id);
    }
}
