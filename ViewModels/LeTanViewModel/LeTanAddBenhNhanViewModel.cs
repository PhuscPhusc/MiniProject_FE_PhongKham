using System;
using System.ComponentModel.DataAnnotations;

public class LeTanAddBenhNhanViewModel
{
    [Required(ErrorMessage = "Id không được để trống")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
    public string HoTen { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    [DataType(DataType.Date)]
    public DateTime? NgaySinh { get; set; }

    [Required(ErrorMessage = "Giới tính không được để trống")]
    public string GioiTinh { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string SDT { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6-50 ký tự")]
    public string MatKhauHash { get; set; }
}
