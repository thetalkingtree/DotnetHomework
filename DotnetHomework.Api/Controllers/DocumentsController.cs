using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetHomework.Api.Repository;
using DotnetHomework.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetHomework.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository<Document> repo;

        public DocumentsController(IDocumentRepository<Document> repo)
        {
            this.repo = repo;
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
            var document = await repo.GetByIdAsync(id);
            if (document == null)
            {
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

            return retVal;
        }

        // PUT: api/Documents/5
        /// <summary>
        /// Update an existing document.
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <param name="document">DocumentDTO</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument([FromForm] DocumentDTO document)
        {
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
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

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
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

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
            try
            {
                await repo.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
