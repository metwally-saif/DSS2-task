using System.Collections.Generic;

namespace Forum.Domain.Models
{
    public class Topic : DomainEntity
    {
        public  long Id { get; set; }
        public string? Creator { get; set; }
        public long? CreatorId { get; set; }
        public string? Subject { get; set; }
        public TopicStatus Status { get; set; }
        public int Likes { get; set; }
        public Comment Comment { get; set; }
    }
}
