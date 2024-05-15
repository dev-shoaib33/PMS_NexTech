using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PMS.API.Models.Shared;
using PMS.Common.Models;
using PMS.Common;
using PMS.Services.Interfaces;
using PMS.API.Controllers;
using Microsoft.Extensions.Logging; 

public class MasterDataControllerTests
{
    private readonly Mock<IMasterDataService> _mockMasterDataService;
    private readonly Mock<ILogger<object>> _mockLogger;
    private readonly MasterDataController _controller;

    public MasterDataControllerTests()
    {
        _mockMasterDataService = new Mock<IMasterDataService>();
        _mockLogger = new Mock<ILogger<object>>();

        var loggerFactoryMock = new Mock<ILoggerFactory>();
        loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(Mock.Of<ILogger<ProductController>>());

        _controller = new MasterDataController(_mockMasterDataService.Object, loggerFactoryMock.Object);
    }

    [Fact]
    public void GetProductCategories_ReturnsOkResultWithData()
    {
        // Arrange
        var mockData = new List<DropDownModel>
        {
            new DropDownModel { Id = 1, Name = "Category 1" },
            new DropDownModel { Id = 2, Name = "Category 2" }
        };

        _mockMasterDataService.Setup(x => x.GetLookupsByType(LookupConstants.LKP_TYPE_PRODUCT_CATEGORY)).Returns(mockData);

        // Act
        var result = _controller.GetProductCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiGridResponseModel<DropDownModel>>(okResult.Value);
        Assert.Equal((int)HttpStatusCode.OK, apiResponse.code);
        Assert.Equal(Constants.GET_API_SUCCESS_MSG, apiResponse.message);
        Assert.Equal(mockData.Count, apiResponse.itemList.Count());
    }

    [Fact]
    public void GetProductCategories_ReturnsOkResultWithNullData()
    {
        // Arrange
        _mockMasterDataService.Setup(x => x.GetLookupsByType(LookupConstants.LKP_TYPE_PRODUCT_CATEGORY)).Returns((List<DropDownModel>)null);

        // Act
        var result = _controller.GetProductCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiGridResponseModel<DropDownModel>>(okResult.Value);
        Assert.Equal((int)HttpStatusCode.OK, apiResponse.code);
        Assert.Equal(Constants.DATA_NOT_FOUND, apiResponse.message);
        Assert.Equal(apiResponse.itemList.Count(), 0);
    }

    [Fact]
    public void GetProductCategories_ReturnsBadRequestOnException()
    {
        // Arrange
        _mockMasterDataService.Setup(x => x.GetLookupsByType(LookupConstants.LKP_TYPE_PRODUCT_CATEGORY)).Throws(new Exception("Test Exception"));

        // Act
        var result = _controller.GetProductCategories();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiGridResponseModel<DropDownModel>>(badRequestResult.Value);
        Assert.Equal((int)HttpStatusCode.InternalServerError, apiResponse.code);
        Assert.Equal(Constants.SYSTEM_ERROR, apiResponse.errorCode);
    }
}
