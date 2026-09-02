namespace ATMS.DTOs
{
   
        public class AtmsDeviceDto
        {
            public string Name { get; set; } = string.Empty;
            public string DeviceType { get; set; } = string.Empty;
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Status { get; set; } = string.Empty;
            public string DeviceEndpoint { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
        }
    }
public class AtmsDeviceCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Status { get; set; } = "Online";
    public string DeviceEndpoint { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public Guid? CreatedById { get; set; }
}

public class AtmsDeviceFilterDto
{
    public string? DeviceType { get; set; }
    public string? Status { get; set; }
}
