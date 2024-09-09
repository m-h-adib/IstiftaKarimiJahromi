using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User.Commands;

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
        if (!UserId.HasValue)
            return ResultDto<string>.Failed("کد کاربر را ارسال نمایید");

        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == UserId);

        if (user == null)
            return ResultDto<string>.Failed("کاربری یافت نشد");

        Random rdm=new Random();
        rdm.Next(10000, 99999);

        PasswordHasher hasher = new PasswordHasher();
        string pass = hasher.HashPassword(rdm.ToString());

        user.Password = pass;
        user.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return ResultDto<string>.Succeeded("رمز با موفقیت تغییر یافت", pass, 200);
    }
}
