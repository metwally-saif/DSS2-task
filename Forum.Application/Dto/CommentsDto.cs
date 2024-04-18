using Forum.Domain.Models;

namespace Forum.Application.Dto;

public class CommentsDto
{
    public int Likes { get; set; }
    public string? Text { get; set; }
    public CommentStatus Status { get; set; }
    public string? Creator { get; set; }
    public long? CreatorId { get; set; }
    public long? TopicId { get; set; }
}