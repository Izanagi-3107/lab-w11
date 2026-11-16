/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Hợp đồng chung cho các đối tượng đăng ký nhận cảnh báo.
using CampusSafetyHub.Core;

namespace CampusSafetyHub.Observer;

public interface IAlertObserver
{
    void Update(AlertEvent alertEvent);
}
