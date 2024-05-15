using System.Reflection.Metadata;

namespace PMS.API.Models.Shared;

public class BaseResponseModel
{
    public bool isSuccess { get; set; }
    public string message { get; set; } = "";
    public int code { get; set; }
    public string? errorCode { get; set; } = null;
}
