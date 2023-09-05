using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartHomeAPI.Models
{
    public class DeviceModels
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceCategory { get; set; }
        public int MeasurementID { get; set; } //idk
        public double? Latitude { get; set; } //can be null
        public double? Longitude { get; set;} //can be null
        public string? RoomID { get; set; }
        public string? RoomName { get; set; }
        public string HouseID { get; set; }
        public string DevOwnerID { get; set; }
        public string? Mode { get; set; }
        public bool Status { get; set; }
        public int? TargetTemp { get; set; }
        public int? Timer { get; set; }
        

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StatusChangeDate { get; set; }
    }
    public class RecordModels
    {
        public string DevID { get; set; }
        public bool Status { get; set; }
        public DateTime StatusChangeDate { get; set; }
    }

    public class UserModels
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserID { get; set;}
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
    }

    public class LoginInfo
    {
        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }
    }
    public class LoginReturns
    {
        public string AuthedUserid { get; set; }
        public string AuthedUsername { get; set; }
    }

    public class HouseModels
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? HouseID { get; set; }
        public string HouseName { get; set; }
        public string HouseOwnerID { get; set; }
    }

    public class HouseInfo
    {
        public string HouseID { get; set; }
        public string HouseName { get; set;}
    }

    public class RoomModels
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? RoomID { get; set;}
        public string RoomName { get; set;}
        public string RoomCategory { get; set;}
        public string HouseID { get; set; }
        public string RoomUserID { get; set; }
    }
    public class RoomReturns
    {
        public string RoomID { get; set; }
        public string RoomName { get; set;}
        public string? HouseID { get; set; }
    }

    public class MeasurementModels
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UniqueMeasurementID { get; set; }
        public int MeasurementID { get; set; }
        public string? Meas1_Name { get; set; }
        public string? Meas1_Unit { get; set; }
        public string? Meas1_UnitFull { get; set; }
        public double? Meas1_Value { get; set; }
        public string? Meas2_Name { get; set; }
        public string? Meas2_Unit { get; set; }
        public string? Meas2_UnitFull { get; set; }
        public double? Meas2_Value { get; set; }
        public string? Meas3_Name { get; set; }
        public string? Meas3_Unit { get; set; }
        public string? Meas3_UnitFull { get; set; }
        public double? Meas3_Value { get; set; }
        public string? Meas4_Name { get; set; }
        public string? Meas4_Unit { get; set; }
        public string? Meas4_UnitFull { get; set; }
        public double? Meas4_Value { get; set; }
        public string? Meas5_Name { get; set; }
        public string? Meas5_Unit { get; set; }
        public string? Meas5_UnitFull { get; set; }
        public double? Meas5_Value { get; set; }
        public string DeviceID { get; set; }
        public string RoomID { get; set; }
        public string HouseID { get; set; }
        public string UserID { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ValueDate { get; set; }
    }
    public class DataGenModels
    {
        public int MeasurementID { get; set; }
        public string? Meas1_Name { get; set; }
        public string? Meas1_Unit { get; set; }
        public string? Meas1_UnitFull { get; set; }
        public double? Meas1_Value { get; set; }
        public string? Meas2_Name { get; set; }
        public string? Meas2_Unit { get; set; }
        public string? Meas2_UnitFull { get; set; }
        public double? Meas2_Value { get; set; }
        public string? Meas3_Name { get; set; }
        public string? Meas3_Unit { get; set; }
        public string? Meas3_UnitFull { get; set; }
        public double? Meas3_Value { get; set; }
        public string? Meas4_Name { get; set; }
        public string? Meas4_Unit { get; set; }
        public string? Meas4_UnitFull { get; set; }
        public double? Meas4_Value { get; set; }
        public string? Meas5_Name { get; set; }
        public string? Meas5_Unit { get; set; }
        public string? Meas5_UnitFull { get; set; }
        public double? Meas5_Value { get; set; }
        public string DeviceID { get; set; }
        public string RoomID { get; set; }
        public string HouseID { get; set; }
        public string UserID { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ValueDate { get; set; }
    }
    public class AutomationModels
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? AutomationID { get; set; }
        public string DevID { get; set; }
        public bool IsForced { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Programme { get; set; }
        public bool SaveAutomation { get; set; }
    }
    public class SpecificPeriodModels
    {
        public DateTime? From { get; set;}
        public DateTime? To { get; set;}
    }
}
