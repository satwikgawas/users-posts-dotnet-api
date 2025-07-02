namespace users_api.Models
{
    public class UserFetchResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }

    public class UserModifiedResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }

    public class UserModifiedRequest
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
