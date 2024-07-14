using System.ComponentModel.DataAnnotations;

namespace DotnetHomework.Models
{
    /// <summary>
    /// Represents the data associated with a document, including file data, extension, file name, and MIME type.
    /// </summary>
    public class DocumentData
    {
        /// <summary>
        /// Gets or sets the primary key and foreign key to the document. This property is required.
        /// </summary>
        [Key]
        public int DocumentId { get; set; } // Primary Key and Foreign Key to Document

        /// <summary>
        /// Gets or sets the file data of the document.
        /// </summary>
        public string FileData { get; set; }

        /// <summary>
        /// Gets or sets the file extension of the document.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the file name of the document.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the document.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the associated document.
        /// </summary>
        public Document Document { get; set; } // Navigation property
    }
}