namespace Emerson.Practices.Postman.PeopleDirectoryApp.AuthenticationApi
{
    public static class AuthenticationApi
    {
        public static void AddAuthenticationApi(this WebApplication app)
        {
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
        }
    }
}
