using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetHomework.Api.Repository;
using DotnetHomework.Models;
using Microsoft.Extensions.Logging;

namespace DotnetHomework.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository<Document> repo;
        private readonly ILogger<DocumentsController> logger;

        public DocumentsController(IDocumentRepository<Document> repo, ILogger<DocumentsController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        // GET: api/Documents/list
        /// <summary>
        /// Get the list of available documents.
        /// </summary>
        /// <returns>List of DocumentDTO</returns>
        [HttpGet]
        [Route("list")]
        public async Task<List<DocumentDTO>> GetDocumentsList()
        {
            logger.LogInformation("Fetching document list.");
            var records = await repo.GetAllAsync();
            var retVal = new List<DocumentDTO>();

            records.ForEach(elem =>
            {
                retVal.Add(new DocumentDTO
                {
                    Id = elem.Id,
                    Tags = elem.DocumentTags.Select(sel => sel.Tag.Name).ToList(),
                    Data = new DataDTO
                    {
                        Extension = elem.Data.Extension,
                        FileName = elem.Data.FileName,
                        MimeType = elem.Data.MimeType,
                        DocumentData = elem.Data.FileData
                    }
                });
            });

            logger.LogInformation($"Fetched {retVal.Count} documents.");
            return retVal;
        }

        // GET: api/Documents/5
        /// <summary>
        /// Get a specific document by ID.
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>DocumentDTO</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentDTO>> GetDocument(int id)
        {
            logger.LogInformation($"Fetching document with ID {id}.");
            var document = await repo.GetByIdAsync(id);
            if (document == null)
            {
                logger.LogWarning($"Document with ID {id} not found.");
                return NotFound();
            }

            var retVal = new DocumentDTO
            {
                Id = document.Id,
                Tags = document.DocumentTags.Select(sel => sel.Tag.Name).ToList(),
                Data = new DataDTO
                {
                    Extension = document.Data.Extension,
                    FileName = document.Data.FileName,
                    MimeType = document.Data.MimeType,
                    DocumentData = document.Data.FileData
                }
            };

            logger.LogInformation($"Fetched document with ID {id}.");
            return retVal;
        }

        // PUT: api/Documents/5
        /// <summary>
        /// Update an existing document.
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <param name="document">DocumentDTO</param>
        /// <returns>No content</returns>
        [HttpPut]
        public async Task<IActionResult> PutDocument([FromForm] DocumentDTO document)
        {
            logger.LogInformation($"Updating document with ID {document.Id}.");
            //In a repetitive scenario I would use automapper, but for this purpose it's good enough            
            try
            {
                Document doc = new Document
                {
                    Id = document.Id,
                    DocumentTags = document.Tags.Select(tag => new DocumentTag { Tag = new Tag { Name = tag } }).ToList(),
                    Data = new DocumentData
                    {
                        FileData = document.Data.DocumentData,
                        Extension = document.Data.Extension,
                        FileName = document.Data.FileName,
                        MimeType = document.Data.MimeType,
                        DocumentId = document.Id
                    }
                };
                await repo.UpdateAsync(doc);
            }
            catch (KeyNotFoundException)
            {
                logger.LogWarning($"Document with ID {document.Id} not found.");
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                logger.LogWarning($"Concurrency issue when updating document with ID {document.Id}.");
                return BadRequest();
            }
            catch (NullReferenceException ex)
            {
                logger.LogError(ex, $"Null reference exception when updating document with ID {document.Id}.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception when updating document with ID {document.Id}.");
                return StatusCode(500, ex.Message);
            }

            logger.LogInformation($"Successfully updated document with ID {document.Id}.");
            return NoContent();
        }

        // POST: api/Documents
        /// <summary>
        /// Create a new document.
        /// </summary>
        /// <param name="document">DocumentDTO</param>
        /// <returns>Created document</returns>
        [HttpPost]
        public async Task<ActionResult<DocumentDTO>> PostDocument([FromBody] DocumentDTO document)
        {
            logger.LogInformation($"Creating new document with ID {document.Id}.");
            //In a repetitive scenario I would use automapper, but for this purpose it's good enough            
            try
            {
                Document doc = new Document
                {
                    Id = document.Id,
                    DocumentTags = document.Tags.Select(tag => new DocumentTag { Tag = new Tag { Name = tag } }).ToList(),
                    Data = new DocumentData
                    {
                        FileData = document.Data.DocumentData,
                        Extension = document.Data.Extension,
                        FileName = document.Data.FileName,
                        MimeType = document.Data.MimeType
                    }
                };
                await repo.AddAsync(doc);
            }

            catch (NullReferenceException ex)
            {
                logger.LogError(ex, "Null reference exception when creating document.");
                return StatusCode(500, ex.Message);
            }

            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Error updating the document");
                return BadRequest(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception when creating document.");
                return StatusCode(500, ex.Message);
            }

            logger.LogInformation($"Successfully created document with ID {document.Id}.");
            return CreatedAtAction("PostDocument", new { id = document.Id }, document);
        }

        // DELETE: api/Documents/5
        /// <summary>
        /// Delete a document by ID.
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            logger.LogInformation($"Deleting document with ID {id}.");
            try
            {
                await repo.DeleteAsync(id);
                logger.LogInformation($"Successfully deleted document with ID {id}.");
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                logger.LogWarning($"Document with ID {id} not found.");
                return NotFound();
            }
        }
    }
}
