using Microsoft.AspNetCore.Mvc;
using ChilluCodeCompilor.Models;
using ChilluCodeCompilor.Runners;

namespace ChilluCodeCompilor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompilerController : ControllerBase
    {
        [HttpPost("execute")]
        public async Task<IActionResult> Execute([FromBody] CodeRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Language) || string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest(new
                {
                    success = false,
                    error = "Language and code must be provided."
                });
            }

            var result = await DockerRunner.RunCode(request);
            return Ok(result);
        }
    }
}
