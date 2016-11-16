using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using AXA.Middleware.API.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;

namespace AXA.Middleware.API.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AxaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AxaContext context)
        {
            try
            {
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    System.Diagnostics.Debugger.Launch();
                }

                using (var wc = new WebClient())
                {
                    var jsonClients =
                        JObject.Parse(wc.DownloadString("http://www.mocky.io/v2/5808862710000087232b75ac"))["clients"];
                    var passwordHash = new PasswordHasher();
                    var hashpass = passwordHash.HashPassword("Pwd@123");
                    var clients = jsonClients.Select(jsonClient => new Client
                    {
                        Email = jsonClient["email"].ToString(),
                        Active = true,
                        Id = jsonClient["id"].ToString(),
                        UserName = jsonClient["name"].ToString(),
                        PasswordHash = hashpass,
                        SecurityStamp = Guid.NewGuid().ToString()
                    }).ToList();

                    var roles = jsonClients.Select(jsonClientRoles => new
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = jsonClientRoles["role"].ToString(),
                        ClientId = jsonClientRoles["id"].ToString()
                    }
                        ).ToList();

                    var jsonPolicies =
                         JObject.Parse(wc.DownloadString("http://www.mocky.io/v2/580891a4100000e8242b75c5"))["policies"];
                    var policies = jsonPolicies.Select(jsonPolicy => new Policy()
                    {
                        Email = jsonPolicy["email"].ToString(),
                        Id = jsonPolicy["id"].ToString(),
                        AmountInsured = double.Parse(jsonPolicy["amountInsured"].ToString()),
                        InceptionDate = Convert.ToDateTime(jsonPolicy["inceptionDate"].ToString()),
                        InstallmentPayment = bool.Parse(jsonPolicy["installmentPayment"].ToString()),
                        Client = clients.FirstOrDefault(p => p.Id == jsonPolicy["clientId"].ToString())
                    }).ToList();



                    clients.ForEach(s => context.Clients.AddOrUpdate(p => p.Id, s));
                    context.SaveChanges();
                    policies.ForEach(s => context.Policies.AddOrUpdate(p => p.Id, s));
                    context.SaveChanges();

                    foreach (var jsonRole in roles)
                    {
                        if (!context.Roles.Any(r => r.Name == jsonRole.Name))
                        {
                            var storeRole = new RoleStore<IdentityRole>(context);
                            var managerRole = new RoleManager<IdentityRole>(storeRole);
                            var role = new IdentityRole { Name = jsonRole.Name };
                            managerRole.Create(role);
                        }
                        var storeUser = new UserStore<Client>(context);
                        var managerUser = new UserManager<Client>(storeUser);
                        managerUser.AddToRole(jsonRole.ClientId, jsonRole.Name);

                    }

                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


        }
    }
}
