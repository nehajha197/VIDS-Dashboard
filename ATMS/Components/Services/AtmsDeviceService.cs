namespace ATMS.Components.Services
{
    using ATMS.DTOs;
    using Dapper;

    using System.Data;

    public class AtmsDeviceService
    {
        private readonly IDbConnection _connection;

        public AtmsDeviceService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<AtmsDeviceDto>> GetDevicesAsync(AtmsDeviceFilterDto filter)
        {
            return await _connection.QueryAsync<AtmsDeviceDto>(
                "EXEC dbo.GetAtmsDevices",
                filter
            );
        }
        public async Task<int> AddDeviceAsync(AtmsDeviceCreateDto device)
        {
            return await _connection.ExecuteScalarAsync<int>(
                "EXEC dbo.InsertAtmsDevice @Name, @DeviceType, @Latitude, @Longitude, @Status, @DeviceEndpoint, @Direction, @CreatedById",
                device
            );
        }
    }
}

