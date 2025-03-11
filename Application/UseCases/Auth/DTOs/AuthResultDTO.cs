namespace Application.Use_Cases.Auth.DTOs
{
    public class AuthResultDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
