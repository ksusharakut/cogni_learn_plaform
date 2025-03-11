namespace Application.Use_Cases.Auth.DTOs
{
    public class SetNewPasswordRequest
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}
