using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models;

public class Blog
{
    public int Id { get; set; }
    public string? Title { get; set; }
    [DataType(DataType.Date)]
    public DateTime CreatedDate { get; set; }
    public string? Banner { get; set; }
    public string? Content { get; set; }
}