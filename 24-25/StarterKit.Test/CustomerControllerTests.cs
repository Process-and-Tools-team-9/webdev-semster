using Xunit;
using Moq;
using StarterKit.Services;
using StarterKit.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly CustomerController _customerController;

    public CustomerControllerTests()
    {
        _customerServiceMock = new Mock<ICustomerService>();
        _customerController = new CustomerController(_customerServiceMock.Object);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsOk_WhenCustomerIsValid()
    {
        // Arrange
        var validCustomer = new CustomerBody
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john_doe@example.com"
        };

        _customerServiceMock.Setup(service => service.AddCustomerAsync(validCustomer))
                            .Returns(Task.CompletedTask);

        // Act
        var result = await _customerController.CreateCustomer(validCustomer);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Customer added successfully.", okResult.Value);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsBadRequest_WhenCustomerIsInvalid()
    {
        // Arrange
        var invalidCustomer = new CustomerBody
        {
            FirstName = "", // Invalid: Empty first name
            LastName = "Doe",
            Email = "john_doe@example.com"
        };

        // Act
        var result = await _customerController.CreateCustomer(invalidCustomer);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsInternalServerError_WhenServiceFails()
    {
        // Arrange
        var validCustomer = new CustomerBody
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john_doe@example.com"
        };

        _customerServiceMock.Setup(service => service.AddCustomerAsync(validCustomer))
                            .ThrowsAsync(new System.Exception("Database error"));

        // Act
        var result = await _customerController.CreateCustomer(validCustomer);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}