using Emerson.Practices.Postman.PeopleDirectoryApp.AuthenticationApi;
using Emerson.Practices.Postman.PeopleDirectoryApp.PeopleApi;
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
var employmentHistory = new List<EmploymentHistory>();

//Add the Apis
app.AddAuthenticationApi();
app.AddPeopleApi(people);
app.AddEmploymentHistoryApi(people, employmentHistory);

app.Run();

