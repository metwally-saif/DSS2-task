using System.Threading.Tasks;
using Forum.Application.Dto;
using Forum.Application.Services;
using Forum.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(
        CommentService commentService
    ) : ControllerBase
    {

        private readonly CommentService _commentService = commentService;

        [HttpGet("{TopicId}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] long? TopicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var comment = await _commentService.GetCommentsByTopicIdAsync(TopicId);

            if (comment is null)
            {
                return BadRequest(comment);
            }   
            
            if (comment.Count == 0)
            {
                return NotFound();
            }
            
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateCommentDto comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var newComment = await _commentService.SaveCommentAsync(comment);

            if (newComment is null)
            {
                return BadRequest("Comment already exists");
            }

            return Ok(newComment);
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] long? commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var comment = await _commentService.GetCommentAsync(commentId);

            if (comment is null)
            {
                return NotFound();
            }

            var deletedComment = await _commentService.DeleteCommentAsync(comment);

            if (deletedComment is null)
            {
                return BadRequest("Failed to delete comment");
            }

            return Ok(deletedComment);
        }
        
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] long? commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }
            
            var comment = await _commentService.GetCommentAsync(commentId);
            
            comment.Likes += 1;
            var updatedComment = await _commentService.UpdateCommentAsync(comment);

            if (updatedComment is null)
            {
                return BadRequest("Failed to update comment");
            }

            return Ok(updatedComment);
        }
    }
}