using System;

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
    public DateTime PurchaseDate { get; set; }
    public decimal PurchaseCost { get; set; }
    public string? Notes { get; set; }

    public Asset()
    {
        CreatedDate = DateTime.Now;
        PurchaseDate = DateTime.Now;
        PurchaseCost = 0;
    }
}

public class User
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Department { get; set; }
    public string? Location { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}
