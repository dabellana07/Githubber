namespace GithubUsersApi.Models
{
    public class GithubUser
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Company { get; set; }
        public int Followers { get; set; }
        public int PublicRepos { get; set; }
    }
}