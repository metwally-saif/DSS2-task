using System.Collections.Generic;

namespace Forum.Domain.Models;

public class TopicWithComments : Topic
{
    public List<Comment> Comments { get; set; } = new List<Comment>();
}