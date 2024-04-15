namespace Forum.Web.UI.Settings
{
    public class BackendApi
    {
        public string? Address { get; set; }

        public Uri? CreateUri(string? relativePart)
        {
            if (Address is null || relativePart is null)
            {
                return null;
            }

            var baseUri = new Uri(Address, UriKind.Absolute);
            var relativeUri = new Uri(relativePart, UriKind.Relative);

            return new Uri(baseUri, relativeUri);
        }
    }
}