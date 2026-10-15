public class ThongTinBenhNhanTheoMaBacSiViewModel
{
    public int Id { get; set; }                  // Mã lần khám
    public int SoKhamBenhId { get; set; }        // FK tới sổ khám bệnh
    public int BacSiId { get; set; }             // FK tới bác sĩ
    public string TenBacSi { get; set; }         // Họ tên bác sĩ
    public DateTime NgayKham { get; set; }       // Ngày khám
    public string? TrieuChung { get; set; }      // Triệu chứng
    public string? KetLuan { get; set; }         // Kết luận
    public string? GhiChu { get; set; }          // Ghi chú
    public int MaSoKhamBenh { get; set; }        // Mã sổ khám bệnh
    public int MaBenhNhan { get; set; }          // Mã bệnh nhân
    public string HoTenBenhNhan { get; set; }    // Họ tên bệnh nhân
}