using PMS.Common;
namespace PMS.API.Models.Shared;

public class ApiGridResponseModel<VM> : BaseResponseModel
{
    public List<VM>? itemList { get; set; }
    public int totalCount { get; set; } = 0;
    public ApiGridResponseModel<VM> GetResponseObject(List<VM>? itemList, bool isSuccess, string message = "Action done Successfully", int code = StatusCodes.Status200OK)
    {
        return new ApiGridResponseModel<VM>()
        {
            itemList = itemList,
            isSuccess = isSuccess,
            code = code,
            message = message,
            totalCount = itemList is not null ? itemList.Count : 0

        };
    }
    public ApiGridResponseModel<VM> GetSuccessResponseObject(List<VM>? itemList, string message = "Action done Successfully")
    {
        return new ApiGridResponseModel<VM>()
        {
            isSuccess = true,
            code = StatusCodes.Status200OK,
            message = message,
            itemList = itemList,
            totalCount = itemList is not null ? itemList.Count : 0

        };
    }
    public ApiGridResponseModel<VM> GetErrorResponseObject(int code = 500, string? errorCode = "", string message = "Some Internal Server Error Occured.")
    {
        return new ApiGridResponseModel<VM>()
        {
            isSuccess = false,
            code = code,
            errorCode = errorCode,
            message = message,
            itemList = null
        };
    }
    public ApiGridResponseModel<VM> GetNullResponseObject(string message = Constants.DATA_NOT_FOUND, int code = StatusCodes.Status200OK)
    {
        return new ApiGridResponseModel<VM>()
        {
            isSuccess = false,
            errorCode = null,
            itemList = new List<VM>(),
            message = message,
            code = code
        };
    }
}
