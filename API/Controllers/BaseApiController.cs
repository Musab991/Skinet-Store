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

        /// //////////START///////////

        // Computed property that retrieves user claims on-demand
        public CurrentUserClaims CurrentUser => new CurrentUserClaims
        {
            Id = User.FindFirst("Id")?.Value,
            Country = User.FindFirst("Country")?.Value,
            Age = int.TryParse(User.FindFirst("Age")?.Value, out var age) ? age : 0,
            EditRole = User.FindFirst("EditRole")?.Value
        };

        // Func delegate for retrieving specific claims
        public Func<string, string> GetClaimValue =>
            claimType => User.FindFirst(claimType)?.Value;

        /// //////////END///////////

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
    //I added this class only as a help ,nothing more..
    public class CurrentUserClaims
    {
        public string Id { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public string EditRole { get; set; }

    }
}
