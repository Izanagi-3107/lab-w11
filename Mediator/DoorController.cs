/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Quản lý trạng thái khóa của cửa thoát hiểm.
namespace CampusSafetyHub.Mediator;

public sealed class DoorController
{
    public bool IsLocked { get; private set; } = true;

    public void Unlock()
    {
        IsLocked = false;
        Console.WriteLine("[DOOR] Exit door unlocked.");
    }

    public void Lock()
    {
        IsLocked = true;
        Console.WriteLine("[DOOR] Exit door locked.");
    }
}
