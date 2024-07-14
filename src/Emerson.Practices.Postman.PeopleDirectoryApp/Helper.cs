using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

public class Helper
{
    public static bool Validate(PersonEntry person)
    {
        if (string.IsNullOrEmpty(person.FirstName))
        {
            return false;
        }

        if (!IsValidEmail(person.EmailAddress))
        {
            return false;
        }

        return true;
    }

    public static bool IsValidEmail(string email)
    {
        // Use regular expression to validate email address
        // You can use a library like FluentEmail or System.Net.Mail for more robust email validation
        // This is a simple example using regular expression
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    public static string GenerateToken(string username, IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username)
        };

        var expiry = DateTime.Now.AddDays(1);
        var token = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiry,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(token);
        return tokenHandler.WriteToken(securityToken);
    }
}