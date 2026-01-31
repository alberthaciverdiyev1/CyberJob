using System.Net;
using System.Text.Json.Serialization;

namespace CyberJob.Core.DTOs.Common;

public class ApiResponse<T> where T : class
{
    public T? Data { get; set; }
    
    [JsonIgnore]
    public bool IsSuccess { get; set; }
    
    [JsonIgnore]
    public bool IsError { get; set; }
    
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }
    
    public List<string>? Messages { get; set; }


    public static ApiResponse<T> Success(HttpStatusCode statusCode, T data)
    {
        return new ApiResponse<T> 
        { 
            Data = data, 
            StatusCode = statusCode, 
            IsSuccess = true, 
            IsError = false 
        };
    }
    
    public static ApiResponse<T> Success(HttpStatusCode statusCode, T data, string message)
    {
        return new ApiResponse<T> 
        { 
            Data = data, 
            StatusCode = statusCode, 
            IsSuccess = true, 
            IsError = false,
            Messages = new List<string> { message }
        };
    }

    public static ApiResponse<T> Success(HttpStatusCode statusCode)
    {
        return new ApiResponse<T> 
        { 
            StatusCode = statusCode, 
            IsSuccess = true, 
            IsError = false 
        };
    }

    public static ApiResponse<T> Fail(HttpStatusCode statusCode, List<string> messages)
    {
        return new ApiResponse<T> 
        { 
            StatusCode = statusCode, 
            Messages = messages, 
            IsSuccess = false, 
            IsError = true 
        };
    }

    public static ApiResponse<T> Fail(HttpStatusCode statusCode, string message)
    {
        return new ApiResponse<T> 
        { 
            StatusCode = statusCode, 
            Messages = new List<string> { message }, 
            IsSuccess = false, 
            IsError = true 
        };
    }
}

public class ApiResponse
{
    [JsonIgnore]
    public bool IsSuccess { get; set; }
    
    [JsonIgnore]
    public bool IsError { get; set; }
    
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }
    
    public List<string>? Messages { get; set; }

    public static ApiResponse Success(HttpStatusCode statusCode) => 
        new() { StatusCode = statusCode, IsSuccess = true };

    public static ApiResponse Success(HttpStatusCode statusCode, string message) => 
        new() { StatusCode = statusCode, IsSuccess = true, Messages = new List<string> { message } };

    public static ApiResponse Fail(HttpStatusCode statusCode, string message) => 
        new() { StatusCode = statusCode, IsError = true, Messages = new List<string> { message } };

    public static ApiResponse Fail(HttpStatusCode statusCode, List<string> messages) => 
        new() { StatusCode = statusCode, IsError = true, Messages = messages };
}