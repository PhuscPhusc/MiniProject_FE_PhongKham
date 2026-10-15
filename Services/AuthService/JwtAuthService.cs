/* File quản lý: Tạo Token và Verify Token */
//Đây là nơi tạo KEY/ Mã Hóa KEY, SInh ra signatrue, Tạo ra Token
/*
truy cập link: https://paper-vole-f30.notion.site/startup-cs-17590bcc6aaf8033b541d2dec1650932?pvs=4
- copy phần JwtService.cs và paste vào
- vào appsettings.json check lại phần "Jwt", đảm bảo 2 bên giống nhau, để khi khởi tạo hàm GenerateToken gửi username và role lên, chưa học database nên gửi role thay password
*/

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtAuthService //tên file phải giống tên Class
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    public JwtAuthService(IConfiguration Configuration)//check lại với appsettings.json đảm bao khớp hoàn toàn với phần "Jwt" bên đó
    {
        _key = Configuration["Jwt:Secret-Key"];
        _issuer = Configuration["Jwt:Issuer"];
        _audience = Configuration["Jwt:Audience"];

    }
    public string GenerateToken(string username, string role)//vì chưa học nên tạm thời là username và role, về sau sang backend thì sẽ dựa trên database
    {
        // Khóa bí mật để ký token
        var key = Encoding.ASCII.GetBytes(_key);
        // Tạo danh sách các claims cho token
        var claims = new List<Claim> //Đây là bước tạo ra danh sách quyền, gọi là Claims. Danh sách Claims nà gồm những quyền gì, những thông tin gì
        {
            new Claim(ClaimTypes.Name, username),               // Claim mặc định cho username
            new Claim(ClaimTypes.Role, role),                   // Claim mặc định cho Role
            new Claim(JwtRegisteredClaimNames.Sub, username),   // Subject của token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()) // Thời gian tạo token
        };
        // Tạo khóa bí mật để ký token
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),//key này lấy từ key bên file appsettings.json
            SecurityAlgorithms.HmacSha256Signature
        );
        // Thiết lập thông tin cho token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),//đưa quyền vào
            Expires = DateTime.UtcNow.AddHours(5), /* Token hết hạn sau 1 giờ. THỜI HẠN. Nếu muốn tồn tại mãi mãi thì qua bên Promgram.cs, phần cấu hình Authentication - Authorization bỏ phần thời hạn */
            SigningCredentials = credentials,
            Issuer = _issuer,                 // Thêm Issuer vào token
            Audience = _audience              // Thêm Audience vào token
        };
        // Tạo token bằng JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Trả về chuỗi token đã mã hóa - Trả về FrontEnd
        return tokenHandler.WriteToken(token);
    }

}

