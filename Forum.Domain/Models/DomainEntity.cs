using System;

namespace Forum.Domain.Models
{
    public abstract class DomainEntity : IDomainEntity
    {
        public long? Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
