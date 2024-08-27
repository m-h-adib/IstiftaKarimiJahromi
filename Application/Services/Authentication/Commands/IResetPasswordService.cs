using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication.Commands;

public interface IResetPasswordService
{
    Task<ResultDto<string>> ExecuteAsync(long? UserId);
}

public class ResetPasswordService: IResetPasswordService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public ResetPasswordService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto<string>> ExecuteAsync(long? UserId)
    {
        if (UserId == null)
            return ResultDto<string>.Failed("bad request");

        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == UserId);

        if (user == null)
            return ResultDto<string>.Failed("کاربر یافت نشد");

        Random random = new Random();
        int pass=random.Next(99999);
        string password = pass.ToString("D5");

        PasswordHasher hash = new PasswordHasher();
        string hashpass = hash.HashPassword(password);

        user.Password=hashpass;
        await _context.SaveChangesAsync();

        return ResultDto<string>.Succeeded("ok", password, 200);
    }
}