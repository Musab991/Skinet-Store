using API.DTOs;
using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

        protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repo,
            ISpecification<T> spec, int pageIndex, int pageSize) where T : BaseEntity
        {
            var items = await repo.ListAsync(spec);
            var count = await repo.CountAsync(spec);

            var pagination = new Pagination<T>(pageSize,
                pageIndex, count, items);

            return Ok(pagination);

        }


    }
    public class BuggyController : BaseApiController
    {
        

        [HttpGet("unauthorized")]
        public IActionResult GetUnathuorized()
        {
            return Unauthorized();

        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Not a good request");

        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound();

        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a test exception");

        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(CreateProductDto product)
        {

            return Ok();
        }


    }
}
