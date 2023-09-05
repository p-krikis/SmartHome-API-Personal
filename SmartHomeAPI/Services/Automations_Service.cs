using Amazon.Util;
using MongoDB.Driver;
using SmartHomeAPI.Models;

namespace SmartHomeAPI.Services
{
    public class Automations_Service
    {
        private readonly IMongoCollection<AutomationModels> _auto;

        public Automations_Service()
        {
            MongoClient client = new("mongodb+srv://<USERNAME>:<PASSWORD>@smarthometest.jpiqofv.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase db = client.GetDatabase("SmartHomeDB");
            _auto = db.GetCollection<AutomationModels>("Automations");
        }
        public async Task AddAutomation(AutomationModels automation)
        {
            _auto.InsertOneAsync(automation);
        }
        public async Task<List<AutomationModels>> FetchAllAutomations(string devID)
        {
            var autoList = await _auto.Find(a => a.DevID == devID).ToListAsync();
            return autoList;
        }
        public async Task<List<AutomationModels>> FetchSingleAutomatio(string autoID)
        {
            var autoData = await _auto.Find(ad => ad.AutomationID == autoID).ToListAsync();
            return autoData;
        }
        public async Task UpdateAutomation (string autoID, AutomationModels automationTBU)
        {
            await _auto.ReplaceOneAsync(a => a.AutomationID == autoID, automationTBU);
        }
        public async Task DeleteAutomation (string autoID)
        {
            await _auto.DeleteOneAsync(a => a.AutomationID == autoID);
        }
    }
}
