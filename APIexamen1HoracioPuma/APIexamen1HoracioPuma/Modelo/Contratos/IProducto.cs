﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIexamen1HoracioPuma.Modelo.Contratos
{
    public interface IProducto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public string ProveedorId { get; set; }
    }
}
