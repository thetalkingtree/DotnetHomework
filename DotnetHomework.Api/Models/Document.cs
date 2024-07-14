using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotnetHomework.Models
{
    /// <summary>
    /// Represents a document with an identifier, associated data, and tags.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Gets or sets the unique identifier for the document.
        /// </summary>
        [Key]
        public int Id { get; set; } // Unique identifier from JSON

        /// <summary>
        /// Gets or sets the associated data for the document. Represents a one-to-one relationship with <see cref="DocumentData"/>.
        /// </summary>
        public DocumentData Data { get; set; } // One-to-One relation with DocumentData

        /// <summary>
        /// Gets or sets the collection of document tags associated with the document. Represents a many-to-many relationship with <see cref="Tag"/>.
        /// </summary>
        public ICollection<DocumentTag> DocumentTags { get; set; } // Many-to-Many relation with Tag
    }
}
