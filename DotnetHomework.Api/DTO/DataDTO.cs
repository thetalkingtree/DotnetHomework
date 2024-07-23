using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Document data is required")]
        public required string DocumentData { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the document. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Document mime type is required")]
        [StringLength(50, ErrorMessage = "Mime type length can't be more than 100 characters.")]
        public required string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the file name of the document. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Document filename is required")]
        [StringLength(255, ErrorMessage = "Filename length can't be more than 255 characters.")]
        public required string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file extension of the document. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Document extension is required")]
        [StringLength(5, ErrorMessage = "Filename length can't be more than 255 characters.")]
        public required string Extension { get; set; }
    }
}