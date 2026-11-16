/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Quản lý trạng thái và thao tác với chuông báo động.
namespace CampusSafetyHub.Mediator;

public sealed class Alarm
{
    public bool IsActive { get; private set; }

    public void Activate()
    {
        IsActive = true;
        Console.WriteLine("[ALARM] Fire alarm activated.");
    }

    public void Deactivate()
    {
        IsActive = false;
        Console.WriteLine("[ALARM] Alarm deactivated.");
    }
}
