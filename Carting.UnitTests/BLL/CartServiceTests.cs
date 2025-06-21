namespace Carting.UnitTests.BLL;

using System;
using Xunit;
using NSubstitute;
using Carting.BLL.Models;
using Carting.BLL.Services;
using Carting.BLL.Interfaces;

public class CartServiceTests
{
    private readonly IRepository<Cart> _repository;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        _repository = Substitute.For<IRepository<Cart>>();
        _cartService = new CartService(_repository);
    }

    // Test Case 1: Null or Empty cartId
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AddItem_ShouldReturnFalse_WhenCartIdIsNullOrEmpty(string cartId)
    {
        // Arrange
        var item = new Item(); // Assume this is a valid item

        // Act
        var result = _cartService.AddItem(cartId, item);

        // Assert
        Assert.False(result);
    }

    // Test Case 2: Invalid item
    [Fact]
    public void AddItem_ShouldReturnFalse_WhenItemIsInvalid()
    {
        // Arrange
        var cartId = "validCartId";
        var invalidItem = new Item(); // Assume this item fails validation

        // Act
        var result = _cartService.AddItem(cartId, invalidItem);

        // Assert
        Assert.False(result);
    }


    // Test Case 3: Repository Throws Exception
    [Fact]
    public void AddItem_ShouldReturnFalse_WhenRepositoryThrowsException()
    {
        // Arrange
        var cartId = "validCartId";
        var item = new Item(); // Assume this is a valid item
        _repository.When(r => r.GetDocumentById(cartId)).Do(x => { throw new Exception(); });

        // Act
        var result = _cartService.AddItem(cartId, item);

        // Assert
        Assert.False(result);
    }

    // Test Case 4: Null or Empty cartId
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GetCart_ShouldReturnNull_WhenCartIdIsNullOrEmpty(string cartId)
    {
        // Act
        var result = _cartService.GetCart(cartId);

        // Assert
        Assert.Null(result);
        _repository.DidNotReceiveWithAnyArgs().GetDocumentById(Arg.Any<string>());
    }

    // Test Case 5: Valid cartId with Existing Cart
    [Fact]
    public void GetCart_ShouldReturnCart_WhenCartExists()
    {
        // Arrange
        var cartId = "validCartId";
        var expectedCart = new Cart { Code = cartId };
        _repository.GetDocumentById(cartId).Returns(expectedCart);

        // Act
        var result = _cartService.GetCart(cartId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCart, result);
        _repository.Received(1).GetDocumentById(cartId);
    }

    // Test Case 6: Null or Empty cartId
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void RemoveItem_ShouldReturnFalse_WhenCartIdIsNullOrEmpty(string cartId)
    {
        // Act
        var result = _cartService.RemoveItem(cartId, 1);

        // Assert
        Assert.False(result);
        _repository.DidNotReceiveWithAnyArgs().GetDocumentById(Arg.Any<string>());
    }

    // Test Case 7: Invalid itemId
    [Fact]
    public void RemoveItem_ShouldReturnFalse_WhenItemIdIsInvalid()
    {
        // Act
        var result = _cartService.RemoveItem("validCartId", -1);

        // Assert
        Assert.False(result);
        _repository.DidNotReceiveWithAnyArgs().GetDocumentById(Arg.Any<string>());
    }

    // Test Case 8: Successful Item Removal
    [Fact]
    public void RemoveItem_ShouldReturnTrue_WhenItemIsSuccessfullyRemoved()
    {
        // Arrange
        var cartId = "validCartId";
        var itemId = 1;
        var item = new Item { Id = itemId };
        var existingCart = new Cart { Id = cartId, Code = "test" };
        existingCart.AddItem(item);
        

        _repository.GetDocumentById(cartId).Returns(existingCart);
        _repository.UpdateDocument(existingCart).Returns(true);

        // Act
        var result = _cartService.RemoveItem(cartId, itemId);

        // Assert
        Assert.True(result);
        _repository.Received(1).UpdateDocument(existingCart);
    }
}

