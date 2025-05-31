using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs;

public class ApiResponse
{
    public object ResponseData { get; set; }
    public required string ResponseMsg { get; set; }
    public bool IsSuccess { get; set; } = false;
    public required string ResponseCode { get; set; }
}
public static class ApiResponseExtensions
{
    public static ApiResponse Success(string code, string msg, object data = null) =>
        new ApiResponse { ResponseCode = code, ResponseMsg = msg, IsSuccess = true, ResponseData = data };

    public static ApiResponse Fail(string code, string msg) =>
        new ApiResponse { ResponseCode = code, ResponseMsg = msg, IsSuccess = false };
}