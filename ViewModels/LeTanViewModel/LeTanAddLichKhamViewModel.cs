public class LeTanAddLichKhamViewModel
{
     public int Id { get; set; }                   // Khóa chính
    public int SoKhamBenhId { get; set; }         // FK tới SoKhamBenh
    public int BacSiId { get; set; }              // FK tới Bác sĩ
    public DateTime? NgayKham { get; set; }        // Ngày khám
   
}