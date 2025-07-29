namespace PlaywrightAutomation.Models;

public class Asset
{
    public string? AssetTag { get; set; }
    public string? Model { get; set; }
    public string? Manufacturer { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public string? CheckedOutTo { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Notes { get; set; }

    public Asset()
    {
        CreatedDate = DateTime.Now;
    }
}
