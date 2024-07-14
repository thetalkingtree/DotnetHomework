using System.ComponentModel.DataAnnotations;

namespace DotnetHomework.Models
{

    /// <summary>
    /// Represents a Data Transfer Object (DTO) containing information about a document, including its identifier, tags, and data attributes.
    /// </summary>
    public class DocumentDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the document. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Unique identifier is required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the tags associated with the document. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Document tag/tags required")]
        public required List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the data attributes of the document. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Empty document is not allowed and document attributes are required")]
        public required DataDTO Data { get; set; }
    }
}