using APIexamen1HoracioPuma.Modelo.Implementacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIexamen1HoracioPuma.Contratos.Repositorios
{
    public interface IProductoRepositorio
    {
        public Task<bool> Create(Producto producto);
        public Task<bool> Update(Producto producto);
        public Task<bool> Delete(string partitionKey, string rowkey);
        public Task<List<Producto>> GetAll();
        public Task<Producto> Get(string id);
    }
}
