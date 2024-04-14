using Application.Services.Authentication.Methods;
using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication.Commands;

public interface ILoginService
{
    Task<ResultDto<LoginResponseDto>> ExecuteAsync(LoginDto loginDto);
}

public class LoginService : ILoginService
{
	#region members
	private readonly IDatabaseContext _context;
    private readonly IBuildToken _buildToken;
    #endregion
    #region ctors
    public LoginService(IDatabaseContext context, IBuildToken buildToken)
    {
        _context = context;
        _buildToken = buildToken;
    }
    #endregion

    public async Task<ResultDto<LoginResponseDto>> ExecuteAsync(LoginDto loginDto)
    {
        var item = await _context.Users.FirstOrDefaultAsync(p => p.Username == loginDto.username);


        PasswordHasher hasher = new PasswordHasher();
        if(hasher.VerifyHashedPassword(item.Password, loginDto.password) == Microsoft.AspNet.Identity.PasswordVerificationResult.Failed)
        {
            return ResultDto<LoginResponseDto>.Failed("رمز عبور اشتباه می باشد", 403);
        }

        #region claims
        var claims = new List<Claim>
                {
                new Claim (ClaimTypes.NameIdentifier,item.Id.ToString()),
                new Claim(ClaimTypes.Role,item.Role.ToString()),
                    new Claim (ClaimTypes.Name, ""),
                };

       
        var data = _buildToken.Execute(new BuildTokenDto { Claims = claims });
        #endregion


        var res = new LoginResponseDto
        {
            Token = data.AccessToken,
            Name =item.Name,
			Status=1
		};


		return ResultDto<LoginResponseDto>.Succeeded("ok", res);
    }
}

public class LoginDto
{
    public string username { get; set; }
    public string password { get; set; }
}

public class LoginResponseDto
{
    public string? Token { get; set; }
    public string? Name { get; set; }
    public int? Status { get; set; }
}