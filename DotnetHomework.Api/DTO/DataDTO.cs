namespace DotnetHomework.Models
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) containing information about a document.
    /// </summary>
    public class DataDTO
    {
        /// <summary>
        /// Gets or sets the document data. This field is required.
        /// </summary>
        public required string DocumentData { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the document. This field is required.
        /// </summary>
        public required string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the file name of the document. This field is required.
        /// </summary>
        public required string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file extension of the document. This field is required.
        /// </summary>
        public required string Extension { get; set; }
    }
}