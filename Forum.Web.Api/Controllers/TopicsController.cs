using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Dto;
using Forum.Application.Services;
using Forum.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Api.Controllers
{

    [Route("api/Topics")]
    [ApiController]
    public class TopicsController(
        TopicService topicService
    ) : ControllerBase
    {
        private readonly TopicService _topicService = topicService;

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var (topics, error) = await _topicService.GetAllTopicsAsync();
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            if (!topics.Any())
            {
                return NoContent();
            }

            return Ok(topics);
        }

        [HttpGet("{topicId}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] long? topicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var topic = await _topicService.GetTopicAsync(topicId);

            if (topic is null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody, Required] CreateTopicDto topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var newTopic = await _topicService.SaveTopicAsync(topic);

            if (newTopic is null)
            {
                return BadRequest();
            }

            return Ok(newTopic);
        }

        [HttpPut("{topicId}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] long topicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }
            var topic = await _topicService.GetTopicAsync(topicId);
            var topicDto = new UpdateTopicDto()
            {
                Id = topic.Id,
                Likes = topic.Likes + 1,
            };

            var updatedTopic = await _topicService.UpdateTopicAsync(topicDto);

            if (updatedTopic is null)
            {
                return BadRequest();
            }

            return Ok(updatedTopic);
        }

        [HttpDelete("{topicId}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] long? topicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var deletedTopic = await _topicService.DeleteTopicAsync(topicId);

            if (deletedTopic is null)
            {
                return BadRequest();
            }

            return Ok(deletedTopic);
        }
        
    }
}