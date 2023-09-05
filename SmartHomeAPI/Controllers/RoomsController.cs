using Microsoft.AspNetCore.Mvc;
using SmartHomeAPI.Models;
using SmartHomeAPI.Services;

namespace SmartHomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly Rooms_Service _roomsService;

        public RoomsController(Rooms_Service roomsService)
        {
            _roomsService = roomsService;
        }
        [HttpPost("AddRoom")]
        public async Task<IActionResult> AddRoom([FromBody] RoomModels rooms)
        {
            await _roomsService.AddRoom(rooms);
            return StatusCode(201);
        }
        [HttpGet("GetAllRoomsOfHouse/{houseID}")]
        public async Task<IActionResult> GetAllRoomsOfHouse(string houseID)
        {
            var roomList = await _roomsService.GetAllRoomsOfHouse(houseID);
            return Ok(roomList);
        }
        [HttpGet("GetAllRoomsOfUser/{userID}")]
        public async Task<IActionResult> GetAllRoomsOfUser(string userID)
        {
            var userRoomList = await _roomsService.GetAllRoomsOfUser(userID);
            return Ok(userRoomList);
        }
        [HttpGet("GetSingleRoom/{roomID}")]
        public async Task<IActionResult> GetSingleRoom(string roomID)
        {
            var room = await _roomsService.GetSpecificRoom(roomID);
            return Ok(room);
        }
        [HttpPut("UpdateRoom/{roomID}")]
        public async Task<IActionResult> UpdateRoom(string roomID, [FromBody] RoomModels roomModels)
        {
            await _roomsService.UpdateRoom(roomID, roomModels);
            return Ok($"Edited room with ID: {roomID} and Name: {roomModels.RoomName}");
        }
        [HttpDelete("DeleteRoom/{roomID}")]
        public async Task<IActionResult> DeleteRoom(string roomID)
        {
            await _roomsService.DeleteRoom(roomID);
            return Ok($"Room with ID: {roomID} has been deleted");
        }
        [HttpDelete("DEV_DELETE_DB_COLLECTION")]
        public async Task<IActionResult> ClearTable()
        {
            await _roomsService.DEV_CLEAR_COLLECTION();
            return Ok();
        }
    }
}
