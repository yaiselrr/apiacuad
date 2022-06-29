using System;

namespace API.Models.Dto
{
    public class C_MEDIDORDto
    {
        public string NOSERIE { get; set; }
        public string NOELECTRONICO { get; set; }
        public int ESTADO { get; set; }
        public int MODELO { get; set; }
        public int MARCA { get; set; }
        public int LOCALIZACION { get; set; }
        public DateTime FECHAALTA { get; set; }
        public float CONSUMOGLOBAL { get; set; }
        public int PROVEEDORMEDIDOR_C { get; set; }
        public string NOFACTURA { get; set; }
        public int SECTOR { get; set; }
    }
}
