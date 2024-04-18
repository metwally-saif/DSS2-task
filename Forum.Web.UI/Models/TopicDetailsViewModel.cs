using Forum.Domain.Models;

namespace Forum.Web.UI.Models;

public class TopicDetailsViewModel
{
    public long Id { get; set; }
    public string? Subject { get; set; }
    public User? Creator { get; set; }
    public List<CommentsView> Comments { get; set; } = [];
    public TopicStatus Status { get; set; }
    public int Likes { get; set; }
}