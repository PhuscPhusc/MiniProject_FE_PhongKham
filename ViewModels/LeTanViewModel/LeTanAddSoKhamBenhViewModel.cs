using System;
using System.ComponentModel.DataAnnotations;

public class LeTanAddSoKhamBenhViewModel
{
    [Required(ErrorMessage = "Id không được để trống")]
    public int Id { get; set; }                // Khóa chính

    [Required(ErrorMessage = "Mã bệnh nhân không được để trống")]
    public int? BenhNhanId { get; set; }       // Khóa ngoại, bắt buộc nhập

    [Required(ErrorMessage = "Ngày tạo không được để trống")]
    [DataType(DataType.Date)]
    public DateTime? NgayTao { get; set; }     // Ngày tạo, bắt buộc nhập

    [StringLength(255, ErrorMessage = "Ghi chú tối đa 255 ký tự")]
    public string? GhiChu { get; set; }        // Ghi chú, có thể null
}
