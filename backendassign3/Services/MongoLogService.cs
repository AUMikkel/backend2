using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backendassign3.DTOs;

namespace backendassign3.Services
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
    public class MongoLogService
    {
        private readonly IMongoCollection<ServiceDto.LogDto> _logsCollection;

        public MongoLogService(IOptions<MongoDBSettings> mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _logsCollection = database.GetCollection<ServiceDto.LogDto>(mongoSettings.Value.CollectionName);
        }

        public async Task<List<ServiceDto.LogDto>> GetLogsAsync(DateTime startTime, DateTime endTime, string? user = null, string? operation = null)
        {
            Console.WriteLine("Inside GetLogsAsync");
            var builder = Builders<ServiceDto.LogDto>.Filter;
            // Base filter for the date range
            var filter = builder.Gte(log => log.Timestamp, startTime) &
                         builder.Lte(log => log.Timestamp, endTime);

            // Add user filter if provided
            if (!string.IsNullOrEmpty(user))
            {
                filter &= builder.Eq(log => log.Properties.LogInfo.User, user);
            }

            // Add operation filter if provided
            if (!string.IsNullOrEmpty(operation))
            {
                filter &= builder.Eq(log => log.Properties.LogInfo.Operation, operation);
            }
            return await _logsCollection.Find(filter).ToListAsync();
        }
    }
}