public class LeTanLayLichHenViewModel
{
    public int Id { get; set; }                  // Mã lần khám
    public int MaSoKhamBenh { get; set; }        // SoKhamBenhId
    public int MaBacSi { get; set; }             // BacSiId
    public DateTime NgayKham { get; set; }       // Ngày khám
    // public string TrieuChung { get; set; }       // Triệu chứng
    // public string KetLuan { get; set; }          // Kết luận
    // public string GhiChu { get; set; }           // Ghi chú
    public int MaBenhNhan { get; set; }          // Id bệnh nhân từ bảng SoKhamBenh
    public string HoTenBenhNhan { get; set; }
}