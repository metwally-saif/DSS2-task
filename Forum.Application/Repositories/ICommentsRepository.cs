using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Application.Dto;
using Forum.Domain.Models;

namespace Forum.Application.Repositories;

public interface ICommentsRepository : IBaseRepository<Comment>
{
    Task<Comment?> GetCommentAsync(long? commentId);

    Task<List<CommentsDto>> GetCommentsByTopicIdAsync(long? topicId);
    
    Task<(IEnumerable<CommentsDto> Comments, string? Error)> GetCommentsByUserIdAsync(long? userId);
}