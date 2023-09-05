using MongoDB.Driver;
using SmartHomeAPI.Models;

namespace SmartHomeAPI.Services
{
    public class Rooms_Service
    {
        private readonly IMongoCollection<RoomModels> _rooms;
        private readonly IMongoCollection<DeviceModels> _devices;
        private readonly IMongoCollection<MeasurementModels> _measurements;

        public Rooms_Service()
        {
            MongoClient client = new("mongodb+srv://<USERNAME>:<PASSWORD>@smarthometest.jpiqofv.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = client.GetDatabase("SmartHomeDB");
            _rooms = db.GetCollection<RoomModels>("Rooms");
            _devices = db.GetCollection<DeviceModels>("Devices");
            _measurements = db.GetCollection<MeasurementModels>("Measurements");
        }
        public async Task AddRoom(RoomModels room)
        {
            await _rooms.InsertOneAsync(room);
            return;
        }
        public async Task<List<RoomReturns>> GetAllRoomsOfHouse(string houseID)
        {
            var rooms = await _rooms.Find(r => r.HouseID == houseID).ToListAsync();
            List<RoomReturns> roomInfo = new List<RoomReturns>();
            foreach (var room in rooms)
            {
                roomInfo.Add(new RoomReturns
                {
                    RoomID = room.RoomID,
                    RoomName = room.RoomName,
                    HouseID = null                    
                });
            }
            return roomInfo;
        }
        public async Task<List<RoomReturns>> GetAllRoomsOfUser(string userID)
        {
            var userRooms = await _rooms.Find(r => r.RoomUserID == userID).ToListAsync();
            List<RoomReturns> userRoomInfo = new List<RoomReturns>();
            foreach (var userRoom in userRooms)
            {
                userRoomInfo.Add(new RoomReturns
                {
                    RoomID = userRoom.RoomID,
                    RoomName = userRoom.RoomName,
                    HouseID = userRoom.HouseID
                });
            }
            return userRoomInfo;
        }
        public async Task<List<RoomReturns>> GetSpecificRoom(string roomID)
        {
            var roomInfoList = await _rooms.Find(r => r.RoomID == roomID).ToListAsync();
            List <RoomReturns> singleRoomInfo = new List<RoomReturns>();
            foreach (var roomInfo in roomInfoList)
            {
                singleRoomInfo.Add(new RoomReturns
                {
                    RoomID = roomInfo.RoomID,
                    RoomName = roomInfo.RoomName,
                    HouseID = null
                });
            }
            return singleRoomInfo;
        }
        public async Task UpdateRoom(string roomID, RoomModels roomTBU)
        {
            await _rooms.ReplaceOneAsync(r => r.RoomID == roomID, roomTBU);
        }
        public async Task DeleteRoom(string roomID)
        {
            var devList = await _devices.Find(d => d.RoomID == roomID).ToListAsync();
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
                device.HouseID = dev.HouseID;
                device.DevOwnerID = dev.DevOwnerID;
                device.Mode = dev.Mode;
                device.Status = dev.Status;
                device.TargetTemp = dev.TargetTemp;
                device.Timer = dev.Timer;
                await _devices.ReplaceOneAsync(d => d.RoomID == roomID, device);
            }

            var measList = await _measurements.Find(m => m.RoomID == roomID).ToListAsync();
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
                measurement.HouseID = meas.HouseID;
                measurement.UserID = meas.UserID;
                await _measurements.ReplaceOneAsync(m => m.RoomID == roomID, measurement);
            }

            await _rooms.DeleteOneAsync(r => r.RoomID == roomID);

            //await _devices.DeleteManyAsync(d => d.RoomID == roomID);
            //await _measurements.DeleteManyAsync(m => m.RoomID == roomID);
        }
        public async Task DEV_CLEAR_COLLECTION()
        {
            await _rooms.DeleteManyAsync(Builders<RoomModels>.Filter.Empty);
        }
    }
}
