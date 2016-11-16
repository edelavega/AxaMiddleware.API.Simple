using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AXA.Middleware.API.Entities
{
    public class Client : IdentityUser
    { 
        [Required]
        public bool Active { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Client> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}