using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AXA.Middleware.API.Entities;
using AXA.Middleware.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AXA.Middleware.API
{
    public class ClientRepository : IDisposable
    {
        private readonly AxaContext _ctx;

        private readonly UserManager<Client> _userManager;

        public ClientRepository()
        {
            _ctx = new AxaContext();
            _userManager = new UserManager<Client>(new UserStore<Client>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(ClientModel userModel)
        {
            var user = new Client
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<Client> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);
            return client;
        }
  

        public async Task<Client> FindAsync(UserLoginInfo loginInfo)
        {
            var user = await _userManager.FindAsync(loginInfo);
            return user;
        }

        public async Task<IdentityResult> CreateAsync(Client user)
        {
            var result = await _userManager.CreateAsync(user);
            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);
            return result;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}