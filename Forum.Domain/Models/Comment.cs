namespace Forum.Domain.Models
{
    public class Comment : DomainEntity
    {
        public long Id { get; set; }
        public int Likes { get; set; }
        public string? Text { get; set; }
        public CommentStatus Status { get; set; }
        public string? Creator { get; set; }
        public long? CreatorId { get; set; }
        public long? TopicId { get; set; }
    }
}
