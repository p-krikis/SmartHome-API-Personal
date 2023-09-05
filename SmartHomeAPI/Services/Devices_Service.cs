using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using SmartHomeAPI.Models;

namespace SmartHomeAPI.Services
{
    public class Devices_Service
    {
        private readonly IMongoCollection<DeviceModels> _devices;
        private readonly IMongoCollection<RecordModels> _scr;
        private readonly IMongoCollection<MeasurementModels> _measurements;

        public Devices_Service(IOptions<MongoDB_Devices> mongoDBsettings) //placeholder TODO fix this
        {
            MongoClient client = new("mongodb+srv://<USERNAME>:<PASSWORD>@smarthometest.jpiqofv.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = client.GetDatabase("SmartHomeDB");
            _devices = db.GetCollection<DeviceModels>("Devices");
            _scr = db.GetCollection<RecordModels>("StatusChangeRecord");
            _measurements = db.GetCollection<MeasurementModels>("Measurements");
        }

        public async Task CreateDevice(string deviceModels)
        {
            List<DeviceModels> devices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceModels>>(deviceModels);
            foreach (var device in devices)
            {
                await _devices.InsertOneAsync(device);
            }
            return;
        }
        public async Task<List<DeviceModels>> GetAllDevices()
        {
            return await _devices.Find(new BsonDocument()).ToListAsync();
        }
        public async Task<List<DeviceModels>> GetAllDevicesOfUser(string userID)
        {
            var userDevice = await _devices.Find(dev => dev.DevOwnerID == userID).ToListAsync();
            return userDevice;
        }
        public async Task<List<DeviceModels>> GetAllDevicesOfHouse(string houseID)
        {
            var houseDevice = await _devices.Find(dev => dev.HouseID == houseID).ToListAsync();
            return houseDevice;
        }

        public async Task<DeviceModels> GetSingleDevice(string deviceID)
        {
            var device = await _devices.Find(dev => dev.DeviceID == deviceID).FirstOrDefaultAsync();
            return device;
        }
        public async Task<List<DeviceModels>> GetDevicesOfRoom(string roomID)
        {
            return await _devices.Find(dev => dev.RoomID == roomID).ToListAsync();
        }

        public async Task UpdateDevice(string deviceID, DeviceModels deviceTBU)
        {
            await _devices.ReplaceOneAsync(dev => dev.DeviceID == deviceID, deviceTBU);

            var updatedDev = _devices.Find(d => d.DeviceID == deviceID).FirstOrDefaultAsync();
            
            if (updatedDev.Result.Status != deviceTBU.Status)
            {
                RecordModels recordModels = new RecordModels();
                recordModels.DevID = deviceTBU.DeviceID;
                recordModels.Status = deviceTBU.Status;
                recordModels.StatusChangeDate = DateTime.Now;

                await _scr.InsertOneAsync(recordModels);
            }
        }

        public async Task DeleteDevice(string deviceID)
        {
            await _devices.DeleteOneAsync(dev => dev.DeviceID == deviceID);
            await _measurements.DeleteManyAsync(m => m.DeviceID == deviceID);
        }
        public async Task DEV_CLEAR_COLLECTION()
        {
            await _devices.DeleteManyAsync(Builders<DeviceModels>.Filter.Empty);
        }
        public async Task DEV_CLEAR_RECORDS()
        {
            await _scr.DeleteManyAsync(Builders<RecordModels>.Filter.Empty);
        }
    }
}
