public record EmploymentHistoryEntry(string CompanyName, string JobTitle, string JobType, string PhoneNumber, string StreetAddress, string City, string Country);

public class EmploymentHistory
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }

    public string CompanyName { get; set; }
    public string JobTitle { get; set; }
    public string JobType { get; set; }
    public string PhoneNumber { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}
