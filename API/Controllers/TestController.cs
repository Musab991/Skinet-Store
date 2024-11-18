using API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class TestController : BaseApiController
    {
        /// <summary>
        /// using extension with no delgates\
        /// I felt this is more type safty , easy to handle errors with it ..
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ToDo()
        {
            string userId = User.GetUserId();
            string userCountry = User.GetCountry();
            int age = User.GetAge();
            string userEditRole = User.GetEditRole();

            return Ok(new { userId, userCountry, userEditRole, age });
        }

    }
    public class TestController2 : BaseApiController
    {
        /// <summary>
        /// I delceared property CurrentUser in my baseAPiController
        /// I access to its class varibales ain't flexible that much
        /// I added then delgate to more felxibilty 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ToDo()
        {
            
            string userId = CurrentUser.Id;
            string userCountry = CurrentUser.Country;
            int age = CurrentUser.Age;
            string userEditRole = CurrentUser.EditRole;

             userEditRole = GetClaimValue("example_claim_name");

            return Ok(new { userId, userCountry, userEditRole, age });
        }

    }
}
