namespace ChatAPI.Options
{
    public class GithubAuthOptions
    {
        public const string GithubAuthOption = "GitHubOAuth";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }
}
