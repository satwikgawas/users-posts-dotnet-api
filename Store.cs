using users_api.Models;

namespace users_api
{
    public static class Store
    {
        public static readonly List<PostModifiedResponse> posts = new List<PostModifiedResponse>();
        public static readonly List<UserModifiedResponse> users = new List<UserModifiedResponse>();
    }
}
