using System;
using System.Collections.Generic;

#nullable disable

namespace API.OtherModels.DB
{
    public partial class CMedidor
    {
        public int MedidorC { get; set; }
        public string NoSerie { get; set; }
        public string NoElectronico { get; set; }
        public int? EstadoC { get; set; }
        public int? ModeloC { get; set; }
        public int? MarcaC { get; set; }
        public int? LocalizacionC { get; set; }
        public DateTime? FechaAlta { get; set; }
        public decimal ConsumoGlobal { get; set; }
        public int? ProveedorMedidorC { get; set; }
        public string NoFactura { get; set; }
        public int? SectorC { get; set; }
    }
}
