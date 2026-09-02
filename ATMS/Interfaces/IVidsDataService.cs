using static ATMS.DTOs.DTO;

namespace ATMS.Interfaces
{
    public interface IVidsDataService
    {

        // Get hourly traffic count for today
        Task<IEnumerable<AverageVehicleCountDto>>
            GetAverageVehicleCountAsync();
        Task<MainHeaderDto> GetActiveProjectHeaderAsync();

        // Get vehicle class count
        Task<VehicleClassDto?>
            GetVehicleClassAsync();


        // Get VIDS device online/offline status
        Task<DeviceStatusDto?>
            GetDeviceStatusAsync();


        // Get total and violated vehicles
        Task<ViolatedVehicleDto?>
            GetViolatedVehicleAsync();


        // Get vehicle class category by location/channel
        Task<IEnumerable<ClassCategoryDto>>
            GetClassCategoryAsync();


        // Get latest violation transaction
        Task<LastTransactionDto?>
            GetLastTransactionAsync();


        // Get latest 5 passing/violated vehicles
        Task<IEnumerable<PassingVehicleDto>>
            GetPassingVehiclesAsync();


        // Get violation category-wise vehicle count
        Task<IEnumerable<ViolationCategoryDto>>
            GetViolationCategoryAsync();


        // Get traffic count by channel
        Task<IEnumerable<TrafficCountDto>>
            GetTrafficCountAsync();
    }
}
    