using Microsoft.AspNetCore.Mvc;
using PMS.API.Controllers.Shared;
using PMS.API.Filters;
using PMS.API.Models.Shared;
using PMS.Common;
using PMS.Common.Models;
using PMS.Services.Interfaces;
using System.Net;

namespace PMS.API.Controllers;

[ServiceFilter(typeof(LoggerAttribute))]
public class MasterDataController : BaseApiController
{
    private readonly IMasterDataService _masterDataService;
    private readonly ILogger<object> _logger;
    public MasterDataController(IMasterDataService masterDataService, ILoggerFactory loggerFactory)
    {
        _masterDataService = masterDataService;
        _logger = loggerFactory.CreateLogger<object>();
    }

    #region GET
    [HttpGet("GetProductCategories")]
    public  ActionResult<ApiGridResponseModel<DropDownModel>> GetProductCategories()
    {
        var reponse = new ApiGridResponseModel<DropDownModel>();

        try
        {
            var data = _masterDataService.GetLookupsByType(LookupConstants.LKP_TYPE_PRODUCT_CATEGORY);
            if (data != null && data.Any())
            {
                return Ok(reponse.GetSuccessResponseObject(data, Constants.GET_API_SUCCESS_MSG));
            }
            else
            {
                return Ok(reponse.GetNullResponseObject());
            }
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exception thrown in MasterDataController:GetPRoductCategories. Exp: {exp.ToString()}");
            return BadRequest(reponse.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR, exp.Message));
        }
    }

    [HttpGet("GetUoms")]
    public  ActionResult<ApiGridResponseModel<DropDownModel>> GetUoms()
    {
        var reponse = new ApiGridResponseModel<DropDownModel>();

        try
        {
            var data = _masterDataService.GetLookupsByType(LookupConstants.LKP_TYPE_UOM);
            if (data != null && data.Any())
            {
                return Ok(reponse.GetSuccessResponseObject(data, Constants.GET_API_SUCCESS_MSG));
            }
            else
            {
                return Ok(reponse.GetNullResponseObject());
            }
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exception thrown in MasterDataController:GetUom. Exp: {exp.ToString()}");
            return BadRequest(reponse.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR, exp.Message));
        }
    }
    #endregion
}
