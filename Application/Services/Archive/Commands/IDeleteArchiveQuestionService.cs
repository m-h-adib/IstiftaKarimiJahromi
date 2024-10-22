using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Archive.Commands;

public interface IDeleteArchiveQuestionService
{
    Task<ResultDto> ExecuteAsync(long? Id);
}

public class DeleteArchiveQuestionService: IDeleteArchiveQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public DeleteArchiveQuestionService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(long? Id)
    {
        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "DeleteArchiveQuestion"))
            return ResultDto.Failed("NOT ACCESS", 403);


        var res = await _context.Questions
            .Where(p => !p.IsRemoved)
            .Where(p => p.Id == Id)
            .FirstOrDefaultAsync();

        if (res == null)
            return ResultDto.Failed("سوال یافت نشد", 404);

        res.IsRemoved = true;
        res.RemovedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}