using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Dto;
using Forum.Application.Repositories;
using Forum.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories;

internal class TopicRepository : BaseRepository<Topic>, ITopicRepository
{
    public TopicRepository(DatabaseContext context) 
        : base(context)
    {
    }


    public Task<Topic?> GetTopicAsync(long? id)
    {
        return Query
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
        
    }

    public Task<(IEnumerable<CreateTopicDto> Topics, string? Error)> GetAllTopicsAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<(IEnumerable<CreateTopicDto> Topics, string? Error)> GetTopicsByUserIdAsync(long? userId)
    {
        throw new System.NotImplementedException();
    }
    
}