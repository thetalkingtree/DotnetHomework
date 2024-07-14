using System.ComponentModel.DataAnnotations;

namespace DotnetHomework.Models
{
    /// <summary>
    /// Represents a tag that can be associated with multiple documents.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag. This property is the primary key and is auto-generated.
        /// </summary>
        public int Id { get; set; } // Primary Key (auto-generated)

        /// <summary>
        /// Gets or sets the name of the tag. This field is required.
        /// </summary>
        [Required]
        public string Name { get; set; } // Tag name from JSON

        /// <summary>
        /// Gets or sets the collection of document tags associated with this tag. Represents a many-to-many relationship with the <see cref="Document"/> class.
        /// </summary>
        public ICollection<DocumentTag> DocumentTags { get; set; } // Many-to-Many relation with Document
    }
}
