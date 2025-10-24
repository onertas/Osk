namespace OskApi.Dtos.User
{
    public class UserLoginDto
    {

        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class CreateUserDto
    {

        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
