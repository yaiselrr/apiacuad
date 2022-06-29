using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class C_MEDIDOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MEDIDOR_C { get; set; }
        public string NO_SERIE { get; set; }
        public string NO_ELECTRONICO { get; set; }
        public int ESTADO_C { get; set; }
        public int MODELO_C { get; set; }
        public int MARCA_C { get; set; }
        public int LOCALIZACION_C { get; set; }
        public DateTime FECHA_ALTA { get; set; }
        public float CONSUMO_GLOBAL { get; set; }
        public int PROVEEDOR_MEDIDOR_C { get; set; }
        public string NO_FACTURA { get; set; }
        public int SECTOR_C { get; set; }
    }
}
