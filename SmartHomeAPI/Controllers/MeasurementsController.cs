using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartHomeAPI.Models;
using SmartHomeAPI.Services;

namespace SmartHomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly Measurements_Service _measurementsService;

        public MeasurementsController(Measurements_Service measurementsService)
        {
            _measurementsService = measurementsService;
        }
        [HttpPost("PostMeasurement")]
        public async Task<IActionResult> PostMeasurement([FromBody] List<MeasurementModels> measurementModels)
        {
            string measurementsJSON = JsonConvert.SerializeObject(measurementModels);
            await _measurementsService.PostMeasurement(measurementsJSON);
            return StatusCode(201);
        }
        [HttpGet("GetMeasurements/{deviceID}")]
        public async Task<IActionResult> GetMeasurements(string deviceID)
        {
            var measureList = await _measurementsService.GetMeasurements(deviceID);
            return Ok(measureList);
        }
        [HttpPost("SpecificPeriod/{deviceID}")]
        public async Task<IActionResult> SpecificPeriod(string deviceID, [FromBody] SpecificPeriodModels specificPeriod)
        {
            var validMeasurements = await _measurementsService.GetSpecificPeriod(deviceID, specificPeriod);
            return Ok(validMeasurements);
        }
        [HttpGet("MostRecent/{deviceID}")]
        public async Task<IActionResult> MostRecent(string deviceID)
        {
            var mostRecentMeas = await _measurementsService.MostRecent(deviceID);
            return Ok(mostRecentMeas);
        }
        [HttpDelete("DEV_DELETE_DB_COLLECTION")]
        public async Task<IActionResult> ClearTable()
        {
            await _measurementsService.DEV_CLEAR_COLLECTION();
            return Ok();
        }
        //[HttpPost("DEV_GENERATE_DATA/{amount}")]
        //public async Task<IActionResult> GenerateData(int amount,[FromBody] MeasurementModels measurement)
        //{
        //    await _measurementsService.GenerateData(amount, measurement);
        //    return Ok();
        //}
    }
}
