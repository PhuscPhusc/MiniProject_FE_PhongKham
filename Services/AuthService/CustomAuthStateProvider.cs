

using System.Security.Claims;                   // Xử lý thông tin người dùng
using System.Text.Json;                         // Xử lý dữ liệu JSON
using Microsoft.AspNetCore.Components.Authorization; // Quản lý xác thực
using Blazored.LocalStorage;                    // Lưu trữ token cục bộ
using System.IdentityModel.Tokens.Jwt;          // Giải mã và xử lý JWT
using System.Threading.Tasks;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;                   // Xử lý tác vụ bất đồng bộ

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
    private IConfiguration _config;

    public CustomAuthStateProvider(ILocalStorageService localStorage, IConfiguration Configuration)
    {
        _localStorage = localStorage;
        _config = Configuration;
    }

    /* Khi load lên page blazor thì lúc nào cũng chạy vào hàm bên dưới */
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Lấy token từ localStorage
        var token = await _localStorage.GetItemAsync<string>("accessToken");//"accessToken": phải khớp với accessToken bên Login.razor. Nghĩa là phải khớp với tên Key lưu trữ accessToken xuống client
        // Trường hợp không có token => Không đăng nhập
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        try
        {
            // Cấu hình kiểm tra token - Công thức check dữ liệu Token của server đối với dữ liệu localstorage lấy từ client - Token Validation
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret-Key"])),//nhớ check lại với phần Jwt của appsettings.json (Bắt buộc khớp với nhau)
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                // Chỉ định RoleClaimType và NameClaimType khớp với cấu hình
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };
            // Giải mã token để xác thực
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            // Debug claims để kiểm tra thông tin
            foreach (var claim in principal.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            // Trả về trạng thái xác thực
            return new AuthenticationState(principal);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            // Nếu token không hợp lệ => Đăng xuất
            await _localStorage.RemoveItemAsync("accessToken");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }


    public async Task MarkUserAsAuthenticated(string token)
    {
        await _localStorage.SetItemAsync("accessToken", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()//Hàm xóa token khi đăng xuất
    {
        await _localStorage.RemoveItemAsync("accessToken");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}


