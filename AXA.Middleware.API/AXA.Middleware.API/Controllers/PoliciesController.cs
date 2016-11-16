using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using AXA.Middleware.API.Entities;
using AXA.Middleware.API.Models;

namespace AXA.Middleware.API.Controllers
{
    [RoutePrefix("api/policies")]
    //[Authorize(Roles = "admin")]
    public class PoliciesController : ApiController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [AcceptVerbs("GET")]
        [Route("{id}/clients")]
        public IHttpActionResult GetClientByPolicy(string id)
        {
            var test = _unitOfWork.PolicyRepository.Get<Policy>(id);
            var policyMatch = _unitOfWork.PolicyRepository.Find<Policy>(policy => policy.Id == id, null, "Client").FirstOrDefault();

            if (policyMatch == null)
                return Ok<ClientModel>(null);

            var client = new ClientModel { UserName = policyMatch.Client.UserName, Email = policyMatch.Client.Email };
            return Ok(client);
        }

    }
}
