using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartHomeAPI.Models;
using SmartHomeAPI.Services;

namespace SmartHomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly Devices_Service _deviceService;

        public DevicesController(Devices_Service deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpPost("AddDevice")]
        public async Task<IActionResult> PostDevice([FromBody] List<DeviceModels> deviceModels)
        {
            string deviceJSON = JsonConvert.SerializeObject(deviceModels);
            await _deviceService.CreateDevice(deviceJSON);
            return StatusCode(201);
        }
        [HttpGet("GetAllDevices")]
        public async Task<IActionResult> ReturnAllDevices()
        {
            var devicelist = await _deviceService.GetAllDevices();
            return Ok(devicelist);
        }

        [HttpGet("GetAllDevicesOfUser/{userID}")]
        public async Task<IActionResult> GetAllDevicesOfUser (string userID)
        {
            var userDevList = await _deviceService.GetAllDevicesOfUser(userID);
            return Ok(userDevList);
        }
        [HttpGet("GetAllDevicesOfHouse/{houseID}")]
        public async Task<IActionResult> GetAllDevicesOfHouse(string houseID)
        {
            var houseDevList = await _deviceService.GetAllDevicesOfHouse(houseID);
            return Ok(houseDevList);
        }

        [HttpGet("GetSingleDevice/{deviceID}")]
        public async Task<IActionResult> ReturnSingleDevice(string deviceID)
        {
            var dev = await _deviceService.GetSingleDevice(deviceID);
            return Ok(dev);
        }

        [HttpGet("GetDevicesOfRoom/{roomID}")]
        public async Task<IActionResult> ReturnDevicesOfRoom(string roomID)
        {
            var deviceOfRoom = await _deviceService.GetDevicesOfRoom(roomID);
            return Ok(deviceOfRoom);
        }

        [HttpPut("UpdateDevice/{deviceID}")]
        public async Task<IActionResult> UpdateSingleDevice(string deviceID, [FromBody] DeviceModels deviceModels)
        {
            await _deviceService.UpdateDevice(deviceID, deviceModels);
            return Ok("Successfully edited device info");
        }

        [HttpDelete("DeleteDevice/{deviceID}")]
        public async Task<IActionResult> DeleteSingleDevice(string deviceID)
        {
            await _deviceService.DeleteDevice(deviceID);
            return Ok("Deleted");
        }
        [HttpDelete("DEV_DELETE_DB_COLLECTION")]
        public async Task<IActionResult> ClearTable()
        {
            await _deviceService.DEV_CLEAR_COLLECTION();
            return Ok();
        }
        [HttpDelete("DEV_CLEAR_RECORDS")]
        public async Task<IActionResult> ClearRecords()
        {
            await _deviceService.DEV_CLEAR_RECORDS();
            return Ok();
        }
    }
}
