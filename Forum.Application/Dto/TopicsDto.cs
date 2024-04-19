using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Forum.Domain.Models;

namespace Forum.Application.Dto;

public class TopicsDto
{
    public long Id { get; set; }
    [Required]
    public string? Creator { get; set; }
    [Required]
    public long? CreatorId { get; set; }
    [Required]
    public string? Subject { get; set; }
    public TopicStatus Status { get; set; }
    public int Likes { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    
}