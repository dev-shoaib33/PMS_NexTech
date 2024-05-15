using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PMS.API.Controllers;
using PMS.API.Models;
using PMS.API.Models.Shared;
using PMS.Common;
using PMS.Services.Interfaces;
using PMS.Services.ServiceModels;
using System.Net;

namespace PMS.Tests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _mockMapper = new Mock<IMapper>();
        var loggerFactoryMock = new Mock<ILoggerFactory>();
        loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(Mock.Of<ILogger<ProductController>>());

        _controller = new ProductController(_mockProductService.Object, _mockMapper.Object, loggerFactoryMock.Object);
    }

    #region Test Cases for GetProductById
    [Fact]
    public async Task GetProductById_WithValidId_ReturnsOk()
    {
        // Arrange
        int productId = 1;
        var productSM = new ProductSM { ProductId = productId };
        var productVM = new ProductVM { ProductId = productId };

        _mockProductService.Setup(x => x.GetProductById(productId)).Returns(productSM);
        _mockMapper.Setup(x => x.Map<ProductVM>(productSM)).Returns(productVM);

        // Act
        var result = await _controller.GetProductById(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsType<ApiResponseModel<ProductVM>>(okResult.Value);
        Assert.Equal(productId, model.data.ProductId);
    }

    [Fact]
    public async Task GetProductById_WithInvalidId_ReturnsBadRequest()
    {
        int productId = 0;

        var result = await _controller.GetProductById(productId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
    #endregion

    #region Test Cases for GetProducts
    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithData()
    {
        // Arrange
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        // Mock data to be returned by the service
        var mockData = new List<ProductSM>
    {
        new ProductSM { ProductId = 1, ProductName = "Product 1", Price = 10 },
        new ProductSM { ProductId = 2, ProductName = "Product 2", Price = 20 }
    };
        int totalCount = mockData.Count;

        productServiceMock.Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), out totalCount))
                          .Returns(mockData);

        // Setup mapper to return mapped data
        mapperMock.Setup(x => x.Map<List<ProductVM>>(It.IsAny<List<ProductSM>>()))
                  .Returns(mockData.Select(p => new ProductVM { ProductId = p.ProductId, ProductName = p.ProductName, Price = p.Price }).ToList());

        var loggerFactory = new Mock<ILoggerFactory>();
        var logger = new Mock<ILogger<ProductController>>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

        var controller = new ProductController(productServiceMock.Object, mapperMock.Object, loggerFactory.Object);

        // Act
        var result = await controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiGridResponseModel<ProductVM>>(okResult.Value);
        Assert.Equal(totalCount, apiResponse.totalCount);
        Assert.Equal(mockData.Count, apiResponse.itemList.Count());
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithNoData()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        var mockData = new List<ProductSM>();
        int totalCount = mockData.Count;

        productServiceMock.Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), out totalCount))
                          .Returns(mockData);

        mapperMock.Setup(x => x.Map<List<ProductVM>>(It.IsAny<List<ProductSM>>()))
                  .Returns(new List<ProductVM>());

       var result = await _controller.GetProducts();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiGridResponseModel<ProductVM>>(okResult.Value);
        Assert.Equal(0, apiResponse.totalCount);
        Assert.Empty(apiResponse.itemList);
    }

    [Fact]
    public async Task GetProducts_ReturnsBadRequest_WhenServiceThrowsException()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        // Setup mock service method to throw exception
        productServiceMock.Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), out It.Ref<int>.IsAny))
                          .Throws(new Exception("Service exception"));

        var loggerFactory = new Mock<ILoggerFactory>();
        var logger = new Mock<ILogger<ProductController>>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

        var controller = new ProductController(productServiceMock.Object, mapperMock.Object, loggerFactory.Object);

        var result = await controller.GetProducts();

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiGridResponseModel<ProductVM>>(badRequestResult.Value);
        Assert.Equal((int)HttpStatusCode.InternalServerError, apiResponse.code);
        Assert.Equal("Service exception", apiResponse.message);
    }
    #endregion

    #region Test Cases for CreateProduct
    [Fact]
    public async Task CreateProduct_ReturnsOkWithValidResponse()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        var vm = new ProductVM
        {
            ProductId = 1,
            ProductCode = "PROD-001",
            ProductName = "Test",
            Description = "Test",
            CategoryLkpId = 1,
            Price = 100
        };
        var expectedResponse = new ApiResponseModel<int>
        {
            code = (int)HttpStatusCode.OK,
            message = Constants.CREATE_API_SUCCESS_MSG,
            data = 123 
        };

        productServiceMock.Setup(x => x.CreateProduct(It.IsAny<ProductSM>())).Returns(123);
        mapperMock.Setup(x => x.Map<ProductSM>(It.IsAny<ProductVM>())).Returns(new ProductSM());

        var loggerFactory = new Mock<ILoggerFactory>();
        var logger = new Mock<ILogger<ProductController>>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

        var controller = new ProductController(productServiceMock.Object, mapperMock.Object, loggerFactory.Object);

        var result = await controller.CreateProduct(vm);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponseModel<int>>(okResult.Value);
        Assert.Equal(expectedResponse.code, response.code);
        Assert.Equal(expectedResponse.message, response.message);
        Assert.Equal(expectedResponse.data, response.data);
    }

    [Fact]
    public async Task CreateProduct_ReturnsBadRequest_WhenServiceThrowsException()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        var vm = new ProductVM();

        productServiceMock.Setup(x => x.CreateProduct(It.IsAny<ProductSM>())).Throws(new Exception(Constants.SYSTEM_ERROR));

        var result = await _controller.CreateProduct(vm);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponseModel<int>>(badRequestResult.Value);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.code);
        Assert.Equal(Constants.SYSTEM_ERROR, response.errorCode);
    }

    [Fact]
    public async Task CreateProduct_ReturnsOkWithValidResponse_WhenServiceReturnsPositiveValue()
    {
        // Arrange
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        var vm = new ProductVM
        {
            ProductId = 1,
            ProductCode = "PROD-001",
            ProductName = "Test",
            Description = "Test",
            CategoryLkpId = 1,
            Price = 100
        };
        var expectedResponse = new ApiResponseModel<int>
        {
            code = (int)HttpStatusCode.OK,
            message = Constants.CREATE_API_SUCCESS_MSG,
            data = 123
        };

        productServiceMock.Setup(x => x.CreateProduct(It.IsAny<ProductSM>())).Returns(123); // Return a positive value

        // Mock the mapping operation to return a ProductSM instance
        mapperMock.Setup(m => m.Map<ProductSM>(It.IsAny<ProductVM>()))
            .Returns((ProductVM vm) => new ProductSM
            {
                ProductId = vm.ProductId,
                ProductCode = vm.ProductCode,
                ProductName = vm.ProductName,
                Description = vm.Description,
                CategoryLkpId = vm.CategoryLkpId,
                Price = vm.Price
            });

        var loggerFactory = new Mock<ILoggerFactory>();
        var logger = new Mock<ILogger<ProductController>>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

        var controller = new ProductController(productServiceMock.Object, mapperMock.Object, loggerFactory.Object);

        var result = await controller.CreateProduct(vm);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponseModel<int>>(okResult.Value);
        Assert.Equal(expectedResponse.code, response.code);
        Assert.Equal(expectedResponse.message, response.message);
        Assert.Equal(expectedResponse.data, response.data);
    }

    #endregion

    #region Test Cases for UpdateProduct

    [Fact]
    public async Task UpdateProduct_ReturnsOkWithValidResponse_WhenServiceReturnsPositiveValue()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        var vm = new ProductVM
        {
            ProductId = 1, 
            ProductCode = "PROD-001",
            ProductName = "Test",
            Description = "Test",
            CategoryLkpId = 1,
            Price = 100
        };
        var expectedResponse = new ApiResponseModel<bool>
        {
            code = (int)HttpStatusCode.OK,
            message = Constants.UPDATE_API_SUCCESS_MSG,
            data = true
        };

        productServiceMock.Setup(x => x.UpdateProduct(It.IsAny<ProductSM>())).Returns(1);
        
        // Mock the mapping operation to return a ProductSM instance
        mapperMock.Setup(m => m.Map<ProductSM>(It.IsAny<ProductVM>()))
            .Returns((ProductVM vm) => new ProductSM
            {
                ProductId = vm.ProductId,
                ProductCode = vm.ProductCode,
                ProductName = vm.ProductName,
                Description = vm.Description,
                CategoryLkpId = vm.CategoryLkpId,
                Price = vm.Price
            });

        var loggerFactory = new Mock<ILoggerFactory>();
        var logger = new Mock<ILogger<ProductController>>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

        var controller = new ProductController(productServiceMock.Object, mapperMock.Object, loggerFactory.Object);
        var result = await controller.UpdateProduct(vm);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponseModel<bool>>(okResult.Value);
        Assert.Equal(expectedResponse.code, response.code);
        Assert.Equal(expectedResponse.message, response.message);
        Assert.Equal(expectedResponse.data, response.data);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsBadRequest_WhenProductIdIsInvalid()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        var vm = new ProductVM
        {
            ProductId = 0, 
            ProductCode = "PROD-001",
            ProductName = "Test",
            Description = "Test",
            CategoryLkpId = 1,
            Price = 100           
        };
        var expectedResponse = new ApiResponseModel<bool>
        {
            code = (int)HttpStatusCode.BadRequest,
            errorCode = Constants.INVALID_INPUT_PARAM,
            message = "Some Internal Server Error Occured.",
            data = false
        };

        var result = await _controller.UpdateProduct(vm);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponseModel<bool>>(badRequestResult.Value);
        Assert.Equal(expectedResponse.code, response.code);
        Assert.Equal(expectedResponse.errorCode, response.errorCode);
        Assert.Equal(expectedResponse.message, response.message);
        Assert.Equal(expectedResponse.data, response.data);
    }

    #endregion

    #region Test Cases for DeleteProduct

    [Fact]
    public async Task DeleteProduct_ReturnsOkWithValidResponse_WhenServiceDeletesProduct()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        int productId = 1; 
        var expectedResponse = new ApiResponseModel<bool>
        {
            code = (int)HttpStatusCode.OK,
            message = Constants.DELETE_API_SUCCESS_MSG,
            data = true
        };

        productServiceMock.Setup(x => x.DeleteProduct(productId)); 

        var result = await _controller.DeleteProduct(productId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponseModel<bool>>(okResult.Value);
        Assert.Equal(expectedResponse.code, response.code);
        Assert.Equal(expectedResponse.message, response.message);
        Assert.Equal(expectedResponse.data, response.data);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsBadRequest_WhenProductIdIsInvalid()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        int productId = 0; 
        var expectedResponse = new ApiResponseModel<bool>
        {
            code = (int)HttpStatusCode.BadRequest,
            errorCode = Constants.INVALID_INPUT_PARAM,
            data = false
        };

        var result = await _controller.DeleteProduct(productId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponseModel<bool>>(badRequestResult.Value);
        Assert.Equal(expectedResponse.code, response.code);
        Assert.Equal(expectedResponse.errorCode, response.errorCode);
        Assert.Equal(expectedResponse.data, response.data);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFound_WhenProductNotFound()
    {
        var productServiceMock = new Mock<IProductService>();
        var mapperMock = new Mock<IMapper>();

        int productId = 999; // A non-existent ProductId
        var expectedResponse = new ApiResponseModel<bool>
        {
            code = (int)HttpStatusCode.NotFound,
            message = Constants.PRODUCT_NOT_FOUND,
            data = false
        };

        productServiceMock.Setup(x => x.DeleteProduct(productId))
                          .Throws(new InvalidOperationException("Product not found"));
        var loggerFactory = new Mock<ILoggerFactory>();
        var logger = new Mock<ILogger<ProductController>>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

        var controller = new ProductController(productServiceMock.Object, mapperMock.Object, loggerFactory.Object);

        var result = await controller.DeleteProduct(productId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponseModel<bool>>(notFoundResult.Value);
        Assert.Equal(expectedResponse.code, response.code);
        Assert.Equal(expectedResponse.message, response.message);
        Assert.Equal(expectedResponse.data, response.data);
    }

    #endregion
}
