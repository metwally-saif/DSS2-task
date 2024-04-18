using System.ComponentModel.DataAnnotations;
using Forum.Domain.Models;

namespace Forum.Application.Dto;

public class CreateTopicDto
{

    [Required]
    public long CreatorId { get; set; }
    [Required]
    public string? Subject { get; set; }
}