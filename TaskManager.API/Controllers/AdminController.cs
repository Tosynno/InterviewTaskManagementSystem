using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.BusinessLogic;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        protected GeneralClass _generalClass;

        public AdminController(GeneralClass generalClass)
        {
            _generalClass = generalClass;
        }

        [HttpPost("CreateTask")]
        public async Task<ActionResult> CreateTask(CreateTaskRequest request)
        {
           // string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            return Ok(await _generalClass.CreateTask(request));

        }
        [HttpGet("GetAllTask")]
        public async Task<ActionResult> GetAllTask()
        {
            // string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            return Ok(await _generalClass.GetAllTask());

        }
        [HttpGet("GetAllTask/{TaskControlId}")]
        public async Task<ActionResult> GetAllTask(string TaskControlId)
        {
            // string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            return Ok(await _generalClass.GetAllTask(TaskControlId));

        }
        [HttpPut("UpdateTask")]
        public async Task<ActionResult> UpdateTask(string TaskControlId, CreateTaskRequest request)
        {
            // string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            return Ok(await _generalClass.UpdateTask(TaskControlId, request));

        }
    }
}
