using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Application.Dto;
using Forum.Domain.Models;

namespace Forum.Application.Repositories;

public interface ITopicRepository : IBaseRepository<Topic>
{
    Task<Topic?> GetTopicAsync(long? topicId);

    Task<(IEnumerable<CreateTopicDto> Topics, string? Error)> GetAllTopicsAsync();

    Task<(IEnumerable<CreateTopicDto> Topics, string? Error)> GetTopicsByUserIdAsync(long? userId);
    
    
}