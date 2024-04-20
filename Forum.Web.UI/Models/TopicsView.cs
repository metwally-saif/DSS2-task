using Forum.Domain.Models;

namespace Forum.Web.UI.Models;

public class TopicsView
{
    public List<Topic> Topics { get; set; }

    public TopicsView()
    {
        Topics = new List<Topic>();
    }
}