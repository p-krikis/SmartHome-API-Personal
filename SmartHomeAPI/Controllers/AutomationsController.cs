using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAPI.Models;
using SmartHomeAPI.Services;

namespace SmartHomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutomationsController : ControllerBase
    {
        private readonly Automations_Service _automationService;

        public AutomationsController(Automations_Service automationService)
        {
            _automationService = automationService;
        }
        [HttpPost("AddAutomation")]
        public async Task<IActionResult> AddAutomation([FromBody] AutomationModels automation)
        {
            await _automationService.AddAutomation(automation);
            return StatusCode(201);
        }
        [HttpGet("GetAllAutomations/{userID}")]
        public async Task<IActionResult> GetAllAutomations(string userID)
        {
            var autoList = await _automationService.FetchAllAutomations(userID);
            return Ok(autoList);
        }
        [HttpGet("GetSingleAutomation/{autoID}")]
        public async Task<IActionResult> GetSingleAutomation(string autoID)
        {
            var autoData = await _automationService.FetchSingleAutomatio(autoID);
            return Ok(autoData);
        }
        [HttpPut("UpdateAutomation/{autoID}")]
        public async Task<IActionResult> UpdateAutomation(string autoID, [FromBody]AutomationModels automation)
        {
            await _automationService.UpdateAutomation(autoID, automation);
            return Ok($"Updated automation entry with id {autoID}");
        }
        [HttpDelete("DeleteAutomation/{autoID}")]
        public async Task<IActionResult> DeleteAutomation(string autoID)
        {
            await _automationService.DeleteAutomation(autoID);
            return Ok($"Selected entry with ID: {autoID} has been deleted");
        }
    }
}
