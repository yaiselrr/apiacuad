using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class BalanceDto
    {

        [Required(ErrorMessage = "Por favor teclee el campo Inmueble")]
        [Display(Name = "Inmueble")]
        public string PropertyNumber { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Nombre")]
        [Display(Name = "Nombre")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Fecha de Expiración")]
        [Display(Name = "Fecha de Vencimiento")]
        public string ExpiredDate { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Saldo Anterior")]
        [Display(Name = "Saldo Anterior")]
        public double BeforeBalance { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Saldo Actual")]
        [Display(Name = "Saldo Actual")]
        public double NewBalance { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Saldo Total")]
        [Display(Name = "Saldo Total")]
        public double TotalBalance { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Saldo concepto")]
        [Display(Name = "Saldo Total")]
        public string Concept { get; set; }
    }
}
