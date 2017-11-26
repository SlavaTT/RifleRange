using System;
using RifleRange.Api;
using System.Web.Http;

namespace RifleRange.Controllers
{
    public class BaseController : ApiController
    {
        protected ModelFactory TheFactory;

        public BaseController()
        {
            TheFactory = new ModelFactory();
        }
        [HttpGet]
        public IHttpActionResult CurrentTime()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}
