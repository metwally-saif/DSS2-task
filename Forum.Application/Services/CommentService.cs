using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Application.Dto;
using Forum.Application.Repositories;
using Forum.Domain.Models;

namespace Forum.Application.Services;

public class CommentService
{
    private readonly ICommentsRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    
    public CommentService(ICommentsRepository commentRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Comment?> GetCommentAsync(long? commentId)
    {
        return await _commentRepository.GetCommentAsync(commentId);
    }
    
    public async Task<List<CommentsDto>> GetCommentsByTopicIdAsync(long? Id)
    {
        return await _commentRepository.GetCommentsByTopicIdAsync(Id);
    }
    
    public async Task<(IEnumerable<CommentsDto> Comments, string? Error)> GetCommentsByUserIdAsync(long? userId)
    {
        return await _commentRepository.GetCommentsByUserIdAsync(userId);
    }
    
    public async Task<Comment?> SaveCommentAsync(CreateCommentDto comment)
    {
        var user = await _userRepository.GetByIdAsync(comment.CreatorId);
        
        var commentDto = new Comment
        {
            Creator = user!.Username ,
            CreatorId = comment.CreatorId,
            TopicId = comment.TopicId,
            Text = comment.Text,
            Likes = 0,
            Status = CommentStatus.Active
        };
        
        return await _commentRepository.SaveAsync(commentDto);
    }
    
    public async Task<Comment?> UpdateCommentAsync(Comment comment)
    {
        return await _commentRepository.UpdateAsync(comment);
    }
    
    public async Task<Comment?> DeleteCommentAsync(Comment comment)
    {
        return await _commentRepository.DeleteAsync(comment);
    }
    
}