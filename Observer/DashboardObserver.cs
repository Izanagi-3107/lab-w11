/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Observer hiển thị cảnh báo trên Console.
using CampusSafetyHub.Core;

namespace CampusSafetyHub.Observer;

public sealed class DashboardObserver : IAlertObserver
{
    public void Update(AlertEvent alertEvent)
    {
        Console.WriteLine(
            $"[DASHBOARD] {alertEvent.SensorName}: " +
            $"{alertEvent.Value:F1}{alertEvent.Unit} " +
            $"[{alertEvent.Severity}]");
    }
}
