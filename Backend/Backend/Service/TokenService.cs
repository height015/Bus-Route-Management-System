using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service.Contacts;
using Microsoft.IdentityModel.Tokens;
using BRMSAPI.Domain;

namespace Service.ServiceManager;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _symmetricSecurityKey;
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));


    }
    public string GenerateToken(Passengers appUserObj, string[] roles)
    {
        var claims = new List<Claim>
        {
  
            new Claim(ClaimTypes.Email, appUserObj.Email),
            new Claim(ClaimTypes.GivenName, appUserObj.UserName)
        };

        // Add roles to claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = _configuration["Token:Issuer"],
            Audience = _configuration["Token:JwtIssuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token);
    }

    public void InvalidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Issuer"]));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidateAudience = true
        };
        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        // You can add the validated token to a blacklist or revoke it in the database
    }

}

