using System;
using System.Collections.Generic;

#nullable disable

namespace API.OtherModels
{
    public partial class CIndividuo
    {
        public int IndividuoC { get; set; }
        public string IndividuoD { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string EMail { get; set; }
        public string Registro { get; set; }
        public string Identificacion { get; set; }
        public string Ctabancaria { get; set; }
        public int? Banco { get; set; }
        public short? FacturaId { get; set; }
        public string DomRef { get; set; }
        public string DomTcta { get; set; }
        public string DomTitular { get; set; }
        public DateTime? DomFecha { get; set; }
        public string DomEstado { get; set; }
        public string DomComentario { get; set; }
        public byte[] Foto { get; set; }
        public decimal? Limite { get; set; }
    }
}
