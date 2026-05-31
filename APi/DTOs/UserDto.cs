namespace APi.DTOs
{
    public class UserDto
    {
        public string id { get; set; } = "";
        public string userName { get; set; } = "";
        public string email { get; set; } = "";
        public string phoneNumber { get; set; } = "";
    }

    public class CreateUserDto
    {
        public string userName { get; set; } = "";
        public string email { get; set; } = "";
        public string phoneNumber { get; set; } = "";
        public string password { get; set; } = "";
    }

    public class UpdateUserDto
    {
        public string userName { get; set; } = "";
        public string email { get; set; } = "";
        public string phoneNumber { get; set; } = "";
    }
}
