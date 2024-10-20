namespace mvc_app.Models
{
    public class ApiUserModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    public ApiUserModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
