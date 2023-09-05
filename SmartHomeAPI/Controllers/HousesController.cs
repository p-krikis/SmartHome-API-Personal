using Microsoft.AspNetCore.Mvc;
using SmartHomeAPI.Models;
using SmartHomeAPI.Services;

namespace SmartHomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousesController : ControllerBase
    {
        private readonly Houses_Service _houseService;

        public HousesController(Houses_Service service)
        {
            _houseService = service;
        }

        [HttpPost("AddHouse")]
        public async Task<IActionResult> AddHouse([FromBody] HouseModels houses)
        {
            var response = await _houseService.AddHouse(houses);
            if (response != null)
            {
                return StatusCode(201);
            }
            else
            {
                return BadRequest("House with same name already exists for this user");
            }
            
        }
        [HttpGet("GetAllHousesOfUser/{userID}")]
        public async Task<IActionResult> GetAllHousesOfUser(string userID)
        {
            var houseList = await _houseService.GetAllHouses(userID);
            return Ok(houseList);
        }
        [HttpGet("GetSingleHouse/{houseID}")]
        public async Task<IActionResult> GetSingleHouse(string houseID)
        {
            var house = await _houseService.GetSpecificHouse(houseID);
            return Ok(house);
        }
        [HttpPut("UpdateHouse/{houseID}")]
        public async Task<IActionResult> UpdateHouse(string houseID, [FromBody] HouseModels houseModels)
        {
            await _houseService.UpdateHouse(houseID, houseModels);
            return Ok($"Edited house with ID {houseID}");
        }
        [HttpDelete("DeleteHouse/{houseID}")]
        public async Task<IActionResult> DeleteHouse (string houseID)
        {
            await _houseService.DeleteHouse(houseID);
            return Ok($"House with ID: {houseID} has been deleted");
        }
        [HttpDelete("DEV_DELETE_DB_COLLECTION")]
        public async Task<IActionResult> ClearTable()
        {
            await _houseService.DEV_CLEAR_COLLECTION();
            return Ok();
        }
    }
}
