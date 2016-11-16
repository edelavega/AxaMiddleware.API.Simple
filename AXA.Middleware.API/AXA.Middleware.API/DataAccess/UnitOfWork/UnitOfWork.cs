using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AXA.Middleware.API.Entities;

namespace AXA.Middleware.API
{
    public class UnitOfWork : IDisposable
    {
        private readonly AxaContext _context = new AxaContext();
        private GenericRepository<Policy> policyRepository;
        private GenericRepository<Client> clientRepository;

        public GenericRepository<Policy> PolicyRepository
        {
            get
            {

                if (this.policyRepository == null)
                {
                    this.policyRepository = new GenericRepository<Policy>(_context);
                }
                return policyRepository;
            }
        }

        public GenericRepository<Client> ClientRepository
        {
            get
            {

                if (this.clientRepository == null)
                {
                    this.clientRepository = new GenericRepository<Client>(_context);
                }
                return clientRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}