using DotnetHomework.Api.Controllers;
using DotnetHomework.Api.Repository;
using DotnetHomework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace DotnetHomework.Api.Tests
{
    public class DocumentsControllerTests
    {
        private readonly Mock<IDocumentRepository<Document>> mockRepo;
        private readonly Mock<ILogger<DocumentsController>> mockLogger;
        private readonly DocumentsController controller;

        public DocumentsControllerTests()
        {
            mockRepo = new Mock<IDocumentRepository<Document>>();
            mockLogger = new Mock<ILogger<DocumentsController>>();
            controller = new DocumentsController(mockRepo.Object, mockLogger.Object);
        }

        [Fact]
        public async Task GetDocumentsList_ReturnsDocumentsList()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(GetTestDocuments());
            
            // Act
            var result = await controller.GetDocumentsList();

            // Assert
            var viewResult = Assert.IsType<List<DocumentDTO>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<DocumentDTO>>(viewResult);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetDocument_ReturnsDocument()
        {
            // Arrange
            int testId = 1;
            mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(GetTestDocument(testId));

            // Act
            var result = await controller.GetDocument(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var returnValue = Assert.IsType<DocumentDTO>(actionResult.Value);
            Assert.Equal(testId, returnValue.Id);
        }

        [Fact]
        public async Task GetDocument_ReturnsNotFound()
        {
            // Arrange
            int testId = 1;
            mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync((Document)null);

            // Act
            var result = await controller.GetDocument(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutDocument_ReturnsNoContent()
        {
            // Arrange
            var documentDTO = new DocumentDTO
            {
                Id = 1,
                Tags = new List<string> { "tag1" },
                Data = new DataDTO
                {
                    Extension = ".txt",
                    FileName = "TestFile",
                    MimeType = "text/plain",
                    DocumentData = "Sample text document content"
                }
            };

            // Act
            var result = await controller.PutDocument(documentDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutDocument_ReturnsNotFound()
        {
            // Arrange
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Document>())).Throws<KeyNotFoundException>();
            var documentDTO = new DocumentDTO
            {
                Id = 1,
                Tags = new List<string> { "tag1" },
                Data = new DataDTO
                {
                    Extension = ".txt",
                    FileName = "TestFile",
                    MimeType = "text/plain",
                    DocumentData = "Sample text document content"
                }
            };

            // Act
            var result = await controller.PutDocument(documentDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PostDocument_ReturnsCreatedDocument()
        {
            // Arrange
            var documentDTO = new DocumentDTO
            {
                Id = 1,
                Tags = new List<string> { "tag1" },
                Data = new DataDTO
                {
                    Extension = ".txt",
                    FileName = "TestFile",
                    MimeType = "text/plain",
                    DocumentData = "Sample text document content"
                }
            };

            // Act
            var result = await controller.PostDocument(documentDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal("PostDocument", createdAtActionResult.ActionName);
            Assert.Equal(documentDTO.Id, ((DocumentDTO)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task DeleteDocument_ReturnsNoContent()
        {
            // Arrange
            int testId = 1;

            // Act
            var result = await controller.DeleteDocument(testId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteDocument_ReturnsNotFound()
        {
            // Arrange
            int testId = 1;
            mockRepo.Setup(repo => repo.DeleteAsync(testId)).Throws<KeyNotFoundException>();

            // Act
            var result = await controller.DeleteDocument(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        private List<Document> GetTestDocuments()
        {
            return new List<Document>
            {
                new Document
                {
                    Id = 1,
                    DocumentTags = new List<DocumentTag> { new DocumentTag { Tag = new Tag { Name = "TestTag1" } } },
                    Data = new DocumentData { Extension = ".txt", FileName = "TestFile1", MimeType = "text/plain", FileData = "Sample text document content" }
                },
                new Document
                {
                    Id = 2,
                    DocumentTags = new List<DocumentTag> { new DocumentTag { Tag = new Tag { Name = "TestTag2" } } },
                    Data = new DocumentData { Extension = ".pdf", FileName = "TestFile2", MimeType = "application/pdf", FileData = "Sample text document content" }
                }
            };
        }

        private Document GetTestDocument(int id)
        {
            return new Document
            {
                Id = id,
                DocumentTags = new List<DocumentTag> { new DocumentTag { Tag = new Tag { Name = "TestTag" } } },
                Data = new DocumentData { Extension = ".txt", FileName = "TestFile", MimeType = "text/plain", FileData = "Sample text document content" }
            };
        }
    }
}
