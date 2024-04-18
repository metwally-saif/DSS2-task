using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Dto;
using Forum.Application.Repositories;
using Forum.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<Comment?> GetCommentAsync(long? commentId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<CommentsDto>> GetCommentsByTopicIdAsync(long? topicId)
        {
            var comments = await Query
                .Where(e => e.TopicId == topicId)
                .Select(e => new CommentsDto
                {
                    Likes = e.Likes,
                    Text = e.Text,
                    Status = e.Status,
                    Creator = e.Creator,
                    CreatorId = e.CreatorId,
                    TopicId = e.TopicId
                })
                .ToListAsync<CommentsDto>();
            
            return comments;

        }

        public Task<(IEnumerable<CommentsDto> Comments, string? Error)> GetCommentsByUserIdAsync(long? userId)
        {
            throw new System.NotImplementedException();
        }
    }
}