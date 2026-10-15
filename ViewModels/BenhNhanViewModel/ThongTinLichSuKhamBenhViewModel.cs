public class ThongTinLichSuKhamBenhViewModel
{
    public int Id { get; set; }
    // public int SoKhamBenhId { get; set; }

    // Thông tin bệnh nhân
    public string HoTenBenhNhan { get; set; }
    public string GioiTinh { get; set; }

    // Thông tin lần khám
    public DateTime NgayKham { get; set; }
    public string TrieuChung { get; set; }
    public string KetLuan { get; set; }
    public string GhiChu { get; set; }

    // Thông tin bác sĩ
    public string TenBacSi { get; set; }
    // public int MaBacSi { get; set; }
}