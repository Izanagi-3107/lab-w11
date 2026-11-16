/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Giao diện chung để bộ giám sát đọc các loại cảm biến.
namespace CampusSafetyHub.Core;

public interface ISensor
{
    double ReadValue();
    string GetUnit();
}
