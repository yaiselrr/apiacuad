using System;
using System.Collections.Generic;

#nullable disable

namespace API.OtherModels
{
    public partial class RegistroFactura
    {
        public int FacturaC { get; set; }
        public int ConceptocobrarC { get; set; }
        public int Renglon { get; set; }
        public decimal? SaldoActual { get; set; }
        public decimal? SaldoAnterior { get; set; }
        public int? Registro { get; set; }
        public decimal? ImpNegociado { get; set; }
        public decimal? ImpAnticipo { get; set; }
        public DateTime? Fecha { get; set; }
        public int? MovAplicado { get; set; }
        public decimal? Tasa01 { get; set; }
    }
}
