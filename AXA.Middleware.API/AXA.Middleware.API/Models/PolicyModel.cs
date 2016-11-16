using System;
using System.ComponentModel.DataAnnotations;

namespace AXA.Middleware.API.Models
{
    class PolicyModel
    {
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
    }
}
