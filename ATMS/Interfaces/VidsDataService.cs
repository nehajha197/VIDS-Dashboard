using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

using Microsoft.Data.SqlClient;
using ATMS.Interfaces;
using static ATMS.DTOs.DTO;

namespace ATMS.Services
{
    public class VidsDataService : IVidsDataService
    {
        private readonly IConfiguration _configuration; 

        public VidsDataService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task<IEnumerable<AverageVehicleCountDto>> GetAverageVehicleCountAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<AverageVehicleCountDto>(
                "sp_GetVIDSData",
                new { Mode = "GetAverageVehicleCount" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<VehicleClassDto?> GetVehicleClassAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<VehicleClassDto>(
                "sp_GetVIDSData",
                new { Mode = "GetVehicleClass" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<DeviceStatusDto?> GetDeviceStatusAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<DeviceStatusDto>(
                "sp_GetVIDSData",
                new { Mode = "DeviceStatus" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ViolatedVehicleDto?> GetViolatedVehicleAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ViolatedVehicleDto>(
                "sp_GetVIDSData",
                new { Mode = "ViolatedVehicle" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ClassCategoryDto>> GetClassCategoryAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ClassCategoryDto>(
                "sp_GetVIDSData",
                new { Mode = "ClassCategory" },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<LastTransactionDto?> GetLastTransactionAsync()
        {
            try
            {
                using var connection = CreateConnection();

                // Use dynamic to handle the GUID properly
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "sp_GetVIDSData",
                    new { Mode = "LastTransaction" },
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return GetDefaultLastTransaction();
                }

                // Safely map properties with null checks
                return new LastTransactionDto
                {
                    EventId = result.EventId?.ToString(),  // Convert GUID to string
                    EventType = result.EventType?.ToString(),
                    VehicleClass = result.VehicleClass?.ToString(),
                    TimeStamp = result.TimeStamp != null ? Convert.ToDateTime(result.TimeStamp) : (DateTime?)null,
                    ChNo = result.ChNo?.ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLastTransactionAsync: {ex.Message}");
                return GetDefaultLastTransaction();
            }
        }

        private LastTransactionDto GetDefaultLastTransaction()
        {
            return new LastTransactionDto
            {
                EventId = null,
                EventType = "Wrong direction",
                VehicleClass = "Bike",
                TimeStamp = DateTime.Now,
                ChNo = "Gate 3"
            };
        }


        public async Task<IEnumerable<PassingVehicleDto>> GetPassingVehiclesAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<PassingVehicleDto>(
                "sp_GetVIDSData",
                new { Mode = "PasssingVehicle" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ViolationCategoryDto>> GetViolationCategoryAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ViolationCategoryDto>(
                "sp_GetVIDSData",
                new { Mode = "ViolationCategory" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<TrafficCountDto>> GetTrafficCountAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<TrafficCountDto>(
                "sp_GetVIDSData",
                new { Mode = "GetTrafficCount" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<MainHeaderDto> GetActiveProjectHeaderAsync()
        {
            using var connection = CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<MainHeaderDto>(
                "sp_GetVIDSData",
                new { Mode = "GetProjectHeader" },
                commandType: CommandType.StoredProcedure
            );

            return result ?? new MainHeaderDto
            {
                ProjectName = "N/A",
                OrgCode = "N/A"
            };
        }

        private IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new SqlConnection(connectionString);
        }
    }
}