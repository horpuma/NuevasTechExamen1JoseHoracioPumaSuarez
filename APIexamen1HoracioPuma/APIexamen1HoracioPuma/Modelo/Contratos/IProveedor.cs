﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIexamen1HoracioPuma.Modelo.Contratos
{
    public interface IProveedor
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
    }
}
