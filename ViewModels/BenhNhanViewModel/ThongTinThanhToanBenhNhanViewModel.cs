public class ThongTinThanhToanBenhNhanViewModel
{
    public int Id { get; set; }
    public int MaLanKham { get; set; }
    public decimal TongTien { get; set; }
    public DateTime NgayThanhToan { get; set; }
    public string TrangThai { get; set; }

    // Thông tin lần khám
    public DateTime NgayKhamBenh { get; set; }

    // Thông tin bệnh nhân
    public int MaBenhNhan { get; set; }
    public string HoTenBenhNhan { get; set; }
    public string GioiTinh { get; set; }
}