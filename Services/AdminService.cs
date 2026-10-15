using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Blazored.LocalStorage;

public class AdminService
{
    public List<AdminGetAllUserDTO> _lstAllUser;
    public AdminDetailViewModel _adminDetailViewModel;
    public List<RoleViewModel> _lstRole;
    public List<AdminLayThongTinThuocViewModel> _thongTinThuoc;
    public List<AdminDanhMucXetNghiemViewModel> _lstDanhMucXetNghiem;
    public AdminUpdateUserViewModel _updateUser;
    public AdminUpdateDanhMucXetNghiemViewModel _updateXetNghiem;

    private readonly ILocalStorageService _localStorage;

    private readonly HttpClient _httpClient;
    public AdminService(IHttpClientFactory httpClientFactory, AdminDetailViewModel adminDetailViewModel, List<AdminGetAllUserDTO> lstAllUser, List<RoleViewModel> lstRole, List<AdminLayThongTinThuocViewModel> thongTinThuoc, List<AdminDanhMucXetNghiemViewModel> lstDanhMucXetNghiem, AdminUpdateUserViewModel updateUser, AdminUpdateDanhMucXetNghiemViewModel updateXetNghiem, ILocalStorageService localStorage)
    {
        /**/

        _localStorage = localStorage;

        /**/
        _updateXetNghiem = updateXetNghiem;
        _updateUser = updateUser;
        _lstDanhMucXetNghiem = lstDanhMucXetNghiem;
        _thongTinThuoc = thongTinThuoc;
        _lstRole = lstRole;
        _lstAllUser = lstAllUser;
        _adminDetailViewModel = adminDetailViewModel;
        _httpClient = httpClientFactory.CreateClient("BackEndPhongKham");

    }
    /*hàm tạo ra token để gắn trước mỗi request*/
    private async Task AttachTokenAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("accessToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }


    /*Hàm call api laythongtinnguoidung theo id*/
    public async Task GetAdminById(int id)
    {
        try
        {
            await AttachTokenAsync(); // gắn token vào header
            /*Vì bên BE trả .First() nên có thể ghi ResponseData<AdminDetailViewModel>. Nếu bên BE để là .ToList() thì phải để List>ResponseData<AdminDetailViewModel>>>*/
            var res = await _httpClient.GetFromJsonAsync<ResponseData<AdminDetailViewModel>>(@$"Admin/LayThongTinNguoiDungDuaTrenId/{id}");
            if (res != null && res.content != null)
            {
                _adminDetailViewModel = res.content;
                setStateHasChange();
                Console.WriteLine(JsonSerializer.Serialize(_adminDetailViewModel));
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");
            _adminDetailViewModel = new AdminDetailViewModel();
            setStateHasChange();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
        }
    }

    /*Hàm lấy thông tin user theo id*/
    public async Task<AdminUpdateUserViewModel> GetUserById(int id)
    {
        try
        {
            /*Vì bên BE trả .First() nên có thể ghi ResponseData<AdminDetailViewModel>. Nếu bên BE để là .ToList() thì phải để List>ResponseData<AdminDetailViewModel>>>*/
            var res = await _httpClient.GetFromJsonAsync<ResponseData<AdminUpdateUserViewModel>>(@$"Admin/LayThongTinNguoiDungDuaTrenId/{id}");
            if (res != null && res.content != null)
            {
                _updateUser = res.content;
                setStateHasChange();
                Console.WriteLine(JsonSerializer.Serialize(_updateUser));
                return res.content;
            }
            return new AdminUpdateUserViewModel(); // fallback nếu không có dữ liệu
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");
            _updateUser = new AdminUpdateUserViewModel();
            setStateHasChange();
            return new AdminUpdateUserViewModel(); // fallback nếu không có dữ liệu
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            return new AdminUpdateUserViewModel(); // fallback nếu không có dữ liệu
        }
    }

    /*Hàm call api laythongtintoanbouser*/
    public async Task GetAllUser()
    {
        await AttachTokenAsync(); // gắn token vào header
        ResponseData<List<AdminGetAllUserDTO>>? res = await _httpClient.GetFromJsonAsync<ResponseData<List<AdminGetAllUserDTO>>>("Admin/LayThongTinNguoiDung");
        if (res != null && res.content != null)
        {
            _lstAllUser = res.content;
            setStateHasChange();
            Console.WriteLine(JsonSerializer.Serialize(_lstAllUser));
        }
    }

    /*Hàm call api laythongtinnguoidungtheokeyword*/
    public async Task GetUserByKeyWord(string tuKhoa)
    {
        if (string.IsNullOrWhiteSpace(tuKhoa))//bổ sung thêm
            return;// không gọi API nếu tuKhoa rỗng

        var keyword = Uri.EscapeDataString(tuKhoa);
        //bắt try catch để không lỗi not found nếu truyền tukhoa không có đúng tên trong DB
        try
        {
            var resp = await _httpClient.GetFromJsonAsync<ResponseData<List<AdminGetAllUserDTO>>>(
                $"Admin/LayThongTinNguoiDungDuaTrenTuKhoa?tuKhoa={keyword}"
            );

            if (resp != null && resp.content != null)
            {
                _lstAllUser = resp.content;
                setStateHasChange();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"API trả về lỗi HTTP: {ex.Message}");
            _lstAllUser = new List<AdminGetAllUserDTO>();
            setStateHasChange();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            // Có thể log thêm hoặc hiển thị thông báo chung
        }
    }

    /*Hàm lấy về danh sách Role*/
    public async Task GetRole()
    {
        var res = await _httpClient.GetFromJsonAsync<ResponseData<List<RoleViewModel>>>("Admin/LayThongTinQuyen");
        if (res != null && res.content != null)
        {
            _lstRole = res.content;
            setStateHasChange();
        }
    }

    /*Hàm lấy về danh sách thuốc kềm danh mục thuôcs*/
    public async Task GetThuocKemDanhMuc()
    {
        var res = await _httpClient.GetFromJsonAsync<ResponseData<List<AdminLayThongTinThuocViewModel>>>("Admin/LayThongTinThuoc");
        if (res != null && res.content != null)
        {
            _thongTinThuoc = res.content;
            setStateHasChange();
        }
    }

    /*Hàm lấy về danh sách danh mục xét nghiệm*/
    public async Task GetDanhMucXetNghiem()
    {
        var res = await _httpClient.GetFromJsonAsync<ResponseData<List<AdminDanhMucXetNghiemViewModel>>>("Admin/LayThongTinDanhMucXetNghiem");
        if (res != null && res.content != null)
        {
            _lstDanhMucXetNghiem = res.content;
            setStateHasChange();
        }
    }

    /*Hàm lấy danh mục xét nghiệm theo id*/
    public async Task<AdminUpdateDanhMucXetNghiemViewModel> GetDanhMucXetNghiemId(int id)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<ResponseData<AdminUpdateDanhMucXetNghiemViewModel>>(@$"Admin/LayThongTinDanhMucXetNghiem/{id}");
            if (res != null && res.content != null)/*nếu tìm thấy*/
            {
                _updateXetNghiem = res.content;
                setStateHasChange();
                return res.content;
            }
            return new AdminUpdateDanhMucXetNghiemViewModel();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");
            _updateXetNghiem = new AdminUpdateDanhMucXetNghiemViewModel();
            setStateHasChange();
            return new AdminUpdateDanhMucXetNghiemViewModel(); // fallback nếu không có dữ liệu
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            return new AdminUpdateDanhMucXetNghiemViewModel(); // fallback nếu không có dữ liệu
        }
    }

    /*Hàm thêm user*/
    public async Task<bool> PostUser(AdminAddNguoiDungMoiViewModel _addUser)/*Cho PostUser trả về bool hoặc một object kết quả để biết thêm có thành công hay không*/
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Admin/ThemNguoiDung", _addUser);
        if (response.IsSuccessStatusCode)
        {
            var addedUser = await response.Content.ReadFromJsonAsync<AdminAddNguoiDungMoiViewModel>();
            Console.WriteLine(@$"Thêm người dùng thành công: " + JsonSerializer.Serialize(addedUser));
            setStateHasChange(); // đảm bảo UI cập nhật, đẳm bảo chắc chắn sẽ hiện thông báo thành công hoặc thất bại
            return true;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Thêm thất bại: " + error);
            return false;
        }
    }

    /*Hàm thêm userRole cho user*/
    public async Task<bool> PostUserRole(AdminAddUserRoleViewModel _userRole)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Admin/GanRoleChoNguoiDung", _userRole);
        if (response.IsSuccessStatusCode)
        {
            var addUserRole = await response.Content.ReadFromJsonAsync<AdminAddUserRoleViewModel>();
            Console.WriteLine(@$"Thêm người dùng thành công: " + JsonSerializer.Serialize(addUserRole));
            setStateHasChange(); // đảm bảo UI cập nhật, đẳm bảo chắc chắn sẽ hiện thông báo thành công hoặc thất bại
            return true;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(@$"Thất bại +  {error}");
            return false;
        }
    }

    /*Hàm thêm Role*/
    public async Task<bool> PostRole(AdminAddNewRoleViewModel _role)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Admin/ThemRole", _role);
        if (response.IsSuccessStatusCode)
        {
            var addedRole = await response.Content.ReadFromJsonAsync<AdminAddNewRoleViewModel>();
            Console.WriteLine(@$"Thêm role thành công: " + JsonSerializer.Serialize(addedRole));
            setStateHasChange(); // đảm bảo UI cập nhật, đẳm bảo chắc chắn sẽ hiện thông báo thành công hoặc thất bại
            return true;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(@$"Thất bại + {error}");
            return false;
        }
    }

    /*Hàm thêm thuốc mới*/
    public async Task<bool> PostThuoc(AdminAddNewThuocViewModel _thuocNew)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Admin/ThemThuoc", _thuocNew);
        if (response.IsSuccessStatusCode)
        {
            var addedThuoc = await response.Content.ReadFromJsonAsync<AdminAddNewThuocViewModel>();
            Console.WriteLine(@$"Thêm role thành công: " + JsonSerializer.Serialize(addedThuoc));
            setStateHasChange(); // đảm bảo UI cập nhật, đẳm bảo chắc chắn sẽ hiện thông báo thành công hoặc thất bại
            return true;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(@$"Thất bại + {error}");
            return false;
        }
    }

    /*Hàm thêm danh mục xét nghiệm*/
    public async Task<bool> PostDanhMucXetNghiem(AdminAddDanhMucXetNghiemViewModel danhMucXetNghiem)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Admin/ThemDanhMucXetNghiem", danhMucXetNghiem);
        if (response.IsSuccessStatusCode)
        {
            var addedDanhMucXetNghiem = await response.Content.ReadFromJsonAsync<AdminAddDanhMucXetNghiemViewModel>();
            Console.WriteLine(@$"Thêm thành công: " + JsonSerializer.Serialize(addedDanhMucXetNghiem));
            setStateHasChange(); // đảm bảo UI cập nhật, đẳm bảo chắc chắn sẽ hiện thông báo thành công hoặc thất bại
            return true;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(@$"Thất bại + {error}");
            return false;
        }
    }

    /*Hàm cập nhật user*/
    public async Task<bool> UpdateUser(int idUser, AdminUpdateUserViewModel model)
    {

        HttpResponseMessage? response = await _httpClient.PutAsJsonAsync(@$"Admin/CapNhatNguoiDung/{idUser}", model);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(@$"Cập nhật người dùng thành công");
            setStateHasChange();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Cập nhật người dùng thất bại");
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(@$"Thất bại + {error}");
            return false;
        }

    }

    /*Hàm cập nhật danh mục xét nghiệm*/
    public async Task<bool> UpdateDanhMucXetNghiem(int idDanhMuc, AdminUpdateDanhMucXetNghiemViewModel danhMuc)
    {
        HttpResponseMessage? response = await _httpClient.PutAsJsonAsync(@$"Admin/CapNhatDanhMucXetNghiem/{idDanhMuc}", danhMuc);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(@$"Cập nhật danh mục xét nghiệm thành công");
            setStateHasChange();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Cập nhật danh mục xét nghiệm thất bại");
            return false;
        }
    }

    public event Action Onchange;

    public void setStateHasChange()
    {
        Onchange?.Invoke();
    }
}