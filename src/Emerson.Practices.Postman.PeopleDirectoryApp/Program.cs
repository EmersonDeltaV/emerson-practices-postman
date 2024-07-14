using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("authentication", new() { Title = "Authentication API", Version = "v1" });
    c.SwaggerDoc("main", new() { Title = "PeopleDirectory API", Version = "v1" });
    c.DocInclusionPredicate((name, api) =>
    {
        return name.Equals(api.GroupName, StringComparison.InvariantCultureIgnoreCase);
    });
});
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/authentication/swagger.json", "Authentication");
        c.SwaggerEndpoint("/swagger/main/swagger.json", "Main");
    });
}

app.UseHttpsRedirection();

var people = new List<Person>();

app.MapPost("/login", (UserCredentials cred) =>
{
    if (cred.Username == "admin")
    {
        var newToken = Helper.GenerateToken(cred.Username, app.Configuration);
        return Results.Ok(new LoginResponse { AccessToken = newToken });
    }
    else
    {
        return Results.BadRequest("Invalid username or password");
    }
})
.Produces<LoginResponse>()
.WithTags("Authorization")
.WithGroupName("Authentication")
.WithOpenApi();

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

app.Run();

