using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("Tasks")]
        public async Task<IActionResult> Tasks()
        {
            return await Task.FromResult(Ok(new string[] { "1Task", "2Tasks" }));
        }
    }
}
