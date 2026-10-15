public class LoginResponse
{
    public string message { get; set; }
    public TokenData token { get; set; }
}

public class TokenData
{
    public string result { get; set; }
    public int id { get; set; }
    // các field khác nếu cần
}
