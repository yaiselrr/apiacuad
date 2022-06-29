using System;
using System.Collections.Generic;

#nullable disable

namespace API.OtherModels.DB
{
    public partial class Derivadum
    {
        public int Inmueble { get; set; }
        public int Derivada { get; set; }
        public int ServicioC { get; set; }
        public int GiroC { get; set; }
        public int NivelC { get; set; }
        public int EstadopC { get; set; }
        public int SituacionC { get; set; }
        public int? Beneficiados { get; set; }
        public DateTime? Alta { get; set; }
        public int? Contrato { get; set; }
        public string Interior { get; set; }
        public string Edificio { get; set; }
        public string Piso { get; set; }
        public int? FolioRuta { get; set; }
        public double? MetrosConstruidos { get; set; }
        public int? Usuario { get; set; }
        public int? Propietario { get; set; }
        public int? Administrador { get; set; }
        public string FolioCenso { get; set; }
        public string Alterna { get; set; }
        public string Catastro { get; set; }
        public int? DirFacturacion { get; set; }
        public int? DirEnvio { get; set; }
        public string Observacion { get; set; }
        public string CalificaCliente { get; set; }
        public int? RutaC { get; set; }
        public short? Adicionales { get; set; }
    }
}
