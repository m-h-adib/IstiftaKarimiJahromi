using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface ISendMessageService
{
    Task<ResultDto> ExecuteAsync(long userId, string message);
}

public class SendMessageService: ISendMessageService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public SendMessageService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(long userId,string message)
    {
        var user = await _context.Users.FirstOrDefaultAsync(p=>p.Id==userId);
        if(user != null && !string.IsNullOrEmpty(user.EitaaId)) {
            string url = "https://eitaayar.ir/api/bot322886:3890a3f8-de6e-4e37-b723-b480f8d72c43/sendMessage?chat_id="+user.EitaaId+"&text="+message;
            var client = new HttpClient();
            var response = await client.GetAsync(url);
        }


        

        return ResultDto.Succeeded("ok");
    }
}

