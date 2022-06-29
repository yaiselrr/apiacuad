using System;
using System.Collections.Generic;

#nullable disable

namespace API.OtherModels
{
    public partial class EstadoDeCuentum
    {
        public int Inmueble { get; set; }
        public int Derivada { get; set; }
        public int Toma { get; set; }
        public int? Periodofinaladeudo { get; set; }
        public int? Periodoinicialadeudo { get; set; }
        public int? Periodosdeadeudo { get; set; }
        public DateTime? Primerafacturacion { get; set; }
        public decimal? Adeudoanterior { get; set; }
        public decimal? Cargosactuales { get; set; }
        public decimal? Pagosactuales { get; set; }
        public decimal? Creditosactuales { get; set; }
        public decimal? Cargospendientes { get; set; }
        public int? Documento { get; set; }
        public int? CvePeriodo { get; set; }
        public int? AFactura { get; set; }
        public int? MFactura { get; set; }
        public int? PeriodoAbsolutoC { get; set; }
        public decimal? Saldototal { get; set; }

        public virtual Derivadum Derivadum { get; set; }
    }
}
