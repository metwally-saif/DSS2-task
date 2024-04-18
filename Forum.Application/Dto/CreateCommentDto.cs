using System.ComponentModel.DataAnnotations;

namespace Forum.Application.Dto;

public class CreateCommentDto
{
    [Required]
    public string Text { get; set; } 
    [Required]
    public long CreatorId { get; set; }
    [Required]
    public long TopicId { get; set; }
}