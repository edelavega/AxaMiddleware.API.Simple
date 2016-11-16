using AXA.Middleware.API.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace AXA.Middleware.API
{
      public class AxaClientManager : UserManager<Client>
    {
        public AxaClientManager(IUserStore<Client> store)
            : base(store)
        {
        }

        public static AxaClientManager Create(IdentityFactoryOptions<AxaClientManager> options, IOwinContext context)
        {
            var manager = new AxaClientManager(new UserStore<Client>(context.Get<AxaContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Client>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Client>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
