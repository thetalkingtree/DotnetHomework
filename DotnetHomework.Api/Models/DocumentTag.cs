namespace DotnetHomework.Models
{
    /// <summary>
    /// Represents the many-to-many relationship between documents and tags.
    /// </summary>
    public class DocumentTag
    {
        /// <summary>
        /// Gets or sets the foreign key to the document. This property is required.
        /// </summary>
        public int DocumentId { get; set; } // Foreign Key to Document

        /// <summary>
        /// Gets or sets the document associated with this relationship.
        /// </summary>
        public Document Document { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the tag. This property is required.
        /// </summary>
        public int TagId { get; set; } // Foreign Key to Tag

        /// <summary>
        /// Gets or sets the tag associated with this relationship.
        /// </summary>
        public Tag Tag { get; set; }
    }
}