namespace recipe_web_api.Infrastructure.Responses
{
    public class UserAuthResponce
    {
        public AuthEnum AuthStatus { get; set; }

        public int UserId { get; set; }
        public UserAuthResponce()
        {
        }
    }
}
