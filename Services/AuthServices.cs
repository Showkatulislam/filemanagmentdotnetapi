using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using filemanagementapi.Context;
using filemanagementapi.Domain.UserModel;
using filemanagementapi.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace filemanagementapi.Services;
public class AuthServices : IAuth
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _config;
    public AuthServices(DatabaseContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    public bool Register(UserModel userModel)
    {

        try
        {
            _context.userModels.Add(userModel);
            return true;
        }
        catch (System.Exception)
        {

            return false;
        }
    }
    public Tuple<int, string> GetToken(UserModel userModel)
    {

        var claim = new[]{
            new Claim(JwtRegisteredClaimNames.Sub,_config["Jwt:key"]),
            new Claim("Id",userModel.Id.ToString()),
            new Claim("UserName",userModel.UserName),
            new Claim("Email",userModel.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));

        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claim,
            expires: DateTime.UtcNow.AddHours(5),
           signingCredentials: signIn

        );
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return new Tuple<int, string>(1, jwtToken);
    }
}
