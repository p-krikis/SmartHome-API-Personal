using MongoDB.Driver;
using SmartHomeAPI.Models;
using System.Diagnostics.Metrics;

namespace SmartHomeAPI.Services
{
    public class Houses_Service 
    {
        private readonly IMongoCollection<HouseModels> _houses;
        private readonly IMongoCollection<RoomModels> _rooms;
        private readonly IMongoCollection<DeviceModels> _devices;
        private readonly IMongoCollection<MeasurementModels> _measurements;

        public Houses_Service() 
        {
            MongoClient client = new("mongodb+srv://<USERNAME>:<PASSWORD>@smarthometest.jpiqofv.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = client.GetDatabase("SmartHomeDB");
            _houses = db.GetCollection<HouseModels>("Houses");
            _rooms = db.GetCollection<RoomModels>("Rooms");
            _devices = db.GetCollection<DeviceModels>("Devices");
            _measurements = db.GetCollection<MeasurementModels>("Measurements");
        }
        public async Task<string> AddHouse (HouseModels house) //checks if house with same name exists, then adds
        {
            var houseDB = await _houses.Find(h => h.HouseName == house.HouseName).FirstOrDefaultAsync();
            if (houseDB != null)
            {
                return null;
            }
            else
            {
                await _houses.InsertOneAsync(house);
                return "OK";
            }
        }
        public async Task<List<HouseInfo>> GetAllHouses(string userID) //returns all houses connected to userID
        {
            var houses = await _houses.Find(h => h.HouseOwnerID == userID).ToListAsync();
            List<HouseInfo> houseInfo = new List<HouseInfo>();
            foreach (var house in houses)
            {
                houseInfo.Add(new HouseInfo()
                {
                    HouseID = house.HouseID,
                    HouseName = house.HouseName
                });
            }
            return houseInfo;
        }
        public async Task<List<HouseInfo>> GetSpecificHouse(string houseID) //returns specific house by houseID
        {
            var houseData = await _houses.Find(h => h.HouseID == houseID).ToListAsync();
            List<HouseInfo> houseInfos = new List<HouseInfo>();
            foreach ( var house in houseData)
            {
                houseInfos.Add(new HouseInfo()
                {
                    HouseID = house.HouseID,
                    HouseName = house.HouseName
                });
            }
            return houseInfos;
        }
        public async Task UpdateHouse(string houseID, HouseModels houseTBU) //updates house
        {
            await _houses.ReplaceOneAsync(h => h.HouseID == houseID, houseTBU);
        }
        public async Task DeleteHouse(string houseID) //deletes house, rooms, sets existing devices and measurements to unallocated
        {
            
            var devList = await _devices.Find(d => d.HouseID == houseID).ToListAsync();
            DeviceModels device = new DeviceModels();
            foreach (var dev in devList)
            {
                device.DeviceID = dev.DeviceID;
                device.DeviceName = dev.DeviceName;
                device.DeviceCategory = dev.DeviceCategory;
                    device.MeasurementID = dev.MeasurementID;
                device.Latitude = dev.Latitude;
                device.Longitude = dev.Longitude;
                device.RoomID = "unallocated";
                device.RoomName = "unallocated";
                device.HouseID = "unallocated";
                device.DevOwnerID = dev.DevOwnerID;
                device.Mode = dev.Mode;
                device.Status = dev.Status;
                device.TargetTemp = dev.TargetTemp;
                device.Timer = dev.Timer;
                await _devices.ReplaceOneAsync(d => d.HouseID == houseID, device);
            }

            var measList = await _measurements.Find(m => m.HouseID == houseID).ToListAsync();
            MeasurementModels measurement = new MeasurementModels();
            foreach (var meas in measList)
            {
                measurement.UniqueMeasurementID = meas.UniqueMeasurementID;
                measurement.MeasurementID = meas.MeasurementID;

                measurement.Meas1_Name = meas.Meas1_Name;
                measurement.Meas1_Unit = meas.Meas1_Unit;
                measurement.Meas1_UnitFull = meas.Meas1_UnitFull;
                measurement.Meas1_Value = meas.Meas1_Value;

                measurement.Meas2_Name = meas.Meas2_Name;
                measurement.Meas2_Unit = meas.Meas2_Unit;
                measurement.Meas2_UnitFull = meas.Meas2_UnitFull;
                measurement.Meas2_Value = meas.Meas2_Value;

                measurement.Meas3_Name = meas.Meas3_Name;
                measurement.Meas3_Unit = meas.Meas3_Unit;
                measurement.Meas3_UnitFull = meas.Meas3_UnitFull;
                measurement.Meas3_Value = meas.Meas3_Value;

                measurement.Meas4_Name = meas.Meas4_Name;
                measurement.Meas4_Unit = meas.Meas4_Unit;
                measurement.Meas4_UnitFull = meas.Meas4_UnitFull;
                measurement.Meas4_Value = meas.Meas4_Value;

                measurement.Meas5_Name = meas.Meas5_Name;
                measurement.Meas5_Unit = meas.Meas5_Unit;
                measurement.Meas5_UnitFull = meas.Meas5_UnitFull;
                measurement.Meas5_Value = meas.Meas5_Value;

                measurement.DeviceID = meas.DeviceID;
                measurement.RoomID = "unallocated";
                measurement.HouseID = "unallocated";
                measurement.UserID = meas.UserID;
                await _measurements.ReplaceOneAsync(m => m.HouseID == houseID, measurement);
            }
            await _houses.DeleteOneAsync(h => h.HouseID == houseID);
            await _rooms.DeleteManyAsync(r => r.HouseID == houseID);

        }
        public async Task DEV_CLEAR_COLLECTION()
        {
            await _houses.DeleteManyAsync(Builders<HouseModels>.Filter.Empty);
        }
    }
}
