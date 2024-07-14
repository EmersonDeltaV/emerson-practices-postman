namespace Emerson.Practices.Postman.PeopleDirectoryApp.PeopleApi
{
    public static class PeopleApi
    {
        public static void AddPeopleApi(this WebApplication app, List<Person> people)
        {
            app.MapPost("/people", (PersonEntry entry) =>
            {
                if (Helper.Validate(entry))
                {
                    var person = new Person(entry);
                    people.Add(person);
                    return Results.Created($"/people/{person.Id}", person);
                }
                else
                {
                    return Results.BadRequest("Invalid person data");
                }
            })
            .Produces<Person>()
            .WithTags("People")
            .WithGroupName("Main")
            .RequireAuthorization();

            app.MapDelete("/people", () =>
            {
                people.Clear();
                return Results.NoContent();
            })
            .Produces<Person>()
            .WithTags("People")
            .WithGroupName("Main")
            .WithOpenApi(n => new(n)
            {
                Description = "Delete all records",
                Summary = "Delete all records"
            })
            .RequireAuthorization();

            app.MapGet("/people", () =>
            {
                return Results.Ok(new SummarizedList<Person>(people));
            })
            .Produces<SummarizedList<Person>>()
            .WithTags("People")
            .WithGroupName("Main")
            .RequireAuthorization();


            app.MapGet("/people/{id}", (Guid? id) =>
            {
                var person = people.FirstOrDefault(p => p.Id == id);
                if (person == null) return Results.NotFound();

                return Results.Ok(person);
            })
            .Produces<Person>()
            .WithTags("People")
            .WithGroupName("Main")
            .RequireAuthorization();

            app.MapPut("/people/{id}", (Guid? id, PersonEntry entry) =>
            {
                var person = people.FirstOrDefault(p => p.Id == id);
                if (person == null) return Results.NotFound();

                if (!Helper.Validate(entry)) return Results.BadRequest("Invalid person data");

                person.NamePrefix = entry.NamePrefix;
                person.FirstName = entry.FirstName;
                person.LastName = entry.LastName;
                person.EmailAddress = entry.EmailAddress;
                person.BirthDate = entry.BirthDate;
                person.FavoriteColor = entry.FavoriteColor;
                return Results.Ok(person);
            })
            .Produces<Person>()
            .WithTags("People")
            .WithGroupName("Main")
            .RequireAuthorization();

            app.MapDelete("/people/{id}", (Guid? id) =>
            {
                var person = people.FirstOrDefault(p => p.Id == id);
                if (person == null) return Results.NotFound();

                people.Remove(person);
                return Results.NoContent();
            })
            .WithTags("People")
            .WithGroupName("Main")
            .RequireAuthorization();
        }
    }
}
