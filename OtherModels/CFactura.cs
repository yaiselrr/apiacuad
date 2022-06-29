using System;
using System.Collections.Generic;

#nullable disable

namespace API.OtherModels
{
    public partial class CFactura
    {
        public int FacturaC { get; set; }
        public int? Inmueble { get; set; }
        public int? Derivada { get; set; }
        public int? Toma { get; set; }
        public int PeriodoAbsolutoC { get; set; }
        public decimal? ConsumoFacturado { get; set; }
        public decimal? ConsumoDescontado { get; set; }
        public int CasoFacturacionC { get; set; }
        public int ServicioC { get; set; }
        public int NivelC { get; set; }
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int EstadoC { get; set; }
        public short? Pagada { get; set; }
        public int? AFactura { get; set; }
        public int? MFactura { get; set; }
        public DateTime? FechaEstado { get; set; }
        public int? SecLectura { get; set; }
        public int? LecturaAnt { get; set; }
        public short Entregada { get; set; }
        public int? PeriodosAdeudo { get; set; }
        public int? EdoP { get; set; }
        public int? SitP { get; set; }
        public int? EdoT { get; set; }
        public int? Inicioadeudo { get; set; }
        public int? Lecturista { get; set; }
        public int? MetodoAnticipo { get; set; }
        public decimal? RefAnticipo { get; set; }
        public DateTime? FechaCorte { get; set; }
        public int? BloqueC { get; set; }
        public string Uuid { get; set; }
    }
}
