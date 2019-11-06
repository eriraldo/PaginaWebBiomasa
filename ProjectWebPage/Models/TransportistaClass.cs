using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWebPage.Models
{
    public class TransportistaClass
    {
        public int IdPaquete { get; set; }
        public DateTime Recibido { get; set; }
        public DateTime Entregado { get; set; }
    }
}