namespace ATMS.DTOs
{
    public class DTO

    {
        public class MainHeaderDto
        {
            public string ProjectName { get; set; } = string.Empty;
            public string OrgCode { get; set; } = string.Empty;
        }
        // =========================================================
        // 1. GetAverageVehicleCount
        // =========================================================
        public class AverageVehicleCountDto
        {
            public string Time { get; set; } = string.Empty;

            public int TrafficCount { get; set; }
        }


        // =========================================================
        // 2. GetVehicleClass
        // =========================================================
        public class VehicleClassDto
        {
            public int Auto { get; set; }

            public int Bike { get; set; }

            public int Car { get; set; }

            public int LCV { get; set; }

            public int Truck { get; set; }

            public int Bus { get; set; }

            public int Pickup { get; set; }

            public int Tractor { get; set; }

            public int Others { get; set; }
        }


        // =========================================================
        // 3. DeviceStatus
        // =========================================================
        public class DeviceStatusDto
        {
            public int Online { get; set; }

            public int Offline { get; set; }
        }


        // =========================================================
        // 4. ViolatedVehicle
        // =========================================================
        public class ViolatedVehicleDto
        {
            public int TotalVehicles { get; set; }

            public int ViolatedVehicles { get; set; }
        }


        // =========================================================
        // 5. ClassCategory
        // =========================================================
        public class ClassCategoryDto
        {
            public string? Location { get; set; }

            public int Auto { get; set; }

            public int Bike { get; set; }

            public int Car { get; set; }

            public int LCV { get; set; }

            public int Truck { get; set; }

            public int Bus { get; set; }

            public int Pickup { get; set; }

            public int Tractor { get; set; }

            public int Others { get; set; }
        }


        // =========================================================
        // 6. LastTransaction
        // =========================================================
        public class LastTransactionDto
        {
            public string? EventId { get; set; }

            public string? EventType { get; set; }

            public string? VehicleClass { get; set; }

            public DateTime? TimeStamp { get; set; }

            public string? ChNo { get; set; }
        }


        // =========================================================
        // 7. PasssingVehicle
        // =========================================================
        public class PassingVehicleDto
        {
            public string? EventType { get; set; }

            public string? VehicleClass { get; set; }

            public DateTime? TimeStamp { get; set; }

            public string? Location { get; set; }

            public string? Image { get; set; }
        }


        // =========================================================
        // 8. ViolationCategory
        // =========================================================
        public class ViolationCategoryDto
        {
            public string? EventType { get; set; }
            public string? ViolationType { get; set; }  // Add this
            public int Count { get; set; }              // Add this
            public string? ColorCode { get; set; }      // Add this
            public int Auto { get; set; }
            public int Bike { get; set; }
            public int Car { get; set; }
            public int LCV { get; set; }
            public int Truck { get; set; }
            public int Bus { get; set; }
            public int Pickup { get; set; }
            public int Tractor { get; set; }
            public int Others { get; set; }
        }
        // =========================================================
        // 9. GetTrafficCount
        // =========================================================
        public class TrafficCountDto
        {
            public string? ChNo { get; set; }

            public int TrafficCount { get; set; }
        }


        // =========================================================
        // 10. Stored Procedure Request DTO
        // =========================================================
        public class VidsDataRequestDto
        {
            public string Mode { get; set; } = string.Empty;
        }


        // =========================================================
        // 11. Complete VehicleEventDetails DTO
        // =========================================================
        public class VehicleEventDetailsDto
        {
            public int Id { get; set; }

            public string? DeviceName { get; set; }

            public string? ChNo { get; set; }

            public string? Direction { get; set; }

            public string? VehicleNumber { get; set; }

            public decimal? Speed { get; set; }

            public string? VehicleClass { get; set; }

            public string? Make { get; set; }

            public string? EventType { get; set; }

            public DateTime? TimeStamp { get; set; }

            public string? Location { get; set; }

            public string? Lane { get; set; }

            public string? Image { get; set; }

            public string? Image1 { get; set; }

            public string? ReviewStatus { get; set; }

            public string? ReviewRemark { get; set; }

            public DateTime? CreatedOn { get; set; }

            public DateTime? CreatedAt { get; set; }

            public int? CreatedById { get; set; }

            public DateTime? LastModifiedAt { get; set; }

            public int? LastModifiedById { get; set; }

            public int? EventId { get; set; }

            public string? Video { get; set; }

            public bool? IsCleared { get; set; }
        }
        // Add these to your existing DTO class
        public class VIDSReportViewModel
        {
            public MainHeaderDto Header { get; set; }
            public List<AverageVehicleCountDto> AverageVehicleCount { get; set; }
            public VehicleClassDto VehicleClass { get; set; }
            public DeviceStatusDto DeviceStatus { get; set; }
            public ViolatedVehicleDto ViolatedVehicle { get; set; }
            public List<ClassCategoryDto> ClassCategory { get; set; }
            public LastTransactionDto LastTransaction { get; set; }
            public List<PassingVehicleDto> PassingVehicles { get; set; }
            public List<ViolationCategoryDto> ViolationCategories { get; set; }
            public List<TrafficCountDto> TrafficCounts { get; set; }
            public DateTime ReportDate { get; set; }
            public string ReportGeneratedBy { get; set; }
            public string ReportTitle { get; set; }
        }
    }
}

