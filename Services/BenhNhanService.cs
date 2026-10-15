using System.Text.Json;

public class BenhNhanService
{
    public ThongTinBenhNhanTheoIdViewModel _thongTinBenhNhan;
    public List<ThongTinLichSuKhamBenhViewModel> _lichSuKhamBenh;
    public List<ThongTinKetQuaXetNghiemBenhNhanViewModel> _ketQuaXetNghiem;
    public List<ThongTinThanhToanBenhNhanViewModel> _lstThanhToan;
    public List<ThongTinLichHenViewModel> _lstThongTinLichHen;

    private HttpClient _httpClient;
    public BenhNhanService(IHttpClientFactory httpClientFactory, ThongTinBenhNhanTheoIdViewModel thongTinBenhNhan, List<ThongTinLichSuKhamBenhViewModel> lichSuKhamBenh, List<ThongTinKetQuaXetNghiemBenhNhanViewModel> ketQuaXetNghiem, List<ThongTinThanhToanBenhNhanViewModel> lstThanhToan, List<ThongTinLichHenViewModel> lstThongTinLichHen)
    {
        _lstThongTinLichHen = lstThongTinLichHen;
        _thongTinBenhNhan = thongTinBenhNhan;
        _lichSuKhamBenh = lichSuKhamBenh;
        _ketQuaXetNghiem = ketQuaXetNghiem;
        _lstThanhToan = lstThanhToan;

        _httpClient = httpClientFactory.CreateClient("BackEndPhongKham");

    }

    /*Hàm lấy thông tin bệnh nhân dựa trên id*/
    public async Task<ThongTinBenhNhanTheoIdViewModel> GetThongTinBenhNhanById(int id)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<ResponseData<ThongTinBenhNhanTheoIdViewModel>>(@$"BenhNhan/LayThongTinCaNhan/{id}");
            if (res != null && res.content != null)
            {
                _thongTinBenhNhan = res.content;
                setStateHasChange();
                return res.content;
            }
            return new ThongTinBenhNhanTheoIdViewModel();

        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");
            _thongTinBenhNhan = new ThongTinBenhNhanTheoIdViewModel();
            setStateHasChange();
            return new ThongTinBenhNhanTheoIdViewModel(); // fallback nếu không có dữ liệu
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            return new ThongTinBenhNhanTheoIdViewModel(); // fallback nếu không có dữ liệu
        }
    }

    /*Hàm lấy thông tin lịch sử khám bệnh*/
    public async Task GetLichSuKhamBenh(int idBenhNhan)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<ResponseData<List<ThongTinLichSuKhamBenhViewModel>>>(@$"BenhNhan/LayLichSuKhamBenh/{idBenhNhan}");
            if (res != null && res.content != null)
            {
                _lichSuKhamBenh = res.content;
                setStateHasChange();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");

        }
    }

    /*Hàm lấy thông tin kết quả xét nghiệm*/
    public async Task GetKetQuaXetNghiem(int idNguoiBenh)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<ResponseData<List<ThongTinKetQuaXetNghiemBenhNhanViewModel>>>(@$"BenhNhan/LayKetQuaXetNghiem/{idNguoiBenh}");
            if (res != null && res.content != null)
            {
                _ketQuaXetNghiem = res.content;
                setStateHasChange();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");

        }
    }

    /*Hàm lấy thông tin thanh toán khám bệnh*/
    public async Task GetThongTinThanhToan(int IdBenhNhan)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<ResponseData<List<ThongTinThanhToanBenhNhanViewModel>>>(@$"BenhNhan/LayThongTinThanhToan/{IdBenhNhan}");
            if (res != null && res.content != null)
            {
                _lstThanhToan = res.content;
                setStateHasChange();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");

        }
    }

    /*Hàm lấy lịch hẹn theo id bệnh nhân*/
    public async Task<List<ThongTinLichHenViewModel>> GetThongTinLichHen(int idBenhNhan)
    {
        if(idBenhNhan <= 0)
        {
            return new List<ThongTinLichHenViewModel>();// không gọi API nếu id không hợp lệ
        }

        try
        {
            /*Vì bên BE trả .First() nên có thể ghi ResponseData<AdminDetailViewModel>. Nếu bên BE để là .ToList() thì phải để List>ResponseData<AdminDetailViewModel>>>*/
            var res = await _httpClient.GetFromJsonAsync<ResponseData<List<ThongTinLichHenViewModel>>>(@$"BenhNhan/LayThongTinLichKhamTheoIdBenhNhan/{idBenhNhan}");
            if (res != null && res.content != null)
            {
                _lstThongTinLichHen = res.content;
                setStateHasChange();
                Console.WriteLine(JsonSerializer.Serialize(_lstThongTinLichHen));
                return res.content;
            }
            return new List<ThongTinLichHenViewModel>(); // fallback nếu không có dữ liệu
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(@$"API trả lỗi HTTP {ex.Message}");
            _lstThongTinLichHen = new List<ThongTinLichHenViewModel>();
            setStateHasChange();
            return new List<ThongTinLichHenViewModel>(); // fallback nếu không có dữ liệu
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khác: {ex.Message}");
            return new List<ThongTinLichHenViewModel>(); // fallback nếu không có dữ liệu
        }
    }


    public event Action Onchange;

    public void setStateHasChange()
    {
        Onchange?.Invoke();
    }
}