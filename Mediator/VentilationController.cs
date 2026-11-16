/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Quản lý trạng thái hệ thống thông gió.
namespace CampusSafetyHub.Mediator;

public sealed class VentilationController
{
    public bool IsRunning { get; private set; } = true;

    public void Stop()
    {
        IsRunning = false;
        Console.WriteLine("[VENTILATION] Ventilation stopped.");
    }

    public void Start()
    {
        IsRunning = true;
        Console.WriteLine("[VENTILATION] Ventilation started.");
    }
}
