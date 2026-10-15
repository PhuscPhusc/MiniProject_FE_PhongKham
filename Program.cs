using System.Security.Claims;
using System.Text;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
//Phần build
builder.Services.AddRazorPages();//DI thư viện razor web
builder.Services.AddServerSideBlazor();//DI thư viện Server Side


// //Khai báo httpfactory để gọi api cho domain: http://localhost:5082/api/ đặt tên là BackEndPhongKham
builder.Services.AddHttpClient();/*có cái này mới dùng được IHttpFactory mà không cần khai báo lại*/
builder.Services.AddHttpClient("BackEndPhongKham", options =>
{
    options.BaseAddress = new Uri("http://localhost:5082/api/");
    options.Timeout = TimeSpan.FromSeconds(30);
    options.DefaultRequestHeaders.Add("Accept", "application/json");
});

//DI ViewModel Admin
builder.Services.AddScoped<AdminDetailViewModel>();

//DI List<AdminDetailViewModel>
builder.Services.AddScoped<List<AdminDetailViewModel>>();

//DI List<AdminGetAllUserDTO>
builder.Services.AddScoped<List<AdminGetAllUserDTO>>();

//DI Viewmodel Role và list<RoleViewModel>
builder.Services.AddScoped<RoleViewModel>();
builder.Services.AddScoped<List<RoleViewModel>>();

//DI AdminLayThongTinThuocViewModel và List<AdminLayThongTinThuocViewModel>
builder.Services.AddScoped<AdminLayThongTinThuocViewModel>();
builder.Services.AddScoped<List<AdminLayThongTinThuocViewModel>>();

//DI AdminDanhMucXetNghiemViewModel và List<AdminDanhMucXetNghiemViewModel>()
builder.Services.AddScoped<AdminDanhMucXetNghiemViewModel>();
builder.Services.AddScoped<List<AdminDanhMucXetNghiemViewModel>>();

//DI AdminAddNguoiDungMoiViewModel
builder.Services.AddScoped<AdminAddNguoiDungMoiViewModel>();

//DI AdminAddUserRoleViewModel
builder.Services.AddScoped<AdminAddUserRoleViewModel>();

//DI AdminAddNewRoleViewModel
builder.Services.AddScoped<AdminAddNewRoleViewModel>();

//DI AdminAddNewThuoc
builder.Services.AddScoped<AdminAddNewThuocViewModel>();

//DI AdminAddDanhMucXetNghiemViewModel
builder.Services.AddScoped<AdminAddDanhMucXetNghiemViewModel>();

//DI Service AdminUpdateUser
builder.Services.AddScoped<AdminUpdateUserViewModel>();

//DI AdminUpdateDanhMucXetNghiemViewModel
builder.Services.AddScoped<AdminUpdateDanhMucXetNghiemViewModel>();

//DI Service Admin
builder.Services.AddScoped<AdminService>();

//DI ThongTinBenhNhanTheoIdViewModel
builder.Services.AddScoped<ThongTinBenhNhanTheoIdViewModel>();

//DI ThongTinLichSuKhamBenhViewModel và List<ThongTinLichSuKhamBenhViewModel>
builder.Services.AddScoped<ThongTinLichSuKhamBenhViewModel>();
builder.Services.AddScoped<List<ThongTinLichSuKhamBenhViewModel>>();

//DI ThongTinKetQuaXetNghiemBenhNhanViewModel và List<ThongTinKetQuaXetNghiemBenhNhanViewModel>
builder.Services.AddScoped<ThongTinKetQuaXetNghiemBenhNhanViewModel>();
builder.Services.AddScoped<List<ThongTinKetQuaXetNghiemBenhNhanViewModel>>();

//DI ThongTinThanhToanBenhNhanViewModel và List<ThongTinThanhToanBenhNhanViewModel>
builder.Services.AddScoped<ThongTinThanhToanBenhNhanViewModel>();
builder.Services.AddScoped<List<ThongTinThanhToanBenhNhanViewModel>>();

//DI ThongTinLichHenViewModel và List<ThongTinLichHenViewModel>
builder.Services.AddScoped<ThongTinLichHenViewModel>();
builder.Services.AddScoped<List<ThongTinLichHenViewModel>>();

//DI Service BenhNhan
builder.Services.AddScoped<BenhNhanService>();


//DI ThongTinBenhNhanTheoMaBacSiViewModel và List<ThongTinBenhNhanTheoMaBacSiViewModel>
builder.Services.AddScoped<ThongTinBenhNhanTheoMaBacSiViewModel>();
builder.Services.AddScoped<List<ThongTinBenhNhanTheoMaBacSiViewModel>>();

//DI ThongTinKetQuaXetNghiemBenhNhanFromBSViewModel và List<ThongTinKetQuaXetNghiemBenhNhanFromBSViewModel>
builder.Services.AddScoped<ThongTinKetQuaXetNghiemBenhNhanFromBSViewModel>();
builder.Services.AddScoped<List<ThongTinKetQuaXetNghiemBenhNhanFromBSViewModel>>();

//DI Service BacSi
builder.Services.AddScoped<BacSiService>();

//DI LeTanAddBenhNhanViewModel
builder.Services.AddScoped<LeTanAddBenhNhanViewModel>();

//DI LeTanUpdateBenhNhanViewModel
builder.Services.AddScoped<LeTanUpdateBenhNhanViewModel>();

//DI LeTanLayThongTinBenhNhanViewModel và List<LeTanLayThongTinBenhNhanViewModel>
builder.Services.AddScoped<LeTanLayThongTinBenhNhanViewModel>();
builder.Services.AddScoped<List<LeTanLayThongTinBenhNhanViewModel>>();

//DI LeTanAddSoKhamBenhViewModel
builder.Services.AddScoped<LeTanAddSoKhamBenhViewModel>();

//DI LeTanAddLichKhamViewModel
builder.Services.AddScoped<LeTanAddLichKhamViewModel>();

//DI LeTanLayThongTinThanhToanViewModel và List<LeTanLayThongTinThanhToanViewModel>
builder.Services.AddScoped<LeTanLayThongTinThanhToanViewModel>();
builder.Services.AddScoped<List<LeTanLayThongTinThanhToanViewModel>>();

//DI LeTanLayLichHenViewModel và List<LeTanLayLichHenViewModel>
builder.Services.AddScoped<LeTanLayLichHenViewModel>();
builder.Services.AddScoped<List<LeTanLayLichHenViewModel>>();

//DI Service LeTan
builder.Services.AddScoped<LeTanService>();


////////////////////////////////////////////////

//DI Service Login và ViewModel login
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<LoginService>();




//Cấu hình Authentication + Authorization - cấu hình accessToken JWT - BACKEND cấu hình tương tự, đây là đang cấu hình cho server
// 1. Đọc key, issuer và audience từ appsettings.json
var privateKey = builder.Configuration["Jwt:Secret-Key"];
var Issuer = builder.Configuration["Jwt:Issuer"];
var Audience = builder.Configuration["Jwt:Audience"];
// 2. Cấu hình Authentication sử dụng JWT Bearer
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>//Thêm builder  //.AddAuthentication("Bearer"):thêm chuẩn mã hóa Token
{
    options.TokenValidationParameters = new TokenValidationParameters//sau khi cài 2 thư viện dotnet thì using thư viện vào
    {

        ValidateIssuerSigningKey = true, // Xác thực key bí mật của token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)), //Đây là hàm để mã hóa KEY/ Tạo ra KEY, đây là hàm giúp tạo ra phần signature (phần chữ ký)
        /* Nếu trong appsettings.json có Issuer và Audience thì thêm 4 dòng dưới, không thì bỏ đi */
        ValidateIssuer = true,// Xác thực Issuer 
        ValidIssuer = Issuer, // Phải khớp với Issuer trong token
        ValidateAudience = true,    // Xác thực Audience
        ValidAudience = Audience, // Phải khớp với Audience trong token
        /**/
        ValidateLifetime = true, // Xác thực thời gian hết hạn của token    //Nếu muốn Token chạy mãi thì bỏ dòng này
        ClockSkew = TimeSpan.Zero, // Bỏ qua độ trễ thời gian giữa server và client (ngăn lỗi thời gian)
        RoleClaimType = ClaimTypes.Role, // Ánh xạ claim role, //Đây là nơi bỏ quyền role, có bài BackEnd hướng dẫn custom lại (tại bài action filter). Nếu đổi tên ClaimTypes thi BẮT BUỘC PHẢI VIẾT FILE CUSTOM bằng tay lại. Tạm thời cứ xài mặc định
        NameClaimType = "UserName",  //Thích đặt gì đặt
    };
});
// 4. Thêm AuthorizationCore để sử dụng trong Blazor Components (Phần view)
builder.Services.AddAuthorizationCore();
/* 4 phần này là cấu hình cho chuẩn Token, chuẩn xác thực như thế nào. ĐÂY LÀ CẤU HÌNH TRÊN SERVER, check lỗi dùng Dotnet build, không báo gì là ok */

//DI JwtAuthService
builder.Services.AddScoped<JwtAuthService>();

//DI Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

//DI đè lại phương thức xác thực của Blazor
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();//nghĩa lại nếu gặp AuthenticationStateProvider thì nó sẽ chạy cái hàm CustomAuthStateProvider của mình
////////////////////////////////////////////////


var app = builder.Build();
//Phần use
app.UseHttpsRedirection();
app.MapBlazorHub();//Middleware của blazor để làm file chạy đầu tiên - Bật middleware MapBlazorHub để xác định file nào là file chạy đầu tiên
app.MapFallbackToPage("/_Host");//Nơi cấu hình để quyết định đâu là file được bật lên đầu tiên
app.UseRouting();// chia các component thành route @page '/duong-dan'
app.UseStaticFiles();//middleware  để sử dụng được file tĩnh như css, js, img,....(nói chung nếu muốn dùng các file trong folder wwwroot(css, img,....)) -> BẮT BUỘC PHẢI CÓ app.UseStaticFiles();

//Ví dụ: có thể vào được blackcar thông qua http://localhost:5188/products/black-car.jpg


/* use 2 cái cho 4 phần cấu hình bên trên */
app.UseAuthentication(); //Xác thực
app.UseAuthorization(); //Phân quyền

app.Run();

