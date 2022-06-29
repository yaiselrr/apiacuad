using System;
using System.Collections.Generic;

#nullable disable

namespace API.OtherModels
{
    public partial class CConceptoacobrar
    {
        public int ConceptocobrarC { get; set; }
        public string ConceptocobrarD { get; set; }
        public string NombreCorto { get; set; }
        public string TipoCalculo { get; set; }
        public DateTime? VigenciaInicio { get; set; }
        public DateTime? VigenciaFin { get; set; }
        public short? Facturacion { get; set; }
        public short? Aplicado { get; set; }
        public short? Proceso { get; set; }
        public short? Adicional { get; set; }
        public short? Arrastrado { get; set; }
        public int? UnidadC { get; set; }
        public decimal? Impuesto { get; set; }
    }
}
