using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public enum BlogStatus
    {
        Draft,
        Approved,
        Rejected
    }

    public class Blog
    {
        public Guid Id { get; set; } 
        public string? Title { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public byte[]? Banner { get; set; } = null;
        public string? Content { get; set; } = string.Empty;
        public BlogStatus Status { get; set; } = BlogStatus.Draft;

        [Required]
        public Guid CreatedById { get; set; } 

        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }

    }
}
