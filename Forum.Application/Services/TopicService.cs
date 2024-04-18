using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Dto;
using Forum.Application.Repositories;
using Forum.Domain.Models;

namespace Forum.Application.Services
{

    public class TopicService 
    {
    private readonly ITopicRepository _topicRepository;
    public readonly IUserRepository _userRepository;

    public TopicService(ITopicRepository topicRepository, IUserRepository userRepository)
    {
        _topicRepository = topicRepository;
        _userRepository = userRepository;
    }

    public async Task<TopicsDto> GetTopicAsync(long? id)
    {
        var topic = await _topicRepository.GetTopicAsync(id);

        return new TopicsDto
        {
            Id = topic.Id,
            Creator = topic.Creator,
            CreatorId = topic.CreatorId,
            Subject = topic.Subject,
            Status = topic.Status,
            Likes = topic.Likes,
            CreateDate = new DateTime(topic.CreateDate.Ticks, DateTimeKind.Utc),
            UpdateDate = new DateTime(topic.UpdateDate.Ticks, DateTimeKind.Utc)
        };
        
    }

    public async Task<(IEnumerable<TopicsDto> Topics, string? Error)> GetAllTopicsAsync()
    {
        var topics = await _topicRepository.GetAllAsync();
        var topicsDto = topics.Select(e => new TopicsDto
        {
            Id = e.Id,
            Creator = e.Creator,
            CreatorId = e.CreatorId,
            Subject = e.Subject,
            Status = e.Status,
            Likes = e.Likes,
            CreateDate = new DateTime(e.CreateDate.Ticks, DateTimeKind.Utc) ,
            UpdateDate = new DateTime(e.UpdateDate.Ticks, DateTimeKind.Utc)
            
        });
        return (topicsDto, null);
        
        
       
    }

    public async Task<(IEnumerable<TopicsDto> Topics, string? Error)> GetTopicsByUserIdAsync(long? userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Topic?> SaveTopicAsync(CreateTopicDto topic)
    {
        var user = await _userRepository.GetByIdAsync(topic.CreatorId);
        
        var TopicDto = new Topic()
        {
            Creator = user.Username,
            Likes = 0,
            CreatorId = topic.CreatorId,
            Subject = topic.Subject,
            Status = TopicStatus.Active,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now
        };
        return await _topicRepository.SaveAsync(TopicDto);
    }

    public async Task<Topic?> UpdateTopicAsync(UpdateTopicDto topic)
    {
        var topicDto = await _topicRepository.GetTopicAsync(topic.Id);
        topicDto.Likes = topic.Likes;
        return await _topicRepository.UpdateAsync(topicDto);
    }

    public async Task<Topic?> DeleteTopicAsync(long? topicId)
    {
        var topic = await _topicRepository.GetTopicAsync(topicId);
        return await _topicRepository.DeleteAsync(topic);
        
    }

    }
}