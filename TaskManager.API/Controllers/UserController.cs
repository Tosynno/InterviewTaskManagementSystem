using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.BusinessLogic;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected GeneralClass _generalClass;

        public UserController(GeneralClass generalClass)
        {
            _generalClass = generalClass;
        }

        [HttpGet("ShowAssignAllTaskByUser")]
        public async Task<ActionResult> ShowAssignAllTaskByUser()
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            return Ok(await _generalClass.ShowAssignAllTaskByUser(auth[0].ToString()));

        }

        [HttpPost("AcceptAssignTaskByUser")]
        public async Task<ActionResult> AcceptAssignTaskByUser(AcceptAssignRequest request)
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            return Ok(await _generalClass.AcceptAssignTaskByUser(auth[0].ToString(), request));

        }

        [HttpPost("CompleteAssignTaskByUser")]
        public async Task<ActionResult> CompleteAssignTaskByUser(CompleteAssignRequest request)
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            return Ok(await _generalClass.CompleteAssignTaskByUser(auth[0].ToString(), request));

        }
    }
}
