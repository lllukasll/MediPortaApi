using MediPortaApi.Entities;
using MediPortaApi.Repositories.Interfaces;
using MediPortaApi.Services.Interfaces;
using MediPortaApi.Services;
using Moq;

namespace MediPortaApi.Test
{
    public class UnitTest
    {
        [Fact]
        public async Task GetTagsAsync_ReturnsListOfTags()
        {
            // Arrange
            var mockRepository = new Mock<ITagRepository>();
            mockRepository.Setup(repo => repo.GetTagsAsync(It.IsAny<string>(), It.IsAny<bool>()))
                          .ReturnsAsync(new List<Tag> { new Tag { Name = "tag1", Count = 10, Percentage = 20 } });
            var mockStackOverflowService = new Mock<IStackOverflowAPIService>();
            var service = new TagService(mockRepository.Object, mockStackOverflowService.Object);

            // Act
            var result = await service.GetTagsAsync("name", false);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("tag1", result[0].Name);
            Assert.Equal(10, result[0].Count);
            Assert.Equal(20, result[0].Percentage);
        }

        [Fact]
        public async Task RefreshTagsAsync_ClearsAndAddsTags()
        {
            // Arrange
            var mockRepository = new Mock<ITagRepository>();
            var mockStackOverflowAPIService = new Mock<IStackOverflowAPIService>();
            mockStackOverflowAPIService.Setup(service => service.GetTagsAsync())
                                       .ReturnsAsync(new List<Tag> { new Tag { Name = "tag1", Count = 10, Percentage = 20 } });
            var service = new TagService(mockRepository.Object, mockStackOverflowAPIService.Object);

            // Act
            await service.RefreshTagsAsync();

            // Assert
            mockRepository.Verify(repo => repo.RefreshTagsAsync(It.IsAny<List<Tag>>()), Times.Once);
        }
    }
}