using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using SmartHomeAPI.Models;

namespace SmartHomeAPI.Services
{
    public class Measurements_Service
    {
        private readonly IMongoCollection<MeasurementModels> _measurements;
        private readonly IMongoCollection<DataGenModels> _dataGen;

        public Measurements_Service(IOptions<MongoDB_Devices> mongoDBsettings) //placeholder TODO fix this
        {
            MongoClient client = new("mongodb+srv://<USERNAME>:<PASSWORD>@smarthometest.jpiqofv.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = client.GetDatabase("SmartHomeDB");
            _measurements = db.GetCollection<MeasurementModels>("Measurements");
            _dataGen = db.GetCollection<DataGenModels>("Measurements");
        }

        public async Task PostMeasurement(string measurementJSON)
        {
            List<MeasurementModels> measurementList = JsonConvert.DeserializeObject<List<MeasurementModels>>(measurementJSON);
            foreach (var measure in measurementList)
            {
                await _measurements.InsertOneAsync(measure);
            }
            return;
        }
        public async Task<List<MeasurementModels>> GetMeasurements (string deviceID)
        {
            var measurementList = await _measurements.Find(m => m.DeviceID == deviceID).ToListAsync();
            return measurementList;
        }
        public async Task DEV_CLEAR_COLLECTION()
        {
            await _measurements.DeleteManyAsync(Builders<MeasurementModels>.Filter.Empty);
        }
        public async Task<List<MeasurementModels>> GetSpecificPeriod(string deviceID, SpecificPeriodModels specificPeriod)
        {
            var measurements = await _measurements.Find(deviceID).ToListAsync();
            List<MeasurementModels> validMeasurements = new List<MeasurementModels>();
            int startCheck;
            int endCheck;
            if (specificPeriod.From !=null && specificPeriod.To != null)
            {
                foreach (var meas in measurements)
                {
                    startCheck = meas.ValueDate.CompareTo(specificPeriod.From);
                    endCheck = meas.ValueDate.CompareTo(specificPeriod.To);
                    if (startCheck >= 0 && endCheck <= 0)
                    {
                        validMeasurements.Add(meas);
                    }
                }
                return validMeasurements;
            }
            else if (specificPeriod.From == null && specificPeriod.To != null)
            {
                foreach (var meas in measurements)
                {
                    endCheck = meas.ValueDate.CompareTo(specificPeriod.To);
                    if (endCheck <= 0)
                    {
                        validMeasurements.Add(meas);
                    }
                }
                return validMeasurements;
            }
            else if (specificPeriod.From != null && specificPeriod.To == null)
            {
                foreach (var meas in measurements)
                {
                    startCheck = meas.ValueDate.CompareTo(specificPeriod.From);
                    if (startCheck >= 0)
                    {
                        validMeasurements.Add(meas);
                    }
                }
                return validMeasurements;
            }
            else
            {
                return measurements;
            }
        }
        public async Task<MeasurementModels> MostRecent(string deviceID)
        {
            var fullMeasList = await _measurements.Find(m => m.DeviceID == deviceID).ToListAsync();
            List<DateTime> measDates = new List<DateTime>();
            foreach (var meas in fullMeasList)
            {
                measDates.Add(meas.ValueDate);
            }
            DateTime mostRecentDate = measDates.Max();
            var mostRecentMeas = await _measurements.Find(m => m.ValueDate == mostRecentDate).FirstOrDefaultAsync();
            return mostRecentMeas;
        }




        //public async Task GenerateData(int amount, MeasurementModels measurement)
        //{
        //    List<DataGenModels> measurementModels = new List<DataGenModels>();
        //    double? templateValue1 = measurement.Meas1_Value;
        //    double? templateValue2 = measurement.Meas2_Value;
        //    double? templateValue3 = measurement.Meas3_Value;
        //    double? templateValue4 = measurement.Meas4_Value;
        //    double? templateValue5 = measurement.Meas5_Value;
        //    double adjValue1;
        //    double adjValue2;
        //    double adjValue3;
        //    double adjValue4;
        //    double adjValue5;
        //    if (measurement.MeasurementID == 1)
        //    {
        //        for (int i = 0; i < amount; i++)
        //        {
        //            adjValue1 = GenerateRandomValue((double)(templateValue1 - 0.3), (double)(templateValue1 + 0.3));
        //            var roundedV1 = Math.Round(adjValue1, 2);
        //            measurementModels.Add(new DataGenModels
        //            {
        //                MeasurementID = measurement.MeasurementID,
        //                Meas1_Name = measurement.Meas1_Name,
        //                Meas1_Unit = measurement.Meas1_Unit,
        //                Meas1_UnitFull = measurement.Meas1_UnitFull,
        //                Meas1_Value = roundedV1,
        //                DeviceID = measurement.DeviceID,
        //                RoomID = measurement.RoomID,
        //                HouseID = measurement.HouseID,
        //                UserID = measurement.UserID,
        //                ValueDate = DateTime.Now.AddHours(i)
        //            });
        //            templateValue = adjValue;
        //        }
        //    }
        //    else if (measurement.MeasurementID == 2)
        //    {
        //        for (int i = 0; i < amount; i++)
        //        {
        //            if (templateValue < 0)
        //            {
        //                adjValue = GenerateRandomValue(0, templateValue + 25);
        //            }
        //            else
        //            {
        //                adjValue = GenerateRandomValue(templateValue - 25, templateValue + 25);
        //            }
        //            var roundedV = Math.Round(adjValue, 0);
        //            measurementModels.Add(new DataGenModels
        //            {
        //                MeasurementID = measurement.MeasurementID,
        //                MeasurementName = measurement.MeasurementName,
        //                Unit = measurement.Unit,
        //                Value = roundedV,
        //                DeviceID = measurement.DeviceID,
        //                RoomID = measurement.RoomID,
        //                HouseID = measurement.HouseID,
        //                UserID = measurement.UserID,
        //                ValueDate = DateTime.Now.AddHours(i)
        //            });
        //            templateValue = adjValue;
        //        }
        //    }
        //    else if (measurement.MeasurementID == 3)
        //    {
        //        for (int i = 0; i < amount; i++)
        //        {
        //            if (templateValue <= 0)
        //            {
        //                adjValue = GenerateRandomValue(0, templateValue + 3);
        //            }
        //            else
        //            {
        //                adjValue = GenerateRandomValue(templateValue - 3, templateValue + 3);
        //            }
        //            var roundedV = Math.Round(adjValue, 0);
        //            measurementModels.Add(new DataGenModels
        //            {
        //                MeasurementID = measurement.MeasurementID,
        //                MeasurementName = measurement.MeasurementName,
        //                Unit = measurement.Unit,
        //                Value = roundedV,
        //                DeviceID = measurement.DeviceID,
        //                RoomID = measurement.RoomID,
        //                HouseID = measurement.HouseID,
        //                UserID = measurement.UserID,
        //                ValueDate = DateTime.Now.AddHours(-i)
        //            });
        //            templateValue = adjValue;
        //        }
        //    }
        //    foreach (var meas in measurementModels)
        //    {
        //        await _dataGen.InsertOneAsync(meas);
        //    }
        //}
        //private double GenerateRandomValue(double minValue, double maxValue)
        //{
        //    Random random = new Random();
        //    double randomValue = random.NextDouble() * (maxValue - minValue) + minValue;
        //    return randomValue;
        //}
    }
}
