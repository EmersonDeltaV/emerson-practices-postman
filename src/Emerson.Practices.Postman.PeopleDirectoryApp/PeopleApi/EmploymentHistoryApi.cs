namespace Emerson.Practices.Postman.PeopleDirectoryApp.PeopleApi
{
    public static class EmploymentHistoryApi
    {
        public static void AddEmploymentHistoryApi(this WebApplication app, List<Person> people, List<EmploymentHistory> employmentHistory)
        {
            app.MapPost("/people/{personId}/employmenthistory", (Guid personId, EmploymentHistoryEntry entry) =>
            {
                var person = people.FirstOrDefault(p => p.Id == personId);
                if (person == null) return Results.NotFound();

                var history = new EmploymentHistory
                {
                    Id = Guid.NewGuid(),
                    PersonId = personId,
                    CompanyName = entry.CompanyName,
                    JobTitle = entry.JobTitle,
                    JobType = entry.JobType,
                    PhoneNumber = entry.PhoneNumber,
                    StreetAddress = entry.StreetAddress,
                    City = entry.City,
                    Country = entry.Country
                };

                employmentHistory.Add(history);
                return Results.Created($"/people/{personId}/employmenthistory/{history.Id}", history);
            })
            .Produces<EmploymentHistory>()
            .WithTags("EmploymentHistory")
            .WithGroupName("Main")
            .RequireAuthorization();

            app.MapDelete("/people/{personId}/employmenthistory", (Guid personId) =>
            {
                var history = employmentHistory.Where(h => h.PersonId == personId).ToList();
                if (history.Count == 0) return Results.NotFound();

                employmentHistory.RemoveAll(h => h.PersonId == personId);
                return Results.NoContent();
            })
            .WithTags("EmploymentHistory")
            .WithGroupName("Main")
            .RequireAuthorization();

            app.MapDelete("/people/{personId}/employmenthistory/{id}", (Guid personId, Guid id) =>
            {
                var history = employmentHistory.FirstOrDefault(h => h.PersonId == personId && h.Id == id);
                if (history == null) return Results.NotFound();

                employmentHistory.Remove(history);
                return Results.NoContent();
            })
            .WithTags("EmploymentHistory")
            .WithGroupName("Main")
            .RequireAuthorization();

            app.MapGet("/people/{personId}/employmenthistory", (Guid personId) =>
            {
                var history = employmentHistory.Where(h => h.PersonId == personId).ToList();
                return Results.Ok(new SummarizedList<EmploymentHistory>(history));
            })
            .Produces<SummarizedList<EmploymentHistory>>()
            .WithTags("EmploymentHistory")
            .WithGroupName("Main")
            .RequireAuthorization();

            app.MapGet("/people/{personId}/employmenthistory/{id}", (Guid personId, Guid id) =>
            {
                var history = employmentHistory.FirstOrDefault(h => h.PersonId == personId && h.Id == id);
                if (history == null) return Results.NotFound();

                return Results.Ok(history);
            })
            .Produces<EmploymentHistory>()
            .WithTags("EmploymentHistory")
            .WithGroupName("Main")
            .RequireAuthorization();

            app.MapPut("/people/{personId}/employmenthistory/{id}", (Guid personId, Guid id, EmploymentHistoryEntry entry) =>
            {
                var history = employmentHistory.FirstOrDefault(h => h.PersonId == personId && h.Id == id);
                if (history == null) return Results.NotFound();

                history.CompanyName = entry.CompanyName;
                history.JobTitle = entry.JobTitle;
                history.JobType = entry.JobType;
                history.PhoneNumber = entry.PhoneNumber;
                history.StreetAddress = entry.StreetAddress;
                history.City = entry.City;
                history.Country = entry.Country;

                return Results.Ok(history);
            })
            .Produces<EmploymentHistory>()
            .WithTags("EmploymentHistory")
            .WithGroupName("Main")
            .RequireAuthorization();
        }
    }
}
