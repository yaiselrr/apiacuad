using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class CreditCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo número de Tarjeta de Crédito")]
        [StringLength(16)]
        [CreditCard]
        [Column(TypeName = "VARCHAR(16)")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Fecha de Expiración")]
        [RegularExpression(@"^((0[1-9]|1[0-2])/(201[4-9]|202[0-9]))$", ErrorMessage = "Characters are not allowed.")]
        public string ExpiredDate { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Nombre de Usuario")]
        [StringLength(16)]
        [CreditCard]
        public string UserName { get; set; }

        public bool IsDefault { get; set; } = false;

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Por favor teclee el campo Usuario")]
        public string GeneralDataUserId { get; set; }

        [ForeignKey("GeneralDataUserId")]
        public GeneralDataUser GeneralDataUser { get; set; }




    }
}
