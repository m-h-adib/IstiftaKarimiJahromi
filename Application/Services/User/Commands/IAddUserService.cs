using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Person.Commands;

public interface IAddUserService
{
    Task<ResultDto> ExecuteAsync(AddAdminDto dto);
}


public class AddUserService: IAddUserService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public AddUserService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddAdminDto dto)
    {
        if (await _context.Users.AnyAsync(p => p.Username == dto.Username && !p.IsRemoved))
            return ResultDto.Failed("کاربری با نام کاربری قبلا در سیستم موجود می باشد");

		#region validator
		var validator = new AddAdminValidator().Validate(dto);
        if (!validator.IsValid)
            return ResultDto.Failed(validator.ErrorMessage());
        #endregion

        PasswordHasher hash=new PasswordHasher();
        string pass=hash.HashPassword(dto.Password);

        var res = await _context.Users.AddAsync(new Domain.Entities.User
        {
            Name = dto.Name,
            Username = dto.Username,
            Password = pass,
            Role=dto.Role!=null ? (RoleType)dto.Role : RoleType.Admin,
            EitaaId=dto.EitaaId
        });

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}

#region dtos
public class AddAdminDto
{
    public string? Name { get; set; }
    public string? Username { get; set; }
	public string? Password { get; set; }
    public int? Role { get; set; }
    public string? EitaaId { get; set; }
}

#endregion

#region validators
public class AddAdminValidator : AbstractValidator<AddAdminDto>
{
    public AddAdminValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("لطفا نام را وارد نمایید");
		RuleFor(p => p.Username).NotEmpty().WithMessage("لطفا نام کاربری را وارد نمایید");
		RuleFor(p => p.Password).NotEmpty().WithMessage("لطفا رمزعبور را وارد نمایید");
        RuleFor(p => p.EitaaId).NotEmpty().WithMessage("لطفا آی دی و یا شناسه ایتا را وارد نمایید");
	}
}
#endregion