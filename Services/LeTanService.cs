using System.Text.Json;

public class LeTanService
{
    public List<LeTanLayThongTinBenhNhanViewModel> _lstBenhNhan;
    public LeTanUpdateBenhNhanViewModel _updateBenhNhan;
    public List<LeTanLayThongTinThanhToanViewModel> _lstThongTinThanhToan;
    public LeTanLayThongTinThanhToanViewModel _thongTinThanhToan;
    public List<LeTanLayLichHenViewModel> _lstLichKham;


    public HttpClient _httpClient;
    public LeTanService(IHttpClientFactory httpClientFactory, List<LeTanLayThongTinBenhNhanViewModel> lstBenhNhan, LeTanUpdateBenhNhanViewModel updateBenhNhan, List<LeTanLayThongTinThanhToanViewModel> lstThongTinThanhToan, LeTanLayThongTinThanhToanViewModel thongTinThanhToan, List<LeTanLayLichHenViewModel> lstLichKham)
    {
        _lstLichKham = lstLichKham;
        _lstBenhNhan = lstBenhNhan;
        _updateBenhNhan = updateBenhNhan;
        _lstThongTinThanhToan = lstThongTinThanhToan;
        _thongTinThanhToan = thongTinThanhToan;
        _httpClient = httpClientFactory.CreateClient("BackEndPhongKham");
    }


    /*Hàm lấy toàn bộ thông tin bệnh nhân*/
    public async Task GetAllBenhNhan()
    {
        ResponseData<List<LeTanLayThongTinBenhNhanViewModel>>? response = await _httpClient.GetFromJsonAsync<ResponseData<List<LeTanLayThongTinBenhNhanViewModel>>>("Receptionist/LayThongTinNguoiDungBenhNhan");
        if (response != null && response.content != null)
        {
            _lstBenhNhan = response.content;
            setStateHasChange();
            Console.WriteLine(JsonSerializer.Serialize(_lstBenhNhan));
        }
    }

    /*Hàm lấy thông tin bệnh nhân dựa trên từ khóa*/
    public async Task GetBenhNhanByKeyword(string tuKhoa)
    {
        if (string.IsNullOrWhiteSpace(tuKhoa))//bổ sung thêm
            return;// không gọi API nếu tuKhoa rỗng

        var keyword = Uri.EscapeDataString(tuKhoa);
        //bắt try catch để không lỗi not found nếu truyền tukhoa không có đúng tên trong DB
        try
        {
            var resp = await _httpClient.GetFromJsonAsync<ResponseData<List<LeTanLayThongTinBenhNhanViewModel>>>(
                $"Receptionist/LayThongTinBenhNhanDuaTrenTuKhoa?tuKhoa={tuKhoa}"
            );

            if (resp != null && resp.content != null)
            {
                _lstBenhNhan = resp.content;
                setStateHasChange();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"API trả về lỗi HTTP: {ex.Message}");
            _lstBenhNhan = new List<LeTanLayThongTinBenhNhanViewModel>();
            setStateHasChange();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            // Có thể log thêm hoặc hiển thị thông báo chung
        }
    }

    /*Hàm lấy thông tin bệnh nhân theo id*/
    public async Task<LeTanUpdateBenhNhanViewModel> GetBenhNhanById(int id)
    {
        try
        {
            /*Vì bên BE trả .First() nên có thể ghi ResponseData<AdminDetailViewModel>. Nếu bên BE để là .ToList() thì phải để List>ResponseData<AdminDetailViewModel>>>*/
            var res = await _httpClient.GetFromJsonAsync<ResponseData<LeTanUpdateBenhNhanViewModel>>(@$"Receptionist/LayThongTinBenhNhanDuaTrenId/{id}");
            if (res != null && res.content != null)
            {
                _updateBenhNhan = res.content;
                setStateHasChange();
                Console.WriteLine(JsonSerializer.Serialize(_updateBenhNhan));
                return res.content;
            }
            return new LeTanUpdateBenhNhanViewModel(); // fallback nếu không có dữ liệu
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");
            _updateBenhNhan = new LeTanUpdateBenhNhanViewModel();
            setStateHasChange();
            return new LeTanUpdateBenhNhanViewModel(); // fallback nếu không có dữ liệu
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            return new LeTanUpdateBenhNhanViewModel(); // fallback nếu không có dữ liệu
        }
    }

    /*Hàm lấy thông tin thanh toán*/
    public async Task GetThongTinThanhToan()
    {
        var res = await _httpClient.GetFromJsonAsync<ResponseData<List<LeTanLayThongTinThanhToanViewModel>>>("Receptionist/LayThongTinThanhToan");
        if (res != null && res.content != null)
        {
            _lstThongTinThanhToan = res.content;
            setStateHasChange();
            Console.WriteLine(JsonSerializer.Serialize(_lstThongTinThanhToan));
        }
    }

    /*Hàm lấy thông tin thanh tonas theo id*/
    public async Task<LeTanLayThongTinThanhToanViewModel> GetThongTinThanhToanById(int id)
    {
        try
        {
            /*Vì bên BE trả .First() nên có thể ghi ResponseData<AdminDetailViewModel>. Nếu bên BE để là .ToList() thì phải để List>ResponseData<AdminDetailViewModel>>>*/
            var res = await _httpClient.GetFromJsonAsync<ResponseData<LeTanLayThongTinThanhToanViewModel>>(@$"Receptionist/LayThongTinThanhToan/{id}");
            if (res != null && res.content != null)
            {
                _thongTinThanhToan = res.content;
                setStateHasChange();
                Console.WriteLine(JsonSerializer.Serialize(_updateBenhNhan));
                return res.content;
            }
            return new LeTanLayThongTinThanhToanViewModel(); // fallback nếu không có dữ liệu
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");
            _thongTinThanhToan = new LeTanLayThongTinThanhToanViewModel();
            setStateHasChange();
            return new LeTanLayThongTinThanhToanViewModel(); // fallback nếu không có dữ liệu
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            return new LeTanLayThongTinThanhToanViewModel(); // fallback nếu không có dữ liệu
        }
    }

    /*Hàm lấy thông tin lịch khám*/
    public async Task GetLichKham()
    {
        var res = await _httpClient.GetFromJsonAsync<ResponseData<List<LeTanLayLichHenViewModel>>>("Receptionist/LayThongTinLichKham");
        if (res != null && res.content != null)
        {
            _lstLichKham = res.content;
            setStateHasChange();
            Console.WriteLine(JsonSerializer.Serialize(_lstLichKham));
        }
    }

    /*Hàm lấy thông tin lịch khám theo id bệnh nhân*/
    public async Task<List<LeTanLayLichHenViewModel>> GetLichKhamByIdBenhNhan(int id)
    {
        if (id <= 0)//bổ sung thêm
            return new List<LeTanLayLichHenViewModel>();// không gọi API nếu id không hợp lệ

        try
        {
            /*Vì bên BE trả .First() nên có thể ghi ResponseData<AdminDetailViewModel>. Nếu bên BE để là .ToList() thì phải để List>ResponseData<AdminDetailViewModel>>>*/
            var res = await _httpClient.GetFromJsonAsync<ResponseData<List<LeTanLayLichHenViewModel>>>(@$"Receptionist/LayThongTinLichKhamTheoIdBenhNhan/{id}");
            if (res != null && res.content != null)
            {
                _lstLichKham = res.content;
                setStateHasChange();
                Console.WriteLine(JsonSerializer.Serialize(_lstLichKham));
                return res.content;
            }
            return new List<LeTanLayLichHenViewModel>(); // fallback nếu không có dữ liệu
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");
            _lstLichKham = new List<LeTanLayLichHenViewModel>();
            setStateHasChange();
            return new List<LeTanLayLichHenViewModel>(); // fallback nếu không có dữ liệu
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            return new List<LeTanLayLichHenViewModel>(); // fallback nếu không có dữ liệu
        }
    }

    /*Hàm thêm bệnh nhân mới*/
    public async Task<bool> PostBenhNhan(LeTanAddBenhNhanViewModel newBenhNhan)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Receptionist/ThemNguoiDung", newBenhNhan);
        if (response.IsSuccessStatusCode)
        {
            var addBenhNhan = await response.Content.ReadFromJsonAsync<LeTanAddBenhNhanViewModel>();
            Console.WriteLine(@$"Thêm người dùng thành công: " + JsonSerializer.Serialize(addBenhNhan));
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

    /*Hàm tạo sổ khám bệnh, có cải tiến để in lỗi trả từ message bên BE*/
    public async Task<(bool Success, string Message)> PostSoKhamBenh(LeTanAddSoKhamBenhViewModel soKhamBenhMoi)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Receptionist/TaoMoiSoKhamBenh", soKhamBenhMoi);
        if (response.IsSuccessStatusCode)
        {
            var addSoKhamBenh = await response.Content.ReadFromJsonAsync<LeTanAddSoKhamBenhViewModel>();
            Console.WriteLine(@$"Thêm sổ khám bệnh thành công: " + JsonSerializer.Serialize(addSoKhamBenh));
            setStateHasChange(); // đảm bảo UI cập nhật, đẳm bảo chắc chắn sẽ hiện thông báo thành công hoặc thất bại
            return (true, "Thêm sổ khám bệnh thành công");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Thêm thất bại: " + error);

            // Nếu API trả về JSON {"message":"..."} thì parse ra
            try
            {
                var doc = JsonDocument.Parse(error);
                var msg = doc.RootElement.GetProperty("message").GetString();
                return (false, msg ?? "Thêm sổ khám bệnh thất bại");
            }
            catch
            {
                return (false, "Thêm sổ khám bệnh thất bại");
            }
        }
    }

    /*Hàm tạo lịch Khám bệnh*/
    public async Task<(bool Success, string Message)> PostLichKham(LeTanAddLichKhamViewModel lichKhamBenhMoi)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Receptionist/TaoLanKham", lichKhamBenhMoi);
        if (response.IsSuccessStatusCode)
        {
            var addLichKhamBenh = await response.Content.ReadFromJsonAsync<LeTanAddLichKhamViewModel>();
            Console.WriteLine(@$"Thêm lịch khám bệnh thành công: " + JsonSerializer.Serialize(addLichKhamBenh));
            setStateHasChange(); // đảm bảo UI cập nhật, đẳm bảo chắc chắn sẽ hiện thông báo thành công hoặc thất bại
            return (true, "Thêm lịch khám bệnh thành công");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Thêm thất bại: " + error);

            // Nếu API trả về JSON {"message":"..."} thì parse ra
            try
            {
                var doc = JsonDocument.Parse(error);
                var msg = doc.RootElement.GetProperty("message").GetString();
                return (false, msg ?? "Thêm lịch khám bệnh thất bại");
            }
            catch
            {
                return (false, "Thêm lịch khám bệnh thất bại");
            }
        }
    }
    /*Hàm cập nhật thông tin bệnh nhân*/
    public async Task<bool> UpdateBenhNhan(int idBenhNhan, LeTanUpdateBenhNhanViewModel model)
    {

        HttpResponseMessage? response = await _httpClient.PutAsJsonAsync(@$"Receptionist/CapNhatNguoiDung/{idBenhNhan}", model);
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

    /*Hàm cập nhật trạng thái thanh toán*/
    public async Task<bool> UpdateThanhToan(int idThanhToan, LeTanUpdateThanhToanViewModel updateThanhToan)
    {
        HttpResponseMessage? response = await _httpClient.PatchAsJsonAsync($"Receptionist/CapNhatTrangThaiThanhToan/{idThanhToan}", updateThanhToan);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(@$"Cập nhật thanh toán thành công");
            setStateHasChange();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Cập nhật thanh toán thất bại");
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(@$"Thất bại: {error}");
            return false;
        }
    }









    public event Action Onchange;

    public void setStateHasChange()
    {
        Onchange?.Invoke();
    }
}