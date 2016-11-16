using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using AXA.Middleware.API.Entities;
using AXA.Middleware.API.Models;

namespace AXA.Middleware.API.Controllers
{
    [RoutePrefix("api/clients")]
    [Authorize(Roles = "admin,user")]
    public class ClientsController : ApiController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private PaginationHeaderModel _paginationHeader;
        // GET: api/Clients/GetClientById/5
        [AcceptVerbs("GET")]
        [Route("{id:guid}")]
        public IHttpActionResult GetClientById(string id)
        {
            var client = _unitOfWork.ClientRepository.Get<Client>(id);
            return Ok(client != null ? new ClientModel{Id=client.Id, UserName = client.UserName, Email = client.Email} : null);
        }

        // GET: api/Clients/GetClientByName/pepe
        [AcceptVerbs("GET")]
        [Route("{userName}")]
        public IHttpActionResult GetClientByUserName(string userName)
        {
            var client = _unitOfWork.ClientRepository.FindFirstOrDefault<Client>(p => p.UserName == userName);
            return Ok(client != null ? new ClientModel { Id = client.Id, UserName = client.UserName, Email = client.Email } : null);
        }

        // GET: api/Clients/5
        [AcceptVerbs("GET")]
        [Route("{userName}/policies")]
        public IHttpActionResult GetPoliciesByUserName(string userName)
        {
            var totalCount = _unitOfWork.PolicyRepository.GetAll<Client>().Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / 10);

            _paginationHeader = new PaginationHeaderModel
            {
                TotalCount = totalCount,
                TotalPages = totalPages - 1
            };

            var clientMatch = _unitOfWork.PolicyRepository.Find<Client>(client => client.UserName == userName, null, "Policies").FirstOrDefault();

            if (clientMatch == null)
                return Ok<List<PolicyModel>>(null);

            var policyList = clientMatch.Policies.Select(p => new PolicyModel
            {
                Id = p.Id,
                AmountInsured = p.AmountInsured,
                Email = p.Email,
                InceptionDate = p.InceptionDate,
                InstallmentPayment = p.InstallmentPayment
            }).ToList();
            return Ok(policyList);
        }

        [AcceptVerbs("GET")]
        [Route(Name = "clients")]
        //Pendiente agregar paginado.
        public IHttpActionResult GetAllClients(int page = 0, int pageSize = 10)
        {
            var totalCount = _unitOfWork.PolicyRepository.GetAll<Client>().Count();
            var totalPages = (int) Math.Ceiling((double) totalCount/pageSize);

            var urlHelper = new UrlHelper(Request);
            var prevLink = page > 0 ? urlHelper.Link("clients", new {page = page - 1, pageSize}) : "";
            var nextLink = page < totalPages - 1
                ? urlHelper.Link("clients", new { page = page + 1, pageSize })
                : "";

            _paginationHeader = new PaginationHeaderModel
            {
                TotalCount = totalCount,
                TotalPages = totalPages - 1,
                PrevPageLink = prevLink,
                NextPageLink = nextLink
            };

            var clientList = _unitOfWork.PolicyRepository.GetAll<Client>()
                .OrderBy(p => p.UserName)
                .Skip(pageSize*page)
                .Take(pageSize)
                .ToList()
                .OrderBy(p => p.UserName)
                .Select(p => new ClientModel
                {
                    Id = p.Id,
                    Email = p.Email,
                    UserName = p.UserName
                });
            //dynamic list = new {count = _unitOfWork.PolicyRepository.GetAll<Client>().Count(), clients = clientList};
            return Ok(clientList);
        }

        protected override OkResult Ok()
        {
            System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(_paginationHeader));

            System.Web.HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            return base.Ok();
        }

        protected override OkNegotiatedContentResult<T> Ok<T>(T content)
        {
            System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(_paginationHeader));

            System.Web.HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            return base.Ok(content);
        }
    }
}
