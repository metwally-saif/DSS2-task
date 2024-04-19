using System.Security.Claims;
using System.Text;
using Forum.Application.Dto;
using Forum.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Forum.Web.UI.Controllers;

[Authorize (Roles = "Admin, User")]
public class TopicsController : Controller
{
    private readonly HttpClient _httpClient;
    
    public TopicsController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5038/"); // Base URL of API
    }
    // GET
    public async Task<IActionResult> Index()
    {
        
        var response = await _httpClient.GetAsync("api/Topics");
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var topics = await response.Content.ReadFromJsonAsync<List<Topic>>();
                return View(topics);
            }
            catch (Exception e)
            {
                return View(new List<Topic>());
            }
        }
        return View(new List<Topic>());
    }
    
    public async Task<IActionResult> List()
    {
        var response = await _httpClient.GetAsync("api/topics");
        if (response.IsSuccessStatusCode)
        {
            var topics = await response.Content.ReadFromJsonAsync<List<Topic>>();
            return View(topics);
        }
        return View();
    }
    
    public async Task<IActionResult> Create(CreateTopicDto topic)
    {
        if (!ModelState.IsValid || topic.Subject == null)
        {
            return View();
        }
        var userId = User.FindFirstValue(ClaimTypes.Sid);

        topic.CreatorId = long.Parse(userId);
 
        var response = await _httpClient.PostAsync("api/Topics",  new StringContent(JsonConvert.SerializeObject(topic), Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return View();
    }
    
    public async Task<IActionResult> Details(long id)
    {
        var response = await _httpClient.GetAsync($"api/Topics/{id}");
        var commentsResponse = await _httpClient.GetAsync($"api/Comments/{id}");
        if (response.IsSuccessStatusCode)
        {
            var topic = await response.Content.ReadFromJsonAsync<Topic>();
            var topicWithComments = new TopicWithComments() 
            { Id = topic.Id, 
                CreatorId = topic.CreatorId, 
                Subject = topic.Subject, 
                Likes = topic.Likes, 
                Creator = topic.Creator, 
                Comments = new List<Comment>()
            };
            if (commentsResponse.IsSuccessStatusCode)
            {
                var comments = await commentsResponse.Content.ReadFromJsonAsync<List<Comment>>();
                topicWithComments.Comments.AddRange(comments);
                return View(topicWithComments);
            }
            return View(topicWithComments);
        }
        return View();
    }
    
    public async Task<IActionResult> Delete(long id)
    {
        var response = await _httpClient.DeleteAsync($"api/Topics/{id}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> CreateComment(Comment comment, long Id)
    {
        if (!ModelState.IsValid || comment.Text == null)
        {
            return RedirectToAction("Details", new {id = comment.TopicId});
        }
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        var CommentDto = new CreateCommentDto()
        {
            CreatorId = long.Parse(userId),
            Text = comment.Text,
            TopicId = Id
        };
        var response = await _httpClient.PostAsync("api/Comments",  new StringContent(JsonConvert.SerializeObject(CommentDto), Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Details", new {id = Id});
        }
        return RedirectToAction("Details", new {id = Id});
    }
    
    public async Task<IActionResult> UpdateLike(long id)
    {
        long topicId = id;
        var response = await _httpClient.PutAsync($"api/Topics/{topicId}", new StringContent("", Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Details", new {id = id});
        }
        return RedirectToAction("Details", new {id = id});
    }
    
    public async Task<IActionResult> likeComment(long id, long topicId)
    {
        long commentId = id;
        var response = await _httpClient.PutAsync($"api/Comments/{commentId}", new StringContent("", Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Details", new {id = topicId});
        }
        return RedirectToAction("Details", new {id = topicId});
    }  
}