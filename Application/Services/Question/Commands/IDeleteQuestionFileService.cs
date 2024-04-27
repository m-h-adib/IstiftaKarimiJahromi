using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Question.Commands;

public interface IDeleteQuestionFileService
{
    Task<ResultDto> ExecuteAsync(long? QuestionFileId);
}


public class DeleteQuestionFileService: IDeleteQuestionFileService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public DeleteQuestionFileService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion


    public async Task<ResultDto> ExecuteAsync(long? QuestionFileId)
    {
        if (QuestionFileId == null)
            return ResultDto.Failed("Bad Request", 400);

        var res = await _context.QuestionAttaches
            .Where(p => !p.IsRemoved && p.Id == QuestionFileId)
            .FirstOrDefaultAsync();

        if (res == null)
            return ResultDto.Failed("فایل مورد نظر یافت نشد", 404);

        res.IsRemoved = true;
        res.RemovedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}