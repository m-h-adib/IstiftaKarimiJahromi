using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication.Methods;

public interface IBuildToken
{
    ResultBuildTokenDto Execute(BuildTokenDto model);
}

public class BuildToken : IBuildToken
{
    private readonly IConfiguration _configuration;

    public BuildToken(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ResultBuildTokenDto Execute(BuildTokenDto model)
    {
        string key =_configuration["jwtConfig:key"];
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var expire = DateTime.Now.AddMinutes(int.Parse(_configuration["jwtConfig:expires"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["jwtConfig:issuer"],
            audience: _configuration["jwtConfig:audience"],
            expires: expire,
            notBefore: DateTime.Now,
            claims: model.Claims,
            signingCredentials: credentials
            );
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);



        return new ResultBuildTokenDto
        {
            AccessToken = jwtToken,
            ExpireAt = expire
        };
    }
}

#region dtos

public class BuildTokenDto
{
    public ICollection<Claim>? Claims { get; set; }
}

public class ResultBuildTokenDto
{
    public string? AccessToken { get; set; }
    public DateTime ExpireAt { get; set; }
}

#endregion dtos