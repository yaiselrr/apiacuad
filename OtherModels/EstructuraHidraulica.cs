using System;
using System.Collections.Generic;

#nullable disable

namespace API.OtherModels
{
    public partial class EstructuraHidraulica
    {
        public int Inmueble { get; set; }
        public int Derivada { get; set; }
        public int Toma { get; set; }
        public int AbastecimientoC { get; set; }
        public int DescargaC { get; set; }
        public int DiametroToma { get; set; }
        public int DiametroDescarga { get; set; }
        public int MaterialTomaC { get; set; }
        public int? MaterialDescargaC { get; set; }
        public decimal? Volumenacumulado { get; set; }
        public int MedidorC { get; set; }
        public float? ValorX { get; set; }
        public float? ValorY { get; set; }
        public int EstadotC { get; set; }
        public int ServicioContratadoC { get; set; }
        public int? CasoFacturacion { get; set; }
        public string UbicacionToma { get; set; }
        public int? CircuitoHidraulico { get; set; }
        public float? ValorZ { get; set; }
        public int? CvePeriodo { get; set; }
        public int? ALectura { get; set; }
        public int? MLectura { get; set; }
        public int? PeriodoAbsolutoC { get; set; }
    }
}
