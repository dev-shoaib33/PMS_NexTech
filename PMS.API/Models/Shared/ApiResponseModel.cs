using PMS.Common;

namespace PMS.API.Models.Shared;

public class ApiResponseModel<VM> : BaseResponseModel
{
    public VM? data { get; set; }
    public ApiResponseModel<VM> GetResponseObject(VM? data, bool isSuccess, string message, int code)
    {
        return new ApiResponseModel<VM>()
        {
            data = data,
            isSuccess = isSuccess,
            code = code,
            message = message
        };
    }
    public ApiResponseModel<VM> GetSuccessResponseObject(VM? data, string message = "Action done Successfully")
    {
        return new ApiResponseModel<VM>()
        {
            isSuccess = true,
            code = StatusCodes.Status200OK,
            message = message,
            data = data
        };
    }
    public ApiResponseModel<VM> GetErrorResponseObject(int code = 500, string? errorCode = "", string message = "Some Internal Server Error Occured.")
    {
        return new ApiResponseModel<VM>()
        {
            isSuccess = false,
            code = code,
            errorCode = errorCode,
            message = message,
            data = default(VM)
        };
    }
    public ApiResponseModel<VM> GetNullResponseObject(string message = Constants.DATA_NOT_FOUND, int code = StatusCodes.Status200OK)
    {
        return new ApiResponseModel<VM>()
        {
            isSuccess = false,
            errorCode = null,
            data = default(VM),
            message = message,
            code = code
        };
    }

}
