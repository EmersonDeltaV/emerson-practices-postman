public record PersonEntry(string NamePrefix, string FirstName, string LastName, string EmailAddress, DateTime BirthDate, string FavoriteColor);

public class Person
{
    public Person(PersonEntry personEntry)
    {
        Id = Guid.NewGuid();
        NamePrefix = personEntry.NamePrefix;
        FirstName = personEntry.FirstName;
        LastName = personEntry.LastName;
        EmailAddress = personEntry.EmailAddress;
        BirthDate = personEntry.BirthDate;
        FavoriteColor = personEntry.FavoriteColor;
    }

    public Person()
    {
    }

    public Guid Id { get; set; }
    public string NamePrefix { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age => CalculateAge(BirthDate);
    public string FavoriteColor { get; set; }
    private int? CalculateAge(DateTime? birthDate)
    {
        if (!birthDate.HasValue)
        {
            return null;
        }

        DateTime today = DateTime.Today;
        int age = today.Year - birthDate.Value.Year;
        if (birthDate > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }
}
