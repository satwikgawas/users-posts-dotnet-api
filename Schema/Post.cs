namespace users_api.Models
{
    public class PostFetchResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserModifiedResponse? User { get; set; }
    }
    public class PostModifiedResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
    }

    public class PostModifiedRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
    }
}
