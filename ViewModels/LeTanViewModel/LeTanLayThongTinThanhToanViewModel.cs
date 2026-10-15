public class LeTanLayThongTinThanhToanViewModel
{
    public int Id { get; set; }                // Mã thanh toán
    public int LanKhamId { get; set; }         // Liên kết đến lần khám
    public decimal TongTien { get; set; }      // Tổng tiền thanh toán
    public DateTime NgayThanhToan { get; set; } // Ngày thanh toán
    public string TrangThai { get; set; }      // Trạng thái thanh toán

    public int MaSoKhamBenh { get; set; }      // Mã sổ khám bệnh (skb.Id)
    public string HoTen { get; set; }          // Họ tên bệnh nhân
    public string GioiTinh { get; set; }       // Giới tính bệnh nhân
}