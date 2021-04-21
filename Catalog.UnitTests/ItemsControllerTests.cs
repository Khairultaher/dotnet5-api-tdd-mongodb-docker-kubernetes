using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Dtos.Item;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        private readonly Mock<IItemsRepository> repositoryStub = new();

        private readonly Mock<ILogger<ItemsController>> loggerStub = new();

        private readonly Random rand = new();

        [Fact]
        public void UnitOfWork_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFount()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var res = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            // Assert.IsType<NotFoundResult>(res.Result);
            res.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            Item expectedItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var res = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            res.Value.Should().BeEquivalentTo(expectedItem);

            //res.Value.Should().BeEquivalentTo(
            //    expectedItem,
            //    options => options.ComparingByMembers<Item>());

            //Assert.IsType<ItemDto>(res.Value);
            //var dto = (res as ActionResult<ItemDto>).Value;
            //Assert.Equal(expectedItem.Id, dto.Id);
            //Assert.Equal(expectedItem.Name, dto.Name);
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
        {
            // Arrange
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var res = await controller.GetItemsAsync();

            // Assert
            res.Should().BeEquivalentTo(expectedItems);

            //res.Should().BeEquivalentTo(
            //    expectedItems,
            //    options => options.ComparingByMembers<Item>());

            //Assert.IsType<ItemDto>(res.Value);
            //var dto = (res as ActionResult<ItemDto>).Value;
            //Assert.Equal(expectedItem.Id, dto.Id);
            //Assert.Equal(expectedItem.Name, dto.Name);
        }
        [Fact]
        public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
        {
            // Arrange
            var allItems = new[]
            {
               new Item { Name = "Potion" },
               new Item { Name = "Antidote" },
               new Item { Name = "Hi-Potion" }
            };

            var nameToMatch = "Potion";

            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(allItems);  

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

            // Assert
            foundItems.Should().OnlyContain(item => 
            item.Name == allItems[0].Name || item.Name == allItems[2].Name);
        }

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange

            //var itemToCreate = new CreateItemDto()
            //{
            //    Name = Guid.NewGuid().ToString(),
            //    Price = rand.Next(1000),
            //};
            var itemToCreate = new CreateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), rand.Next(1000));
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var res = await controller.CreateItemAsync(itemToCreate);

            // Assert
            var createdItem = (res.Result as CreatedAtActionResult).Value as ItemDto;

            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());

            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            var existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);


            //var itemToUpdate = new UpdateItemDto
            //{
            //    Name = Guid.NewGuid().ToString(),
            //    Price = existingItem.Price + 10,

            //};
            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), existingItem.Price + 10);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var res = await controller.UpdateItemAsync(itemId, itemToUpdate);

            // Assert
            res.Should().BeOfType<NoContentResult>();

        }


        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            var existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var res = await controller.DeleteItemAsync(existingItem.Id);

            // Assert
            res.Should().BeOfType<NoContentResult>();

        }

        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
