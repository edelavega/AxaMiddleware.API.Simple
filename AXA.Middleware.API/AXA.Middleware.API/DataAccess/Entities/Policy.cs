using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AXA.Middleware.API.Entities
{
    public class Policy
    {
        [Key]
        public string Id { get; set; }

        [Range(0, 999999999.99)]
        [Required]
        public double AmountInsured { get; set; }

        [MaxLength(100)]
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public DateTime InceptionDate { get; set; }

        [Required]
        public bool InstallmentPayment { get; set; }

        public string ClientId { get; set; }

        [ForeignKey("ClientId")]
        [Required]
        public virtual Client Client { get; set; }
    }
}
