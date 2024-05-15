using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMS.API.Controllers.Shared;
using PMS.API.Filters;
using PMS.API.Models;
using PMS.API.Models.Shared;
using PMS.Common;
using PMS.Services.Interfaces;
using PMS.Services.ServiceModels;
using System.Net;

namespace PMS.API.Controllers;

/// <summary>
/// Controller for managing products.
/// </summary>
[ServiceFilter(typeof(LoggerAttribute))]
public class ProductController : BaseApiController
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ILogger<object> _logger;

    public ProductController(IProductService productService, IMapper mapper, ILoggerFactory loggerFactory)
    {
        _productService = productService;
        _mapper = mapper; 
        _logger = loggerFactory.CreateLogger<object>();
    }

    #region GET
    /// <summary>
    /// Get a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <returns>The product details.</returns>
    [HttpGet("{id}", Name = "GetProductById")]
    public async Task<ActionResult<ApiResponseModel<ProductVM>>> GetProductById(int id)
    {
        var response = new ApiResponseModel<ProductVM>();

        try
        {
            if (id <= 0)
            {
                return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.BadRequest, Constants.INVALID_INPUT_PARAM));
            }

            var data = _productService.GetProductById(id);

            if (data != null)
            {
                var d = _mapper.Map<ProductVM>(data);
                return Ok(response.GetSuccessResponseObject(_mapper.Map<ProductVM>(data), Constants.GET_API_SUCCESS_MSG));
            }
            else
            {
                return Ok(response.GetNullResponseObject());
            }
        }
        catch (Exception exp)
        {
            return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR, exp.Message));
        }
    }

    /// <summary>
    /// Get a list of products.
    /// </summary>
    /// <param name="pageSize">The number of products per page.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="searchText">Optional search text.</param>
    /// <returns>A list of products.</returns>
    [HttpGet]
    public async Task<ActionResult<ApiGridResponseModel<ProductVM>>> GetProducts(int pageSize = 10, int pageNumber = 1, string searchText = "")
    {
        var response = new ApiGridResponseModel<ProductVM>();
        int totalCount = 0;
        try
        {
            var data = _productService.GetProducts(pageSize, pageNumber, searchText, out totalCount);

            if (data != null && data.Any())
            {
                response = response.GetSuccessResponseObject(_mapper.Map<List<ProductVM>>(data), Constants.GET_API_SUCCESS_MSG);
                response.totalCount = totalCount;
                return Ok(response);
            }
            else
            {
                return Ok(response.GetNullResponseObject());
            }
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exception thrown in ProductController:GetProducts. Exp: {exp.ToString()}");
            return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR, exp.Message));
        }
    }
    #endregion

    #region POST
    /// <summary>
    /// Create a new product.
    /// </summary>
    /// <param name="vm">The product view model.</param>
    /// <returns>True if the product was created successfully.</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponseModel<int>>> CreateProduct(ProductVM vm)
    {
        var response = new ApiResponseModel<int>();

        try
        {
            int resp = _productService.CreateProduct(_mapper.Map<ProductSM>(vm));

            if (resp > 0)
                return Ok(response.GetSuccessResponseObject(resp,Constants.CREATE_API_SUCCESS_MSG));
            else
                return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR, "Failed to Update Data"));
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exception thrown in ProductController:GetProducts. Exp: {exp.ToString()}");
            return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR, exp.Message));
        }
    }
    #endregion

    #region PUT
    [HttpPut]
    public async Task<ActionResult<ApiResponseModel<bool>>> UpdateProduct(ProductVM vm)

    {
        var response = new ApiResponseModel<bool>();

        try
        {
            if (vm.ProductId <= 0)
            {
                return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.BadRequest, Constants.INVALID_INPUT_PARAM));
            }

            var updatedProduct = _mapper.Map<ProductSM>(vm);

            int resp =  _productService.UpdateProduct(updatedProduct);

            if (resp > 0)
                return Ok(response.GetSuccessResponseObject(true, Constants.UPDATE_API_SUCCESS_MSG));
            else
            {
                _logger.LogError($"Exception thrown in ProductController:UpdateProduct. Failed to update product Payload:{JsonConvert.SerializeObject(vm)}");
                return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR));
            }
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exception thrown in ProductController:UpdateProduct. Exp: {exp.ToString()}");
            return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR, exp.Message));
        }
    }

    #endregion

    #region Delete
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponseModel<bool>>> DeleteProduct(int id)
    {
        var response = new ApiResponseModel<bool>();

        try
        {
            if (id <= 0)
            {
                return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.BadRequest, Constants.INVALID_INPUT_PARAM));
            }

            _productService.DeleteProduct(id);

            return Ok(response.GetSuccessResponseObject(true, Constants.DELETE_API_SUCCESS_MSG));
        }
        catch (InvalidOperationException)
        {
            // Handle the case where the product was not found
            _logger.LogError($"Exception thrown in ProductController:DeleteProduct. Product not Found against productId: {id}");
            return NotFound(response.GetErrorResponseObject((int)HttpStatusCode.NotFound, Constants.PRODUCT_NOT_FOUND, Constants.PRODUCT_NOT_FOUND));
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exception thrown in ProductController:DeleteProduct. Exp: {exp.ToString()}");
            return BadRequest(response.GetErrorResponseObject((int)HttpStatusCode.InternalServerError, Constants.SYSTEM_ERROR, exp.Message));
        }
    }

    #endregion


}

