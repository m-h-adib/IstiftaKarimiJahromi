

using Application.Services.User.Queries;

namespace Application.Common;

public class ResultDto
{
    #region ctors

    public ResultDto()
    {
    }

    public ResultDto(bool isSuccess, string? message, int statusCode)
    {
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
    }

    #endregion ctors

    #region methods

    public static ResultDto Succeeded(string? message, int statusCode = 200)
    {
        return new ResultDto { IsSuccess = true, Message = message, StatusCode = statusCode };
    }

    public static ResultDto Failed(string? message, int statusCode = 400)
    {
        return new ResultDto { IsSuccess = false, Message = message, StatusCode = statusCode };
    }

    #endregion methods

    #region members

    public bool IsSuccess { get; set; }

    public string? Message { get; set; }
    public int StatusCode { get; set; }

    #endregion members
}

public class ResultDto<TData> : ResultDto
{
    #region ctors

    public ResultDto(bool isSuccess, string? message, int statusCode) : base(isSuccess, message, statusCode)
    {
    }

    public ResultDto(bool isSuccess, string? message, TData? data, int statusCode) : base(isSuccess, message, statusCode)
    {
        Message = message;
        Data = data;
        StatusCode = statusCode;
    }

    #endregion ctors

    #region methods

    public static ResultDto<TData> Succeeded(string? message, TData data, int statusCode = 200)
    {
        return new ResultDto<TData>(true, message, data, statusCode);
    }

    public new static ResultDto<TData> Succeeded(string? message, int statusCode = 200)
    {
        return new ResultDto<TData>(true, message, statusCode);
    }

    public new static ResultDto<TData> Failed(string? message, int statusCode = 400)
    {
        return new ResultDto<TData>(false, message, statusCode);
    }

    internal static ResultDto<GetOneUserDto> Succeeded(string v, List<GetOneUserDto> res)
    {
        throw new NotImplementedException();
    }

    #endregion methods

    public TData? Data { get; set; }
}
