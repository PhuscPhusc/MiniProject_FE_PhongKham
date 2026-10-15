public class BacSiService
{
    public List<ThongTinBenhNhanTheoMaBacSiViewModel> _lstBenhNhan;
    public List<ThongTinKetQuaXetNghiemBenhNhanFromBSViewModel> _lstKetQuaXetNghiemBenhNhan;

    public HttpClient _httpClient;
    public BacSiService(IHttpClientFactory httpClientFactory, List<ThongTinBenhNhanTheoMaBacSiViewModel> lstBenhNhan, List<ThongTinKetQuaXetNghiemBenhNhanFromBSViewModel> lstKetQuaXetNghiemBenhNhan)
    {
        _lstBenhNhan = lstBenhNhan;
        _lstKetQuaXetNghiemBenhNhan = lstKetQuaXetNghiemBenhNhan;
        _httpClient = httpClientFactory.CreateClient("BackEndPhongKham");
    }

    /*Hàm lấy thông tin các lần khám của bệnh nhân*/
    public async Task GetLayThongTinBenhNhanKhamTheoMaBacSi(int idBacSi)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<ResponseData<List<ThongTinBenhNhanTheoMaBacSiViewModel>>>(@$"Doctor/LayThongTinBenhNhanKhamTheoMaBacSi/{idBacSi}");
            if (res != null && res.content != null)
            {
                _lstBenhNhan = res.content;
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

    /*Hàm lấy thông tin kết quả xét nghiệm của bệnh nhân*/
    public async Task GetThongTinKetQuaXetNghiemBenhNhan(int IdBenhNhan)
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<ResponseData<List<ThongTinKetQuaXetNghiemBenhNhanFromBSViewModel>>>(@$"Doctor/LayKetQuaXetNghiem/{IdBenhNhan}");
            if (res != null && res.content != null)
            {
                _lstKetQuaXetNghiemBenhNhan = res.content;
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





    public event Action Onchange;

    public void setStateHasChange()
    {
        Onchange?.Invoke();
    }
}