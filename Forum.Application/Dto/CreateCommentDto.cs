using System.ComponentModel.DataAnnotations;

namespace Forum.Application.Dto;

public class CreateCommentDto
{
    public string Text { get; set; } 
    public long CreatorId { get; set; }
    public long TopicId { get; set; }
}